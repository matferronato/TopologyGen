create_vagrantfile(){
  echo "Creating Vagrantfile"
  #currently at /source/Bash/
  cd ../../ && chmod -R 777 ./
  cd ./source/Python/Dot_to_Vagrantfile
  python3 reader.py ../../../TopologyInfo/topology.dot -p libvirt
  rm ../../../Automate/Vagrantfile
  mv Vagrantfile ../../../Automate
  cd ../../../source/Bash
}

main() {
  create_vagrantfile
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
