lock_files(){

echo -e ${YELLOW}Locking files${NC}
echo closed > ../../Automate/Host_Scripts/locker.txt
}

main() {
  create_files
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
