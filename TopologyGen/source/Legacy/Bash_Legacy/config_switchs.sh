config_switchs(){
    echo -e ${YELLOW}"Setting switch machines as ${BLUE}switchs"${NC}
    allSwitchs=`ls | grep switch`
    for eachSwitch in $allSwitchs; do
      thisFile=${eachSwitch}_setup.txt
      echo sudo ip link add name br0 type bridge >> ../$thisFile
      echo sudo ip link set dev br0 up  >> ../$thisFile
      allInterfaces=`cat $eachSwitch | awk '{print $3}'`
      for eachInterface in $allInterfaces; do
        echo sudo ip link set dev $eachInterface master br0 >> ../$thisFile
      done
    done
}

main() {
  config_switchs
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
