create_files(){

echo -e ${YELLOW}Creating machine config files${NC}
#current directory is /source/Bash/

rm -rf ../../Automate/Guest_Scripts/*.cnfg
rm -rf ../../Automate/Guest_Scripts/BGP_Information/*
rm -rf ../../Automate/Guest_Scripts/Color_Information/*
rm -rf ../../Automate/Guest_Scripts/Interface_Information/*.interface
rm -rf ../../Automate/User_Scripts/tmp/*
> ../../Automate/Guest_Scripts/Color_Information/Blue.cnfg && > ../../Automate/Guest_Scripts/Color_Information/Red.cnfg
> ../../Automate/Guest_Scripts/Color_Information/Yellow.cnfg && > ../../Automate/Guest_Scripts/Color_Information/Purple.cnfg
ls ../../source/Python/Machine_Configuration/ | grep -v configure_machines.py | grep -v __pycache__ | xargs printf -- '../../source/Python/Machine_Configuration/%s\n' | xargs rm
cp ../../Automate/User_Scripts/Python_Services/* ../../source/Python/Machine_Configuration/
allMachines=`cat ../../Automate/Host_Scripts/all_machines.txt`

for eachMachine in $allMachines; do
  cat ../../Automate/Host_Scripts/ip_info.txt | grep ${eachMachine}'\s'  >> ../../Automate/Guest_Scripts/Interface_Information/$eachMachine.interface
  touch ../../Automate/Guest_Scripts/$eachMachine.cnfg
done
}

main() {
  create_files
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
