#!/bin/bash
declare -A all_machines
declare -A all_functionalities
source ./source_all.sh
lock_files
#create_vagrantfile
create_files
interface_up
config_machines

####GERA LISTA DE MAQUINAS
#allMachines=`cat ../../Automate/Host_Scripts/ipList.txt | awk '{print $1}' | sort | uniq`
#cd ../../Automate/Guest_Scripts/ && rm -rf *.txt && cd ./Interface_Information/ && rm -rf *

#add_server_default_gateway
#if [[ $vlan == 1 ]]; then config_vlan
#else
#  if [[ $createSwitchs == 1 ]]; then config_switchs; fi
#fi
#if [[ $createRouters == 1 ]]; then config_routers; fi
#if [[ $bgp == 1 ]]; then config_bgp; fi
#if [[ $vxlan == 1 ]]; then config_vxlan; fi
#add_hostnames

#running_vagrant
#apply_configs
#if [[ $macro == 1 ]]; then run_macro $server $switch $router $oob; fi
unlock_files
