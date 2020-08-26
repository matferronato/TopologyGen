config_vlan(){
  echo -e ${YELLOW}"Adding ${BLUE}vlan ${YELLOW}protocol" ${NC}
  allSwitchs=`ls | grep switch`       #encontra todos os switchs disponiveis
  for eachSwitch in $allSwitchs; do   #para cada switch da lista de switchs
    thisFile=${eachSwitch}_setup.txt
    echo sudo ip link add name br1 type bridge >> ../$thisFile
    read -a allInterfaces <<< $(cat $eachSwitch | awk '{print $3}' | awk '{a=$0;printf "%s ",a,$0}' | tac)
    sizeArray=${#allInterfaces[@]}
    for (( i=0; i < 2; i=i+1 )); do
      echo sudo ip link add name br$i type bridge >> ../$thisFile  #cria uma bridge para linkar interfaces
      echo sudo ip link set dev br$i up >> ../$thisFile            #levanta as interfaces bridges
      echo sudo ip link add link ${allInterfaces[0]} name ${allInterfaces[0]}.$((i+1))00 type vlan id $((i+1))00 >> ../$thisFile #cria nova interface ethy.x como vlan
      echo sudo ip link set dev ${allInterfaces[0]}.$((i+1))00 up >> ../$thisFile      #levanta a nova interface
      echo sudo ip link set dev ${allInterfaces[0]}.$((i+1))00 master br$i >> ../$thisFile  #linka interface com a bridge. Essa é a interface entre roteadores 50/49
    done
    interfaces=`cat $eachSwitch | awk '{print $3}' | tac` #pega a lista de todas as interfaces ao contrario, para a interface entre switchs ficar na frente
    interfaces=`echo $interfaces  | cut -d " " -f2- `     #retira a interface 50/49
    read -a allInterfaces <<< $(echo $interfaces)         #transforma em array
    sizeArray=${#allInterfaces[@]}
    index=0     #numero da interface a ser atribuida
    for (( i=0; i < $((sizeArray/2)); i=i+1 )); do
      echo sudo ip link set dev ${allInterfaces[$index]} master br0 >> ../$thisFile  #coloca metade das maquinas na bridge br0
      index=$((index+1))
    done
    for (( i=0; i < $((sizeArray/2)); i=i+1 )); do
      echo sudo ip link set dev ${allInterfaces[$index]} master br1 >> ../$thisFile  #e a outra metade na bridge br1
      index=$((index+1))
    done
  done
}

main() {
  config_vlan
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
