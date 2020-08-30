running_vagrant(){
  echo -e ${YELLOW}"Creating machines"${NC}
  cd ../../Automate
  vagrant up
  sleep 3
  cd ../source/Bash/

}

main() {
  running_vagrant
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
