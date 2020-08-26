# -*- coding: utf-8 -*-
connections = "\"!\":\"@\" -- \"$\":\"%\"\n"

def writeConnections(file, parent, tree):
	i = 1;
	topLevelText = connections.replace("!",str(tree.get_node(parent).tag).replace("\"",""))  #texto padrao colocando o pai como uma das conexoes
	for eachParent in tree.children(parent):                                                 #para cada filho do nodo pai
		topLevelConnection = topLevelText.replace("$",str(eachParent.tag).replace("\"",""))  #cria a string desejada agora alterando o filho
		topLevelConnection = topLevelConnection.replace("@","eth"+str(i))
		topLevelConnection = topLevelConnection.replace("%","eth"+str(50))
		i = i +1
		if(not("root" in topLevelConnection)):                                               #descobre se é uma conexao com o root, a qual eh uma conexao fantasia
			file.write(topLevelConnection)                                                   #adiciona a conexao valida entre pai e filho
		childLevelText = connections.replace("!",str(eachParent.tag).replace("\"",""))       #altera o texto para colocar o filho como pai, criando uma conexao com os netos
		j = 1
		for eachChildren in tree.children(eachParent.tag):
			thisConnection = childLevelText.replace("$",str(eachChildren.tag).replace("\"","")) #cria um novo texto, isso eh necessario pois assim pode-se calcular qual o numero correto da eth
			thisConnection = thisConnection.replace("@","eth"+str(j))
			thisConnection = thisConnection.replace("%","eth"+str(50))
			j=j+1
			file.write(thisConnection)
			writeConnections(file, eachChildren.tag, tree)                                            #se tem filhos, entao faz recursao
	file.write("\n")
