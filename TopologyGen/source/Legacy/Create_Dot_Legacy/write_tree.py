# -*- coding: utf-8 -*-
from treelib import Tree, Node
possibleNames = ["\"oob$\"", "\"router$\"", "\"switch$\"", "\"server$\""]
texts = " [function=\"leaf\" vagrant=\"eth1\" os=\"hashicorp/bionic64\" version=\"1.0.282\" memory=\"500\" config=\"./helper_scripts/config_production_switch.sh\" ] \n"

def writeTree(machineNumbers, file, baggage, parentName, index, tree):
	while(machineNumbers[index] == 0):    #procura o index aonde comeca a arvore, imaginando que possa nao existir um dos niveis
		index = index +1
	machineName = possibleNames[index]                  #array de strings que contem as possibilidades de nomes de maquinas
	for i in range(1,machineNumbers[index]+1):          #cria a arvore usando i como bagagem do nome (e.g se o pai é .1 o primero filho será .1.1)
		newbaggage = baggage + str('.'+str(i))          #cria a bagagem em relacao ao pai
		thisName = machineName.replace("$",newbaggage)  #replace na string do texto padrao
		tree.create_node(thisName, thisName, parent=parentName) #adiciona um nodo a arvore colocando o novo nome como filho e o nome recebido de cima como pai
		thisText = thisName + texts
		file.write(thisText)                                    #escreve no .dot
		if (index != 3):                                        #caso nao chegou na criacao da folha, inicia recursao
			writeTree(machineNumbers, file, newbaggage, thisName, index+1, tree)
