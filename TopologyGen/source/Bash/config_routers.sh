config_routers(){
  echo -e ${YELLOW}"Setting routers machines as ${BLUE}routers"${NC}
  allRouters=`ls | grep router`
  allOob=`ls | grep oob`
  totalRouters=$allRouters" "$allOob
  turno=0
    for eachRouter in $totalRouters; do
        thisFile=${eachRouter}_setup.txt
        echo sudo sysctl -w net.ipv4.ip_forward=1 >> ../$thisFile  #habilita forward
        echo sudo ip route del 0/0 >> ../$thisFile                 #deleta default gateway
        read -a allInterfaces <<< $(cat $eachRouter | awk '{print $3}' | awk '{a=$0;printf "%s ",a,$0}')    #bash cria array assim
        read -a allIps <<< $(cat $eachRouter | awk '{print $2}' | awk '{a=$0;printf "%s ",a,$0}')
        sizeArray=${#allInterfaces[@]}
        for (( index=0; index < $sizeArray; index=index+1 )); do  #varre o array de ips para configurar a tabela de roteamento
          currentNetwork=`echo ${allIps[index]} | sed -r 's/(([0-9]{1,3}\.){3})[0-9]{1,3}/\10/g'`
          echo sudo ip route add $currentNetwork dev ${allInterfaces[index]} >> ../$thisFile
        done
        for (( index=0; index < $sizeArray; index=index+1 )); do #cria default gateway para os roteadores e oobs
          if [[ ${allInterfaces[index]} == eth50 ]]; then        #testa se é porta 50, que é a porta de comunicação entre intralevels com a 49, ou entre router e oob
            if [[ $eachRouter == *router* && $oob == 0 ]]; then  #se não existe oob então meu vizinho vai ter ip 2, pois eu sou o 1
              currentNetwork=`echo ${allIps[index]} | sed -r 's/(([0-9]{1,3}\.){3})[0-9]{1,3}/\12/g' | sed 's/\/24//'`
            elif  [[ $eachRouter == *router* ]]; then            #no cenario que existe oob, se eu sou o roteador, então o vizinho da porta 50 tera ip 1
               currentNetwork=`echo ${allIps[index]} | sed -r 's/(([0-9]{1,3}\.){3})[0-9]{1,3}/\11/g' | sed 's/\/24//'`
            else                                                 #entretanto, se sou o oob, então meu vizinho terá ip 2
               currentNetwork=`echo ${allIps[index]} | sed -r 's/(([0-9]{1,3}\.){3})[0-9]{1,3}/\12/g' | sed 's/\/24//'`
            fi
            echo sudo ip route add 0/0 via $currentNetwork dev eth50 >> ../$thisFile
            turno=1                                              #cria pares de maquinas 49 - 50 para evitar maquina no meio com dois enlaces
          fi
          if [[ ${allInterfaces[index]} == eth49 && $turno == 1 ]]; then #quando encontro uma porta 49, meu vizinho 50 sempre terá ip terminado em 1
            currentNetwork=`echo ${allIps[index]} | sed -r 's/(([0-9]{1,3}\.){3})[0-9]{1,3}/\11/g' | sed 's/\/24//'`
            echo sudo ip route add 0/0 via $currentNetwork dev eth49 >> ../$thisFile
            index=$sizeArray;
            turno=0
          fi
        done
    done
}

main() {
  config_routers
}

if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
