# -*- coding: utf-8 -*-

def giveLeavesIp(allIpNetworks, machineNumbers, tree):
	i = 0;
	childList = tree.leaves("root") #salva todos os filhos da raiz
	if (machineNumbers[0] == 0 and machineNumbers[1] == 0): #no caso de nao existirem roteadores, entao todas as maquinas pertencem a mesma rede
		currentIp = allIpNetworks.pop(0)                    #retira o unico dominio criado de ip
		for eachChild in childList:                         #para cada servidor cria um ip novo. Ips nao podem terminar em 0 e nao devem terminar em 1 (reservado ao roteador)
			thisIp = currentIp.replace("0/",str(i+2)+"/");
			i = i +1
			eachChild.data = thisIp
	elif (machineNumbers[2] != 0):                          #caso existam roteadores no meio
		index = 0
		while(machineNumbers[index] == 0): index = index + 1 #procura saber quem eh o nivel acima do switch (router ou oob)
		leavesNetworks =  machineNumbers[0] * machineNumbers[1] * machineNumbers[2] if (machineNumbers[0] != 0 and machineNumbers[1] != 0) else machineNumbers[index] * machineNumbers[2]   #minha quantidade de redes a serem retiradas serao o numero de roteadores pelo numero de switchs
		i = 0
		for j in range(0, leavesNetworks):
			currentIp = allIpNetworks.pop(0)                 #pega o ip a ser utilizado na rede do filho
			currentParent = tree.parent(childList[i].tag)    #guarda o ip no switch, para o pai roteador pegar no futuro
			currentParent.data = currentIp
			for k in range(machineNumbers[3]):               #este laco itera por maquina no dominio de ip, enquanto a maquina e selecionada pela variavel i criada antes dos lacos
				thisIp = currentIp.replace("0/",str(k+2)+"/")
				childList[i].data = thisIp
				i = i + 1
	else:
		routerNumber = machineNumbers[1] if machineNumbers[1] != 0 else 1  #em ambiente com roteadores, descobre a quantidade de routers e oobs
		oobNumber = machineNumbers[0] if machineNumbers[0] != 0 else 1
		for i in range(0, oobNumber * routerNumber * machineNumbers[3]):  #se nao existe switchs, a quantidade de redes sera o numero de servidores vezes numero de roteadores
			currentIp = allIpNetworks.pop(0)
			thisIp = currentIp.replace("0/",str(2)+"/")
			childList[i].data = thisIp
