# -*- coding: utf-8 -*-

def generateIpNetworks(numberOfNetworks):
	standartIp = "!.!.!.%/24" #ip padrao a ser alterado
	allIPNetworks = []
	for i in range(1, numberOfNetworks+1):
		thisIp = standartIp.replace("!",str(i))
		thisIp = thisIp.replace("%",str(0))
		allIPNetworks.append(thisIp)
	return allIPNetworks
