# -*- coding: utf-8 -*-
from treelib import Tree, Node
from parse_args import parseArgs
from create_dot import createDot
from create_ips import createIps

def main():
	arguments = parseArgs()
	memory    = arguments[0]
	linux     = arguments[1]
	oobRouter = arguments[6]        #out of bound routers
	routers   = arguments[5]
	switchs   = arguments[4]
	servers   = arguments[3]
	machineNumbers = []
	tree = Tree();
	tree.create_node("root", "root")
	machineNumbers.extend([oobRouter, routers, switchs, servers])

	createDot(machineNumbers, tree)
	createIps(machineNumbers, tree)

#-----------------------------------------------------
if __name__ == '__main__': # chamada da funcao principal
    main()
