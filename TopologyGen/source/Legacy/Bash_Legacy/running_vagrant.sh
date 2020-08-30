running_vagrant(){
  echo -e ${YELLOW}"Creating machines"${NC}
  cd ./Vagrant
  sudo vagrant up --provider=libvirt
  sleep 2
}

main() {
  running_vagrant
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
