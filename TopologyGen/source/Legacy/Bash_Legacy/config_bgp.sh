config_bgp(){
  echo -e ${YELLOW}"Setting ${BLUE}bgp ${YELLOW}protocol"${NC}
  allRouters=`ls | grep router`
  allOob=`ls | grep oob`
  totalRouters=$allRouters" "$allOob     #encontra todos as maquinas roteadoras
	for eachRouter in $totalRouters; do    #para cada maquina encontrada, implementar os comandos de bgp
		thisFile=${eachRouter}_setup.txt     #arquivo no qual os comandos serão salvos
    read -a allInterfaces <<< $(cat $eachRouter | awk '{print $3}' | awk '{a=$0;printf "%s ",a,$0}')    #bash cria array assim
    read -a allIps <<< $(cat $eachRouter | awk '{print $2}' | awk '{a=$0;printf "%s ",a,$0}')
    sizeArray=${#allInterfaces[@]}
		cp ../../Host_Scripts/bgpd.conf_template.txt ../BGP_Information/${eachRouter}bgpd.conf #copia para a pasta BGP_information uma copia do tamplate para cada roteador
		cp ../../Host_Scripts/zebra.conf_template.txt ../BGP_Information/${eachRouter}zebra.conf
		thisBGPFile="../BGP_Information/${eachRouter}bgpd.conf"        #determina o arquivo que será copiado pelo roteador assim como arquivos auxiliares
		auxBGPFile="../BGP_Information/${eachRouter}bgpd.confAux"
		thisZebraFile="../BGP_Information/${eachRouter}zebra.conf"
		auxZebraFile="../BGP_Information/${eachRouter}zebra.confAux"
		for (( index=0; index < $sizeArray; index=index+1 )); do  #para cada interface de rede
				bgpFileLineBase=`cat ../BGP_Information/${eachRouter}bgpd.conf | grep -n router-id | sed 's/:.*//'` #encontra a linha cujas infos de rede se encontram abaixo
				bgpFileLineTarget=$((bgpFileLineBase+1))                                                            #incrementa uma linha para escrever na linha de baixo
				currentNetwork=`echo ${allIps[index]} | sed -r 's/(([0-9]{1,3}\.){3})[0-9]{1,3}/\10/g'`				      #encontra a rede correspondente a porta e a nomeia com .0
				thisText="network $currentNetwork"                                                                  #determina o texto a ser escrito
				sed "$bgpFileLineTarget i $thisText" $thisBGPFile > $auxBGPFile                                     #escreve no arquivo auxiliar na linha determinada
				mv $auxBGPFile $thisBGPFile                                                                         #arquivo auxiliar se torna arquivo principal

				zebraFileLineBase=`cat ../BGP_Information/${eachRouter}zebra.conf | grep -n enable | sed 's/:.*//'` #procedimento abaixo é o mesmo que o acima, mas para zebra
				zebraFileLineTarget=$((zebraFileLineBase+1))
				currentNetwork=${allIps[index]}
				thisText="interface ${allInterfaces[index]}"
				sed "$zebraFileLineTarget i $thisText" $thisZebraFile > $auxZebraFile
				mv $auxZebraFile $thisZebraFile

				thisText="ip address $currentNetwork"
				zebraFileLineTarget=$((zebraFileLineBase+2))
				sed "$zebraFileLineTarget i $thisText" $thisZebraFile > $auxZebraFile
				mv $auxZebraFile $thisZebraFile

		done
		echo apt-get install quagga >> ../$thisFile                  #escreve no arquivo de config para baixar o quagga
		echo sudo sysctl -w net.ipv4.ip_forward=1 >> ../$thisFile    #habilita forward
		echo "sudo cat $thisBGPFile > /etc/quagga/bgpd.conf" >> ../$thisFile #escreve o arquivo gerado por cima do arquivo default do quagga
		echo "sudo cat $thisZebraFile > /etc/quagga/zebra.conf" >> ../$thisFile
		echo sudo service zebra start >> ../$thisFile                #inicia os daemons
		echo sudo service bgpd start >> ../$thisFile
	done
}

main() {
  config_bgp
}

if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
