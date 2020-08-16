create_dotfile(){
  echo -e "${YELLOW}Running ${BLUE}converter.py, ${YELLOW}creating network topology"${NC}
  cd ../Python/Create_Dot/
  python converter.py --l0 $server --l1 $switch --l2 $router --l3 $oob
  cd ../../Bash
}

main() {
  create_dotfile
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
