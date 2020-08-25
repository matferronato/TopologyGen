add_server_default_gateway(){
  echo -e ${YELLOW}"Creating ${BLUE} default gateway ${YELLOW}for routers"${NC}
  allServers=`ls | grep server`
  for eachServer in $allServers; do
      thisFile=${eachServer}_setup.txt
      echo sudo ip route del 0/0 >> ../$thisFile
      ip=`cat $eachServer | awk '{print $2}'`
      currentNetwork=`echo $ip | sed -r 's/(([0-9]{1,3}\.){3})[0-9]{1,3}/\11/g' | sed 's/\/24//g'`
      echo sudo ip route add 0/0 via $currentNetwork dev eth50 >> ../$thisFile
  done
}

main() {
  add_server_default_gateway
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
