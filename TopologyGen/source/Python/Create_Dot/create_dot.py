# -*- coding: utf-8 -*-
import os
from treelib import Tree, Node
from write_tree import writeTree
from write_connections import writeConnections
from write_toplevel_connections import writeTopLevelConnection

def createDot(machineNumbers, tree):
	baggage   = ""
	if os.path.exists("../../../TopologyInfo/topology.dot"): os.remove("../../../TopologyInfo/topology.dot")
	file = open("../../../TopologyInfo/topology.dot","w+")
	file.write("graph vx {\n")

	writeTree(machineNumbers, file, "", "root", 0, tree)
	tree.show(key=lambda x: x.tag, reverse=True, line_type='ascii-em')
	writeConnections(file, "root", tree)
	writeTopLevelConnection(file, "root", tree)

	file.write("\n}")
	file.close()
