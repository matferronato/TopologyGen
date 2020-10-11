import os

def provideCustomVlan(machines, protocol, graph):
    for eachMachine in machines:
        if "Switch" in eachMachine:
            try:
                if "CustomVlan=True" in protocol[eachMachine]:
                    files = [f for f in os.listdir('.') if os.path.isfile(f)]
                    numberOfVlans = 0
                    VlanList = []
                    for f in files:
                        if "Server_Vlan" in f:
                            numberOfVlans = numberOfVlans + 1
                            OneList = []
                            VlanList.append(OneList[:])
                    connections = graph.returnNode(eachMachine).connections
                    for eachConnectedMachine in connections:
                        for i in range(0,numberOfVlans):
                            if "Vlan"+str(i+1)+"=True" in protocol[eachConnectedMachine]:
                                VlanList[i].append(eachConnectedMachine)
                    file = open("../../../Automate/Guest_Scripts/"+eachMachine+".cnfg", "a")
                    file.write("sudo ip link set br0 down\n")
                    file.write("sudo ip link delete br0 type bridge\n")
                    for eachVlan in range(0,numberOfVlans):
                        file.write("sudo ip link add name br"+str(eachVlan)+" type bridge\n")
                        file.write("sudo ip link set dev br"+str(eachVlan)+" up\n")
                        interfaces = list(graph.returnNode(eachMachine).eth)
                        for eachInterface in interfaces:
                            trunk = int(eachInterface.replace("swp",""))
                            if trunk > 49:
                                file.write("sudo ip link add link "+str(eachInterface)+" name "+str(eachInterface)+"."+str((eachVlan+1)*100)+" type vlan id "+str((eachVlan+1)*100)+"\n")
                                file.write("sudo ip link set dev "+str(eachInterface)+"."+str((eachVlan+1)*100)+" up\n")
                                file.write("sudo ip link set dev "+str(eachInterface)+"."+str((eachVlan+1)*100)+" master br"+str(eachVlan)+"\n")
                    for i, eachServerList in enumerate(VlanList):
                        if (i == 0):
                            color = open("../../../Automate/Guest_Scripts/Color_Information/Blue.cnfg", "a")
                        elif (i == 1):
                            color = open("../../../Automate/Guest_Scripts/Color_Information/Red.cnfg", "a")
                        elif (i == 2):
                            color = open("../../../Automate/Guest_Scripts/Color_Information/Yellow.cnfg", "a")
                        else:
                            color = open("../../../Automate/Guest_Scripts/Color_Information/Purple.cnfg", "a")
                        if eachServerList:
                            for eachServer in eachServerList:
                                index = graph.returnNode(eachMachine).connections.index(eachServer)
                                eth = graph.returnNode(eachMachine).eth[index]
                                file.write("sudo ip link set dev "+eth+" master br"+str(i)+"\n")
                                color.write(eachMachine+" "+eachServer+"\n")
                        color.close()
                    file.close()
            except:
                continue
