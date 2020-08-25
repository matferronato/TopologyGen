apply_configs(){
  allMachines=`ls ./Automate/Guest_Scripts/Interface_Information/ | grep -v hostname | sed 's/.txt//'`
  for eachMachine in $allMachines; do
    thisfile=${eachMachine}.txt_setup.txt
    echo -e ${YELLOW}"Running machine ${BLUE}${eachMachine} ${YELLOW}configs"${NC}
    vagrant ssh $eachMachine -c "sudo /vagrant/Automate/Guest_Scripts/$thisfile"
  done
  cd ..
}

main() {
  apply_configs
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
