config_vxlan(){
  echo -e ${YELLOW}"Setting ${BLUE}vxlan ${YELLOW}protocol"${NC}
  allRouters=`ls | grep router` #recupera lista de todas as maquinas roteadores (por definição, maquinas oob não devem ter nenhuma configuração)
  i=1
  for eachRouter in $allRouters; do
    thisFile=${eachRouter}_setup.txt
    echo sudo ip link add br0 type bridge >> ../$thisFile  #cria a bridge na qual vai ser ligada a interface vxlan de saída com a interface local de entrada
    echo sudo ip link set br0 up  >> ../$thisFile          #levanta a interface
    read -a allInterfaces <<< $(cat $eachRouter | awk '{print $3}' | awk '{a=$0;printf "%s ",a,$0}' | tac)
    echo sudo ip link add name vxlan10 type vxlan id 10 dev ${allInterfaces[0]} group 239.1.1.1 dstport 4789  >> ../$thisFile #cria uma interface vxlan ligada a eth50
    #necessario criar ip?
    echo sudo ip link set dev eth1 master br0 >> ../$thisFile   #linka via bridge a eth de saida com a eth de entrada
    echo sudo ip link set dev vxlan10 master br0 >> ../$thisFile
    echo sudo ip link set vxlan10 up  >> ../$thisFile     #levanta interface de saída
    myId=`echo $eachRouter | sed 's/router//' | sed 's/txt//'` #depois de ter configurado vxlan nos roteadores, é necessario configurar novo ip para servidores
    if [ -e ./server${myId}1.1.txt ]; then                     #existem duas possibilidades, ou abaixo do router tem switch ou não. Testamos isso pelo nome do servidor
      for (( index = 1; index < $((server+1)) ; index=index+1 )); do #caso exista roteadores, é preciso mudar todos os membros abaixo do switch.x.1.1
        serverId=server${myId}1.$index                               #é descoberto o nome das maquinas contidas abaixo da eth1 do router
        thisFile=${serverId}.txt_setup.txt
        echo sudo ip addr add 100.100.100.$i/24 dev eth50 >> ../$thisFile #criado novo range de ip que servira para vms abaixo do switch1 dos routers
        i=$((i+1))
      done
    else       #no caso de existir somente um servidor abaixo da eth1
      serverId=server${myId}1  #é descoberto o nome da maquina
      thisFile=${serverId}.txt_setup.txt
      echo sudo ip addr add 100.100.100.$i/24 dev eth50 >> ../$thisFile #é criado um novo range de ip
      i=$((i+1))
    fi
  done
}

main() {
  config_vxlan
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
