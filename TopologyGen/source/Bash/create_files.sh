create_files(){

  echo -e ${YELLOW}Creating machine config files${NC}
for eachMachine in $allMachines; do
  cat ../../Host_Scripts/ipList.txt | grep $eachMachine >> $eachMachine.txt
done
}

main() {
  create_files
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
