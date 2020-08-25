unlock_files(){

echo -e ${YELLOW}Unlocking files${NC}
echo open > ./Automate/Host_Scripts/locker.txt
}

main() {
  create_files
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
