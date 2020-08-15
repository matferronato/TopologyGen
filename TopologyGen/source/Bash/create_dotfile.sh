create_dotfile(){
  echo -e "${YELLOW}Running ${BLUE}converter.py, ${YELLOW}creating network topology"${NC}
  python ../Python/converter.py --l0 $server --l1 $switch --l2 $router --l3 $oob
}

main() {
  create_dotfile
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
