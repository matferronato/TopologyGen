running_vagrant(){
  echo -e ${YELLOW}"Creating machines"${NC}
  cd ../../Automate
  startVagrant=`date +%s`
  vagrant up
  echo -e ${YELLOW}"Vagrant is UP, waiting 20 seconds to apply machine configs"${NC}
  sleep 20
  cd ../source/Bash/

}

main() {
  running_vagrant
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
