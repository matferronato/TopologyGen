#!/bin/bash

run_macro(){
######################################################
#      ADD YOUR COMMANDS INSIDE THIS FUNCTIO         #
######################################################
echo banana
}

main() {
  run_macro
}


if [ "${1}" != "--source-only" ]; then
    main "${@}"
fi
