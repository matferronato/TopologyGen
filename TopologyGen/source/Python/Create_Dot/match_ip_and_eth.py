# -*- coding: utf-8 -*-
import types

def matchIpAndEth(parent, file, tree):                    #OK..... esse precisa ser otimizado no futuro....
	children = tree.children(parent)                #recupera todos os nodos filhos de um determinado pai
	thisNode = tree.get_node(parent)                #recupera o nodo deste pai
	l = 0                                           #lenght lateral da arvore no nivel referente as conexoes intra level
	if(len(children) != 0):                         #se ele nao possui nenhum filho, entao o nodo eh uma folha, encerrando a recursao
		k = 0                                       #variavel que calcula qual das maquinas switchs esta se conectando com qual
		for eachChild in children:
			if (isinstance(eachChild.data, types.StringTypes)) :  #procura se o nodo tem somente um filho. isso so ira ocorrer para folhas, e no caso de haver somente um roteador/switch como ramo
				if not ("switch" in eachChild.tag):               # se ele for um switch, entao nao tem ip associado
					currentEth = "eth50" if ("server" in eachChild.tag) else "eth1"	 #se ele for servidor entao a ligacao esta acima, se for outro, a ligacao esta abaixo
					file.write(eachChild.tag.replace("\"","") + " " +  eachChild.data + " " + currentEth + "\n")
				else:                                             #caso a maquina seja um switch, eh necessario criar as N +1 interfaces sem ips
					i = 1
					for eachGrandChild in tree.children(eachChild.tag): #uma interface para cada servidor
						file.write(eachChild.tag.replace("\"","") + " " +  "0.0.0.0/24" + " eth"+ str(i) +"\n")
						i = i+1
					file.write(eachChild.tag.replace("\"","") + " " +  "0.0.0.0/24" + " eth"+ str(50) +"\n") #uma interface extra para a conexao com router
			elif (eachChild.data != None):             #agora, caso a maquina seja um roteador
				i = 1                                  #esta i ira concatenar com o nome eth para gerar as interfaces dos filhos eth1 eth2 ethN
				currentEth = ""
				flagLeftNeigh = True                   #para uma maquina com conexoes entre as layers, havera 3 casos, esta flag controla o caso 3, onde existem maquinas no meio das outras
				for eachData in eachChild.data:  #Each ETH as a network device
					if(i != len(tree.children(eachChild.tag))+1):  #enquanto estiver no dominio dos filhos, a maquina ira criar uma eth menor que 50
						currentEth = "eth"+str(i)
						i = i+1
						file.write(eachChild.tag.replace("\"","") + " " +  eachData + " " + currentEth + "\n")
					else:
						if(tree.parent(eachChild.tag).tag == "root"):  #estando fora no dominio dos filhos, eh testado se a proxima conexao eh entre os levels ou para roteadores acima
							if(l == 0):                                #no primeiro caso, a maquina eh a primeira, entao so tem conexao para direita
								each_eth = "eth50"
							elif(l == len(tree.siblings(eachChild.tag))+1): #no segundo caso, a maquina eh a ultima, entao so tem conexao para esquerda
								each_eth = "eth49"
							elif (flagLeftNeigh == True): #no caso três, existem duas conexoes por maquina, a 49 a esquerda e a 50 a direita
								each_eth = "eth49"
								flagLeftNeigh = False
							else:
								each_eth = "eth50"
								flagLeftNeigh = True
							file.write(eachChild.tag.replace("\"","") + " " +  eachData + " " + each_eth +"\n")
						else:                             #caso a conexao nao seja nenhuma das anteriores, entao eh uma conexao para um roteador num level acima
							currentEth = "eth"+str(50)
							file.write(eachChild.tag.replace("\"","") + " " +  eachData + " " + currentEth + "\n")

			else:                                          #para topologias somente com switchs e servidores
				i = 1
				for eachGrandChild in tree.children(eachChild.tag): #para cada filho do switch, eh criado uma nova interface eth
					file.write(eachChild.tag.replace("\"","") + " " +  "0.0.0.0/24" + " eth"+ str(i) +"\n")
					i = i+1
				if(k != 0):                                          #no caso de so existirem switchs, eh criada conexoes entre switchs
					file.write(eachChild.tag.replace("\"","") + " " +  "0.0.0.0/24" + " eth"+ str(49) +"\n")
				if(k != len(tree.siblings(eachChild.tag))):
					file.write(eachChild.tag.replace("\"","") + " " +  "0.0.0.0/24" + " eth"+ str(50) +"\n")
				k = k+1                                             #busca proxima maquina interconectada com switchs
			matchIpAndEth(eachChild.tag, file, tree)                      #faz o processo de recursao
			l = l +1                                                #busca proximo vizinho no caso de roteadores
