# -*- coding: utf-8 -*-
connections = "\"!\":\"@\" -- \"$\":\"%\"\n"

def writeTopLevelConnection (file, parent, tree):
		allChildren = tree.children(parent)
		if(len(allChildren)-1 == 0): return          #se eh um pai "solteiro" retorna
		for i in range(0, len(allChildren)-1):       #no range de todos os filhos do root, cria ligacao entre irmaos.
			firstConnection = connections.replace("!",str(allChildren[i].tag).replace("\"",""))
			firstConnection = firstConnection.replace("$",str(allChildren[i+1].tag).replace("\"",""))
			firstConnection = firstConnection.replace("@","eth"+str(50))
			firstConnection = firstConnection.replace("%","eth"+str(49))
			file.write(firstConnection)
