# -*- coding: utf-8 -*-
import os

from get_number_of_networks import getNumberOfNetwork
from generate_ip_networks import generateIpNetworks
from provide_ip import provideIp
from match_ip_and_eth import matchIpAndEth

def createIps(machineNumbers, tree):
	if os.path.exists("../../../Automate/Host_Scripts/ipList.txt"): os.remove("../../../Automate/Host_Scripts/ipList.txt")
	file = open("../../../Automate/Host_Scripts/ipList.txt","w+")

	numberOfNetworks = getNumberOfNetwork(machineNumbers)
	allIpNetworks = generateIpNetworks(numberOfNetworks)
	provideIp(allIpNetworks, machineNumbers, "root", tree)
	matchIpAndEth("root",file, tree)

	file.close()
