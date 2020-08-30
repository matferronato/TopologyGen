# -*- coding: utf-8 -*-

def getNumberOfNetwork(machineNumbers):
	if(machineNumbers[0] == 0 and machineNumbers[1] == 0): #se nao existem roteadores, entao so existe um dominio de ip
		return 1
	else:
		bottomNetworks = machineNumbers[2] if machineNumbers[2] != 0 else machineNumbers[3] #as redes de baixo sao os servidores ou switchs (switchs nao tem ip, mas podem definir "barreiras" entre redes).
		middleNetworks = machineNumbers[1] if machineNumbers[1] != 0 else machineNumbers[0] #as redes do meio sao os roteadores, neste caso out of bounds toma lugar dos roteadores caso eles n existam
		topNetworks = 1 if machineNumbers[0] != 0 else 0                                    #redes que tenho entre roteadores e oob
		oobNetworks = machineNumbers[0] if machineNumbers[0] != 0 else 1                    #quantidade de oob que possuo

		index = 0 if machineNumbers[0] != 0 else 1
		intraNetworks = machineNumbers[index]-1
		allNetworks = (((bottomNetworks * middleNetworks) + (middleNetworks* topNetworks)) * oobNetworks) + intraNetworks #o meu numero de redes, sao as redes inferiores dos roteadores + as redes superiores vezes o numero de redes superiores + a conexao entre essas redes
		return allNetworks
