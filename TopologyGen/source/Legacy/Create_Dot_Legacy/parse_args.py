# -*- coding: utf-8 -*-
import sys
argNumber = 7

def getOtherLevels(arguments, lastLevel, i):       #necessario para descobrir se existem outros leveis de comunicacao alem de servidor, switch, roteador
	if (lastLevel == 0) :                          #confere a ultima camada vista, este valor muda caso exista uma opcao como --l3 e em seguida um salto --l5
		newLevels = int(str(sys.argv[i])[-1:]) - 2 #no primeiro instante retira 2 referente as 3 camadas default(0,1,2), isso ajuda a saber quantas camadas foram adicionadas (--l3 e uma camada extra pois 3-2 =1)
		lastLevel = int(str(sys.argv[i])[-1:])     #salva o last level diferente de zero
	else:
		lastLevel = int(str(sys.argv[i])[-1:])
	for eachLevel in range(1, newLevels+1):                #para cada nova camada de maquinas, é adicionado maquinas extras. Por exemplo, no comando -l5 2, serao criadas 1 maquina para l3 uma para l4 e 2 para l5
		if (eachLevel == newLevels):                       #responsavel por colocar o valor setado no argumento para a camada requisitada
			arguments.append(int(str(sys.argv[i+1])))
		else:
			arguments.append(1)
	return arguments, lastLevel

def checkForNullValues(arguments):                 #define valores padroes para variaveis nao setadas
	if (arguments[0] == None): arguments[0] = 500
	if (arguments[1] == None): arguments[1] = "hashicorp/bionic64"
	if (arguments[2] == None): arguments[2] = 0
	if (arguments[3] == None): arguments[3] = 1
	if (arguments[4] == None): arguments[4] = 0
	if (arguments[5] == None): arguments[5] = 0
	if (arguments[6] == None): arguments[6] = 0
	return arguments

def parseArgs():                                      #realiza o parser dos argumentos vindos do prompt
	arguments = [None] * argNumber                    #cria uma lista prenchida com zeros de N posicoes
	lastLevel = 0                                     #valor padrao necessario para permitir insercao de camadas extras pulando as demais, exemplo digitar --l5 sem digitar --l3 e --l4
	for i in range(len(str(sys.argv).split())):
		if (str(sys.argv[i]) == "-m"): arguments [0] = str(sys.argv[i+1])
		elif (str(sys.argv[i]) == "-l"): arguments [1] = str(sys.argv[i+1])
		elif (str(sys.argv[i]) == "-o"): arguments [2] = str(sys.argv[i+1])
		elif (str(sys.argv[i]) == ("--l" + '0')): arguments [3] = int(str(sys.argv[i+1]))
		elif (str(sys.argv[i]) == ("--l" + '1')): arguments [4] = int(str(sys.argv[i+1]))
		elif (str(sys.argv[i]) == ("--l" + '2')): arguments [5] = int(str(sys.argv[i+1]))
		elif (str(sys.argv[i]) == ("--l" + '3')): arguments [6] = int(str(sys.argv[i+1]))
		#elif ("--l" in str(sys.argv[i])) :            #procura se existe mais layers que as padroes
		#	arguments, lastLevel = getOtherLevels(arguments, lastLevel, i)
	arguments = checkForNullValues(arguments)         #preenche valores vazios
	return arguments
