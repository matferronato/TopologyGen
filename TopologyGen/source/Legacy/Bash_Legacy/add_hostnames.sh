add_hostnames(){
  echo -e ${YELLOW}"Setting ${BLUE}hostnames"${NC}
  while read line; do
    ip=`echo $line | awk '{print $2}' | sed 's/\/24//'`
    hostname=`echo $line | awk '{print $1}'`
    echo $ip $hostname >> ./hostnames.txt
  done<../../Host_Scripts/ipList.txt
  for eachFile in $listOfFiles; do
    thisFile=${eachFile}_setup.txt
    echo "cat /vagrant/Automate/Guest_Scripts/Interface_Information/hostnames.txt > /etc/hosts" >> ../$thisFile
  done
}

main() {
  add_hostnames
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
