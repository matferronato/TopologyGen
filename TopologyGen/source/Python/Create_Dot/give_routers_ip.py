# -*- coding: utf-8 -*-

def giveRoutersIp(allIpNetworks, machineNumbers, parent, tree):
	children = tree.children(parent)   #retorna todos os filhos de um determinado pai
	thisNode = tree.get_node(parent)   #retorna o nodo pai
	childrenIpList = []                                             #como existem varias redes conectadas a um roteador, aqui eh criado uma lista vazia para agregar esses valores
	for eachChild in children:
		if("switch" in eachChild.tag or "server" in eachChild.tag): #se os filhos sao switchs ou servidores, isso equivale a chegar nas folhas da arvore
			thisEthIp = eachChild.data                              #o ip de uma determinada eth para um nodo acima das folhas sera o ip contido na folha, criando a conexao entre folha e roteador
			bufferIp = thisEthIp.split('.')[:3]
			thisEthIp = str(bufferIp[0]) + '.' + str(bufferIp[0]) + '.' + str(bufferIp[0]) + ".1/24" #quebra o valor x.x.x.y/24 em um valor x.x.x.1 reservado ao roteador, valendo para as N redes conectadas no router/oob em layers abaixo
			childrenIpList.append(thisEthIp)
		else:
			giveRoutersIp(allIpNetworks, machineNumbers, eachChild.tag, tree) #no caso do nodo nao ter folhas logo abaixo(tratando folhas como server OU switchs), realizamos a recursao, o que significa que existe oob e router nessa topologia
			thisEthIp = eachChild.data
			bufferIp = thisEthIp[-1]
			bufferIp = bufferIp.split('.')[:3]
			thisEthIp = str(bufferIp[0]) + '.' + str(bufferIp[0]) + '.' + str(bufferIp[0]) + ".1/24" #cria a ligacao entre o nodo pai .1 com o nodo filho .2
			childrenIpList.append(thisEthIp)
	if (thisNode.tag != "root"):                                        #aqui eh atribuido o ip entre o nodo filho .2 e o nodo pai .1, mas esse nao existe para o nodo fantasia raiz
		if (tree.parent(thisNode.tag).tag != "root"):	                #linha de cima testa o novo raiz enquanto esta linha testa se o nodo pai eh a raiz
			thisIp = allIpNetworks.pop(0) if len(allIpNetworks) != 0 else allIpNetworks   #testa se valor eh lista o string, para retirar o dominio final de ip
			bufferIp = thisIp.split('.')[:3]
			thisEthIp = str(bufferIp[0]) + '.' + str(bufferIp[0]) + '.' + str(bufferIp[0]) + ".2/24"
			childrenIpList.append(thisEthIp)
		thisNode.data = childrenIpList       #por fim, apos a recursao, adiciona os ips criados ao campo de payload do nodo
