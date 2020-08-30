interface_up(){
  echo -e "${YELLOW}Setting interfaces"${NC}
  listOfFiles=`ls`
  for eachFile in $listOfFiles; do
  while read line; do
    thisFile=${eachFile}_setup.txt
    ip=`echo $line | awk '{print $2}'`
    interface=`echo $line | awk '{print $3}'`
    echo sudo ifconfig $interface $ip up >> ../$thisFile
  done<$eachFile
  done
}

main() {
  interface_up
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
