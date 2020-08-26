# -*- coding: utf-8 -*-
from give_leaves_ip import giveLeavesIp
from give_routers_ip import giveRoutersIp
from give_intraconnection_ip import giveIntraConnectionIp

def provideIp(allIpNetworks, machineNumbers, parent, tree):
	giveLeavesIp(allIpNetworks, machineNumbers, tree)
	if(machineNumbers[0] != 0 or machineNumbers[1] != 0 ): #nao existindo roteadores, nao existe atribuicao de ip entre mesma layers, nem ip entre roteadores
		giveRoutersIp(allIpNetworks, machineNumbers, "root", tree)
		giveIntraConnectionIp(allIpNetworks, machineNumbers, "root", tree)
