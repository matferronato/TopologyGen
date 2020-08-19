create_files(){

echo -e ${YELLOW}Creating machine config files${NC}
> ../../Host_Scripts/allMachinesNames.txt
for eachMachine in $allMachines; do
  cat ../../Host_Scripts/ipList.txt | grep $eachMachine >> $eachMachine.txt
  echo $eachMachine >> ../../Host_Scripts/allMachinesNames.txt
done
}

main() {
  create_files
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
