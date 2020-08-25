create_vagrantfile(){
  echo "Creating Vagrantfile"
  cd ../../../ && chmod -R 777 ./
  cd ./source/Python/Dot_to_Vagrantfile
  python reader.py ../../../TopologyInfo/topology.dot -p libvirt
  mv Vagrantfile ../../../Vagrant
  cd ../../../
}

main() {
  create_vagrantfile
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
