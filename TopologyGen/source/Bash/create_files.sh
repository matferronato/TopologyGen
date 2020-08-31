create_files(){

echo -e ${YELLOW}Creating machine config files${NC}
#current directory is /source/Bash/
rm -rf ../../Automate/Guest_Scripts/*.cnfg
rm -rf ../../Automate/Guest_Scripts/Interface_Information/*.interface
allMachines=`cat ../../Automate/Host_Scripts/all_machines.txt`

for eachMachine in $allMachines; do
  cat ../../Automate/Host_Scripts/ip_info.txt | grep $eachMachine >> ../../Automate/Guest_Scripts/Interface_Information/$eachMachine.interface
  touch ../../Automate/Guest_Scripts/$eachMachine.cnfg
done
}

main() {
  create_files
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
