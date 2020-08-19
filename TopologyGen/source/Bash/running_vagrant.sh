running_vagrant(){
  echo -e ${YELLOW}"Creating machines"${NC}
  cd ./Vagrant
  vagrant up
  sleep 20
}

main() {
  running_vagrant
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
