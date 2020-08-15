parse_args(){
  echo -e "${YELLOW}Parsing arguments"${NC}
  server="";switch="";router="";oob=""; protocols=""; machineType="";
  createSwitchs=0
  createRouters=0
  vxlan=0
  vlan=0
  bgp=0

  everyCommand=$*
  read -a commandArray <<< $everyCommand
  sizeArray=${#commandArray[@]}

  for (( everyCommand=0; everyCommand < $sizeArray; everyCommand=everyCommand+1)); do
    if [[ ${commandArray[$everyCommand]} == --l0 ]];       then server=${commandArray[$everyCommand+1]};      fi
    if [[ ${commandArray[$everyCommand]} == --l1 ]];       then switch=${commandArray[$everyCommand+1]};      fi
    if [[ ${commandArray[$everyCommand]} == --l2 ]];       then router=${commandArray[$everyCommand+1]};      fi
    if [[ ${commandArray[$everyCommand]} == --l3 ]];       then oob=${commandArray[$everyCommand+1]};         fi
    if [[ ${commandArray[$everyCommand]} == -m ]]; then
      if [[ ${commandArray[$everyCommand+1]} == *r* ]]; then
        createRouters=1
      fi
      if [[ ${commandArray[$everyCommand+1]} == *s* ]]; then
        createSwitchs=1
      fi
    fi

    if [[ ${commandArray[$everyCommand]} == -p ]]; then
      if [[ ${commandArray[$everyCommand+1]} == *x* ]]; then
        vxlan=1
      fi
      if [[ ${commandArray[$everyCommand+1]} == *l* ]]; then
        vlan=1
      fi
      if [[ ${commandArray[$everyCommand+1]} == *b* ]]; then
        bgp=1
      fi
    fi
  done


  if [[ $server == "" ]];           then server=1;  echo no server definied, using server equal to 1;           fi
  if [[ $switch == "" ]];           then switch=0;  echo no switch definied, using switch equal to 0;           fi
  if [[ $router == "" ]];           then router=0;  echo no router definied, using router equal to 0;           fi
  if [[ $oob == "" ]];              then oob=0;     echo no out of bound switch definied, using oob equal to 0; fi
  if [[ $vxlan == 1 ]];             then            echo vxlan will be configured ;                             fi
  if [[ $vlan == 1 ]];              then            echo vlan will be configured ;                              fi
  if [[ $bgp == 1 ]];               then            echo bgp will be configured ;                               fi
  if [[ $createRouters == 1 ]];     then            echo routers machines will be configured as routers;  fi
  if [[ $createSwitchs == 1 ]];     then            echo switch machines will be configured as switchs;         fi

}

main() {
  parse_args
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
