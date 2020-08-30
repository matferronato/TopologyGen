#!/bin/bash
declare -A all_machines
declare -A all_functionalities
source ./source_all.sh
lock_files
create_vagrantfile
create_files
interface_up
config_machines
running_vagrant
apply_configs
unlock_files
