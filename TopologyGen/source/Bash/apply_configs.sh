apply_configs(){
  cd ../../Automate/Guest_Scripts
  chmod 777 *
  cd ..
  allMachines=`ls ./Guest_Scripts/Interface_Information/ | grep -v hostname | sed 's/.interface//'`
  for eachMachine in $allMachines; do
    thisfile=${eachMachine}.cnfg
    echo -e ${YELLOW}"Running machine ${BLUE}${eachMachine} ${YELLOW}configs"${NC}
    vagrant ssh $eachMachine -c "sudo chmod 777 /vagrant/Guest_Scripts/*; sudo /vagrant/Guest_Scripts/$thisfile" 
  done
  wait
  cd ../source/Bash/
}

main() {
  apply_configs
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
