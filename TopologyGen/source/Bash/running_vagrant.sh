running_vagrant(){
  echo -e ${YELLOW}"Creating machines"${NC}
  cd ../../Automate
  vagrant up
  echo -e ${YELLOW}"Vagrant is UP, waiting 10 seconds to apply machine configs"${NC}
  sleep 10
  cd ../source/Bash/

}

main() {
  running_vagrant
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
