#!/bin/bash

echo "#################################"
echo "  Running Switch Post Config (config_vagrant_switch.sh)"
echo "#################################"
sudo su


# Config for Vagrant Interface
cat <<EOT > /etc/network/interfaces
auto lo
iface lo inet loopback

auto eth0
iface eth0 inet dhcp

auto vagrant
iface vagrant
    vrf-table auto

auto eth1
iface eth1 inet dhcp
    alias Interface used by Vagrant
    vrf vagrant

source /etc/network/interfaces.d/*
EOT

echo "#################################"
echo "   Finished"
echo "#################################"
