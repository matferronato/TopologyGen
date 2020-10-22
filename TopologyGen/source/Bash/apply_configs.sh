apply_configs(){
  cd ../../Automate/Guest_Scripts
  chmod 777 *
  cd ..
  allMachines=`ls ./Guest_Scripts/Interface_Information/ | grep -v hostname | sed 's/.interface//'`
  startConfig=`date +%s`
  for eachMachine in $allMachines; do
    thisfile=${eachMachine}.cnfg
    echo -e ${YELLOW}"Running machine ${BLUE}${eachMachine} ${YELLOW}configs"${NC}
    vagrant ssh $eachMachine -c "sudo chmod 777 /vagrant/Guest_Scripts/*; sudo /vagrant/Guest_Scripts/$thisfile" &
  done
  wait
  cd ../source/Bash/
  end=`date +%s`
  machineNumber=`wc -l ../../Automate/Host_Scripts/all_machines.txt | awk '{print $1}'`
  vagrantRunTime=$((end-startVagrant))
  configRunTIme=$((end-startConfig))
  cpuUsage=`mpstat | grep -A 5 "%idle" | tail -n 1 | awk -F " " '{print 100 -  $ 12}'a`
  memoryUsage=`free -m | awk '{print $3}' | xargs | awk '{print $2}'`
  echo $machineNumber >> ~/statistic.txt
  echo Total Time = $vagrantRunTime s >> ~/statistic.txt
  echo Config Time = $configRunTIme s >> ~/statistic.txt
  echo $cpuUsage >> ~/statistic.txt
  echo $memoryUsage >> ~/statistic.txt
  echo "" >> ~/statistic.txt

}

main() {
  apply_configs
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
