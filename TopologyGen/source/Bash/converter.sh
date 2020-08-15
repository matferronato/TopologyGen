#!/bin/bash
source ./source_all.sh
parse_args $*
create_dotfile

####GERA LISTA DE MAQUINAS
allMachines=`cat ../../Automate/Host_Scripts/ipList.txt | awk '{print $1}' | sort | uniq`
cd ../../Automate/Guest_Scripts/ && rm -rf *.txt && cd ./Interface_Information/ && rm -rf *

create_files
interface_up
add_server_default_gateway

if [[ $vlan == 1 ]]; then config_vlan
else
  if [[ $createSwitchs == 1 ]]; then config_switchs; fi
fi
if [[ $createRouters == 1 ]]; then config_routers; fi
if [[ $bgp == 1 ]]; then config_bgp; fi
if [[ $vxlan == 1 ]]; then config_vxlan; fi
add_hostnames

#CRIA ARQUIVO VAGRANT FILE
cd ../../../ && chmod -R 777 ./
echo "Creating Vagrantfile"
cd ./source/Python/ && python reader.py ../../TopologyInfo/topology.dot -p libvirt && mv Vagrantfile ../../Vagrant && cd ../../

#LEVANTA AS MAQUINAS
echo -e ${YELLOW}"Creating machines"${NC}
#vagrant up
#sleep 20

#CONFIGURA AS MAQUINAS
#allMachines=`ls ./Automate/Guest_Scripts/Interface_Information/ | grep -v hostname | sed 's/.txt//'`
#for eachMachine in $allMachines; do
#  thisfile=${eachMachine}.txt_setup.txt
#  echo -e ${YELLOW}"Running machine ${BLUE}${eachMachine} ${YELLOW}configs"${NC}
#  vagrant ssh $eachMachine -c "sudo /vagrant/Automate/Guest_Scripts/$thisfile"
#done
