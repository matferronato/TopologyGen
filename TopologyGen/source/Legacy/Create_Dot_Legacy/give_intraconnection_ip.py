# -*- coding: utf-8 -*-


def giveIntraConnectionIp(allIpNetworks, machineNumbers, parent, tree):
	child = tree.children(parent)
	for i in range(0, len(child)):  #este laco roda por todos os filhos da raiz indexando por I para poder retirar nodos vizinhos como em um array
		if(i != 0):                 #testa se este eh o primeiro nodo, sem vizinhos a sua esquerda
			bufferIPList = child[i].data     #caso nao seja o primeiro nodo, a ideia eh retirar seu payload e o payload do vizinho
			siblingIPList = child[i-1].data
			bufferIp = siblingIPList[-1]     #retirar o ultimos valor adicionado a este (que sempre sera o ip atribuido a conexao entre os mesmos niveis)
			bufferIp = bufferIp.split('.')[:3]
			thisEthIp = str(bufferIp[0]) + '.' + str(bufferIp[0]) + '.' + str(bufferIp[0]) + ".2/24"
			bufferIPList.append(thisEthIp)  #adiciona o valor criado a lista de payload atual
			child[i].data = bufferIPList
		if (i != len(child)-1):		#testa se este eh o ultimo nodo, sem vizinhos a direita
			bufferIPList = child[i].data #caso nao seja o ultimo, a ideia eh retirar seu payload para adicionar novo valor a este
			thisIp = allIpNetworks.pop(0) if len(allIpNetworks) != 0 else allIpNetworks
			bufferIp = thisIp.split('.')[:3]
			thisEthIp = str(bufferIp[0]) + '.' + str(bufferIp[0]) + '.' + str(bufferIp[0]) + ".1/24"
			bufferIPList.append(thisEthIp)
			child[i].data = bufferIPList
#aqui eh importante notar que um nodo intermediario ira cair nos dois ifs
