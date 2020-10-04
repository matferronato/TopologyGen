interface_up(){
  echo -e "${YELLOW}Setting interfaces"${NC}
  cd ../../Automate/Guest_Scripts/Interface_Information/
  listOfFiles=`ls`
  for eachFile in $listOfFiles; do
  while read line; do
    thisFile=`echo $eachFile | sed 's/.interface/.cnfg/g'`
    ip=`echo $line | awk '{print $2}'`
    interface=`echo $line | awk '{print $3}'`
    echo sudo ip addr add $ip dev $interface >> ../$thisFile
    echo sudo ip link set $interface up >> ../$thisFile
    echo sudo ifconfig $interface $ip up >> ../$thisFile
  done<$eachFile
  done
  cd ../../../source/Bash
}

main() {
  interface_up
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
