
def provideCustomVlan(machines, protocol, graph):
    for eachMachine in machines:
        if "Switch" in eachMachine:
            try:
                if "CustomVlan=True" in protocol[eachMachine]:
                    vlan1 = []
                    vlan2 = []
                    connections = graph.returnNode(eachMachine).connections
                    for eachConnectedMachine in connections:
                        if "Vlan1=True" in protocol[eachConnectedMachine]:
                            vlan1.append(eachConnectedMachine)
                        if "Vlan2=True" in protocol[eachConnectedMachine]:
                            vlan2.append(eachConnectedMachine)
                    file = open("../../../Automate/Guest_Scripts/"+eachMachine+".cnfg", "a")
                    file.write("sudo ip link set br0 down\n")
                    file.write("sudo ip link delete br0 type bridge\n")
                    for eachVlan in range(0,2):
                        file.write("sudo ip link add name br"+str(eachVlan)+" type bridge\n")
                        file.write("sudo ip link set dev br"+str(eachVlan)+" up\n")
                        interfaces = list(graph.returnNode(eachMachine).eth)
                        for eachInterface in interfaces:
                            trunk = int(eachInterface.replace("swp",""))
                            if trunk > 49:
                                file.write("sudo ip link add link "+str(eachInterface)+" name "+str(eachInterface)+"."+str((eachVlan+1)*100)+" type vlan id "+str((eachVlan+1)*100)+"\n")
                                file.write("sudo ip link set dev "+str(eachInterface)+"."+str((eachVlan+1)*100)+" up\n")
                                file.write("sudo ip link set dev "+str(eachInterface)+"."+str((eachVlan+1)*100)+" master br"+str(eachVlan)+"\n")
                    for eachServer in vlan1:
                        index = graph.returnNode(eachMachine).connections.index(eachServer)
                        eth = graph.returnNode(eachMachine).eth[index]
                        file.write("sudo ip link set dev "+eth+" master br0\n")
                    for eachServer in vlan2:
                        index = graph.returnNode(eachMachine).connections.index(eachServer)
                        eth = graph.returnNode(eachMachine).eth[index]
                        file.write("sudo ip link set dev "+eth+" master br1\n")
                    file.close()
            except:
                continue
