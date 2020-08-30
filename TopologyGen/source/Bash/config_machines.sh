config_machines(){

echo -e ${YELLOW}Running Configuration Script${NC}
#current directory is /source/Bash/
cd ../Python/Machine_Configuration
python3 configure_machines.py
cd ../../Bash
}

main() {
  create_files
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
