import os
import time
import shutil
from user_main import run


class UpwardTree():
    def __init__(self):
        self.treeDict = {}
    class Nodes():
        def __init__(self, name=None, payload=None, parent=None):
            self.name = name
            self.payload = payload
            self.parent = parent

    def addNode(self, name=None, payload=None, parent=None):
        node = self.Nodes(name, payload, parent)
        self.treeDict[name] = node
    def returnNode(self, name):
        return self.treeDict[name]

class Family():
    def __init__(self, name, family):
        self.myFamily = family
        self.me = name
    def getFamily(self):
        return self.myFamily
    def getNextSon(self, child):
        for i in range(0, len(self.myFamily)):
            if(i == len(self.myFamily)-1):
                return "None"
            elif(self.myFamily[i] == child):
                return self.myFamily[i+1]
    def getFirstBorn(self):
        return self.myFamily[0]


class Graph():
    def __init__(self):
        self.structure = {}

    class Node:
        def __init__(self):
            self.name = ""
            self.eth = []
            self.ip = []
            self.connections = []
            self.connectionIP = []
            self.connectionEth = []
        def getName(self):
            return self.name
        def getConnections(self):
            return self.interfaces
        def getConnectionsIP(self):
            ips = [eachConnection[0] for eachConnection in self.interfaces]
            return ips
        def getConnectionsIP(self):
            eths = [eachConnection[1] for eachConnection in self.interfaces]
            return eths

        def setName(self, name):
            self.name = name
        def setInterfaces(self, interfaces):
            for eachInterface in interfaces:
                self.ip.append(eachInterface[0])
                self.eth.append(eachInterface[1])
        def setConnections(self, connections):
            self.connections = connections
        def setConnectionIPs(self, connectionIP):
            self.connectionIP = connectionIP
        def setConnectionsEth(self, connectionEth):
            self.connectionEth = connectionEth

        def setIPbyIndex(self, ip, index):
            self.ip[index] = ip
        def setEthbyIndex(self, eth, index):
            self.eth[index] = eth
        def setConnectionbyIndex(self, connection, index):
            self.connections[index] = connection
        def setConnectionIPbyIndex(self, otherIps, index):
            self.connectionIP[index] = otherIps
        def setConnectionEthbyIndex(self, otherEths, index):
            self.connectionEth[index] = otherEths

        def define(self, name, interfaces, connections, otherIps, otherEths):
            self.setName(name)
            self.setInterfaces(interfaces)
            self.setConnections(connections)
            self.setConnectionIPs(otherIps)
            self.setConnectionsEth(otherEths)

    def addNode(self, name, interfaces, connections, otherIps, otherEths):
        node = self.Node()
        node.define(name, interfaces, connections, otherIps, otherEths)
        self.structure[name] = node
    def returnNode(self, name):
        return self.structure[name]
    def setNodeIPbyIndex(self, name, ip, index):
        self.structure[name].setIPbyIndex(ip, index)
    def setNodeEthbyIndex(self, name, eth, index):
        self.structure[name].setEthbyIndex(eth, index)
    def setNodeConnectionbyIndex(self, name, connection, index):
        self.structure[name].setConnectionbyIndex(connection, index)
    def setNodeConnectionIPbyIndex(self, name, connectionIP, index):
        self.structure[name].setConnectionIPbyIndex(connectionIP, index)
    def setNodeConnectionEthbyIndex(self, name, connectionEth, index):
        self.structure[name].setConnectionEthbyIndex(connectionEth, index)

def getAllMachineNames():
    print("creating structure for machine names")
    file = open("../../../Automate/Host_Scripts/all_machines.txt", "r" , newline='\n')
    lines = file.readlines()
    allMachinesList = []
    for line in lines:
        allMachinesList.append(line.strip())
    allMachinesList.sort
    return allMachinesList

def getAllIpNetworks():
    print("creating structure for ip networks")
    file = open("../../../Automate/Host_Scripts/all_ips.txt", "r" , newline='\n')
    lines = file.readlines()
    allIpsList = []
    for line in lines:
        allIpsList.append(line.strip())
    allIpsList.sort
    return allIpsList

def getSimplesConnections():
    print("looking for relevant machine nodes")
    file = open("../../../Automate/Host_Scripts/connections_detailed.txt", "r" , newline='\n')
    lines = file.readlines()
    setOsMachines = set()
    for line in lines:
        lineList      = line.split()
        firstMachine  = lineList[0]
        secondMachine = lineList[3]
        setOsMachines.add(firstMachine)
        setOsMachines.add(secondMachine)
    return setOsMachines

def getTypeMachines(allMachines):
    print("allowing setup for requested machines")
    typeMachines = {}
    listServers = []
    listSwitchs = []
    listRouters = []
    for oneMachine in allMachines:
        listServers.append((1,oneMachine)) if "Server" in oneMachine else listServers.append((0,oneMachine))
        listSwitchs.append((1,oneMachine)) if "Switch" in oneMachine else listSwitchs.append((0,oneMachine))
        listRouters.append((1,oneMachine)) if "Router" in oneMachine else listRouters.append((0,oneMachine))
    typeMachines["Servers"] =  [i[0] for i in listServers]
    typeMachines["Switchs"] =  [i[0] for i in listSwitchs]
    typeMachines["Routers"] =  [i[0] for i in listRouters]
    return typeMachines

def getMachineTypes(allMachines):
    print("recoverying machines for setup")
    machineTypes = {}
    listServers = []
    listSwitchs = []
    listRouters = []
    for oneMachine in allMachines:
        if "Server" in oneMachine : listServers.append(oneMachine)
        if "Switch" in oneMachine : listSwitchs.append(oneMachine)
        if "Router" in oneMachine : listRouters.append(oneMachine)
    machineTypes["Servers"] =  listServers
    machineTypes["Switchs"] =  listSwitchs
    machineTypes["Routers"] =  listRouters
    return machineTypes

def getIpAndEth(machineTypes):
    print("recoverying network and intarface information for each machine")
    file = open("../../../Automate/Host_Scripts/ip_info.txt", "r" , newline='\n')
    lines = file.readlines()
    interfaces = {}
    for line in lines:
        for eachKey in machineTypes:
            for eachMachine in machineTypes[eachKey]:
                if eachMachine in line:
                    defaultList = []
                    if eachMachine in interfaces:
                        interfaces[eachMachine].append((line.split()[1], line.split()[2]))
                    else:
                        defaultList.append((line.split()[1], line.split()[2]))
                        interfaces[eachMachine] = defaultList
    return interfaces

def checkForUserToggleOff(machineTypes,typeOfMachines,allMachines):
    file = open("../../../Automate/Host_Scripts/machine_info.txt", "r" , newline='\n')
    lines = file.readlines()
    for line in lines:
        i = 0;
        type=""
        for eachMachine in allMachines:
            if "Server" in eachMachine : type="Servers"
            if "Switch" in eachMachine : type="Switchs"
            if "Router" in eachMachine : type="Routers"
            if eachMachine in line:
                if "False" in line.split()[4]:
                    typeOfMachines[type][i] = 0
                    machineTypes[type].remove(eachMachine)

def getServicesByMachine(machinesByType):
    print("recoverying service information")
    file = open("../../../Automate/Host_Scripts/machine_info.txt", "r" , newline='\n')
    lines = file.readlines()
    servicesList = {}
    for line in lines:
        type=""
        servicesAvailable = []
        for eachKey in machinesByType:
            for eachMachine in machinesByType[eachKey]:
                if eachMachine in line:
                    i = 0
                    for eachItem in line.split():
                        if(i > 4):
                            servicesAvailable.append(eachItem)
                        i=i+1
                    servicesList[eachMachine]=servicesAvailable
    return servicesList

def getMachineByService(serviceByMachine):
    print("mapping each service for each machine")
    protocol = set()
    for eachMachine in serviceByMachine:
        for eachService in serviceByMachine[eachMachine]:
            oneProtocol = eachService.split("=", 1)[0]
            protocol.add(oneProtocol)
    machinesByService = {}
    for eachProtocol in protocol:
        machines = []
        for eachMachine in serviceByMachine:
            for eachService in serviceByMachine[eachMachine]:
                if "True" in eachService and eachProtocol in eachService:
                    machines.append(eachMachine)
        machinesByService[eachProtocol] = machines
    return machinesByService

def getAllNodes(machinesByType, interfaceInfo):
    print("creating topology data struct")
    graph = Graph()
    for eachKey in machinesByType:
        for eachMachine in machinesByType[eachKey]:
            connectedMachines = []
            connectedMachinesIP = []
            connectedMachinesEth = []
            for eachIP in interfaceInfo[eachMachine]:
                connectedMachines.append(None)
                connectedMachinesIP.append(None)
                connectedMachinesEth.append(None)
            graph.addNode(eachMachine,interfaceInfo[eachMachine],connectedMachines,connectedMachinesIP,connectedMachinesEth)
    return graph

def returnNetworkName(ip):
    point = 0
    currentIp = ""
    for eachChar in ip:
        if(point == 3):
            break
        if eachChar == '.':
            point = point+1
        currentIp = currentIp+eachChar
    return  currentIp

def setupOtherMachineInfo(thisGraph, myName, hisName, hisIP, hisEth, index):
        thisGraph.setNodeConnectionbyIndex(myName, hisName, index)
        thisGraph.setNodeConnectionIPbyIndex(myName, hisIP, index)
        thisGraph.setNodeConnectionEthbyIndex(myName, hisEth, index)

def linkEdges(thisGraph, allMachines_list):
    print("linking machine nodes")
    file = open("../../../Automate/Host_Scripts/connections_detailed.txt", "r" , newline='\n')
    lines = file.readlines()
    for line in lines:
        lineList      = line.split()
        firstMachine  = lineList[0]
        firstIP       = lineList[1]
        firstEth      = lineList[2]
        secondMachine = lineList[3]
        secondIP      = lineList[4]
        secondEth     = lineList[5]
        index = 0
        if(firstIP == "0.0.0.0"):
            while(thisGraph.returnNode(firstMachine).connections[index] != None):
                index=index+1
            setupOtherMachineInfo(thisGraph, firstMachine, secondMachine, secondIP, secondEth, index)
        else:
            try:
                index = thisGraph.returnNode(firstMachine).ip.index(firstIP)
            except:
                continue
            setupOtherMachineInfo(thisGraph, firstMachine, secondMachine, secondIP, secondEth, index)
        index = 0
        if(secondIP == "0.0.0.0"):
            while(thisGraph.returnNode(secondMachine).connections[index] != None):
                index=index+1
            setupOtherMachineInfo(thisGraph, secondMachine, firstMachine, firstIP, firstEth, index)
        else:
            index = thisGraph.returnNode(secondMachine).ip.index(secondIP)
            setupOtherMachineInfo(thisGraph, secondMachine, firstMachine, firstIP, firstEth, index)


def returnPlan(myTree, localNode):
    parent = myTree.returnNode(localNode)
    eth = parent.payload
    childEth = ""
    while (parent.parent != "None"):
        childEth = eth
        parent = myTree.returnNode(parent.parent)
        eth = parent.payload
    return childEth


def findEth(graph,router,network, blockedEth=None, blockedNode=None):
    myTree = UpwardTree()
    myTree.addNode(router, "null", "None")
    networkName = returnNetworkName(network)
    children = graph.returnNode(router).connections
    thisFamily = Family(router, children)
    killedFamilyMembers = {}
    if(blockedEth!=None):
        killedFamilyMembers[blockedNode] = 1
    fringe = []
    fringe.append(thisFamily)
    while(1):
        if len(fringe) == 0 :
            print(network, family.me ,"entrou em null")
            time.sleep(2)
            return "null_port"
        family = fringe.pop()
        child = family.getFirstBorn()
        killedFamilyMembers[family.me] = 1
        i = 0
        while(child != "None"):
            if(not (child in killedFamilyMembers)):
                if("Router" in family.me):
                    childFamily = graph.returnNode(child).connections
                    fringe.append(Family(child, childFamily))
                    myTree.addNode(child, graph.returnNode(family.me).eth[i], family.me)
                else:
                    killedFamilyMembers[child] = 1
            connectedIP =  graph.returnNode(family.me).ip[i]
            if(networkName in connectedIP):
                return returnPlan(myTree, child)
            child = family.getNextSon(child)
            i=i+1


def getRouterTables(graph, routers,networks):
    print("creating routing plan for each router")
    routerNames = {key: {} for key in routers}
    for eachRouter in routers:
        routerTable = {key: [] for key in networks}
        for eachNetwork in networks:
            eth = findEth(graph,eachRouter,eachNetwork)
            routerTable[eachNetwork] = eth
        routerNames[eachRouter] = routerTable
    return routerNames

def checkIfItemIsSimilar(list, item):
    for i in range(0, len(list)):
        if item in list[i]:
            return i
    return None


def runRoutesCleanUp(routeTable, graph, routers, networks):
    print("cleaning up loops")
    for eachRouter in routers:
        for eachNetwork in networks:
            #print(routeTable[eachRouter][eachNetwork],eachRouter,eachNetwork)
            indexNetworkConnection = graph.returnNode(eachRouter).eth.index(routeTable[eachRouter][eachNetwork])
            otherMachine = graph.returnNode(eachRouter).connections[indexNetworkConnection]
            if("Router" in otherMachine):
                #print(eachRouter,eachNetwork, routeTable[eachRouter][eachNetwork], "via", otherMachine)
                indexOtherNetworkConnection = graph.returnNode(otherMachine).eth.index(routeTable[otherMachine][eachNetwork])
                myMachine = graph.returnNode(otherMachine).connections[indexOtherNetworkConnection]
                if(eachRouter == myMachine):
                    networkName = returnNetworkName(eachNetwork)
                    indexIP = checkIfItemIsSimilar(graph.returnNode(eachRouter).ip, networkName)
                    if(indexIP == None):
                        newEth = findEth(graph,eachRouter,eachNetwork, 1, otherMachine)
                        routeTable[eachRouter][eachNetwork] = newEth
                        routeTable[eachRouter][eachNetwork]

def writeStaticRoutes(routersRouterTable, routers, networks):
    print("creating structure for machine names")
    file = open("../../../Automate/Host_Scripts/static_routes.txt", "w")
    for eachRouter in routers:
        file.write(eachRouter+"\n")
        for eachNetwork in networks:
            textToPrint = "    " + eachNetwork + " via -> "  + routersRouterTable[eachRouter][eachNetwork]+"\n"
            file.write(textToPrint)

def addDefaultGateWayServers(servers, graph):
    print("changing servers default gateway")
    Types = ["Router", "Switch", "Server"]
    for eachServer in servers:
        findDefaultGateWay = False
        file = open("../../../Automate/Guest_Scripts/"+eachServer+".cnfg", "a")
        connections = graph.returnNode(eachServer).connections
        for eachType in Types:
            if(findDefaultGateWay == True):
                break
            for i in range(0, len(connections)):
                try:
                    if(eachType in connections[i]):
                        findDefaultGateWay = True
                        network = returnNetworkName(graph.returnNode(eachServer).ip[i]) + "1"
                        interface = graph.returnNode(eachServer).eth[i]
                        file.write("sudo ip route del 0/0\n")
                        file.write("sudo ip route add 0/0 via " + network + " dev " +  interface + "\n")
                        break
                except:
                    continue

def setupSwitchs(switchs, serviceByMachine_dict, graph):
    print("seting up switchs")
    for eachSwitch in switchs:
        if "vlan=True" in serviceByMachine_dict[eachSwitch]:
            file = open("../../../Automate/Guest_Scripts/"+eachSwitch+".cnfg", "a")
            interfaces = list(graph.returnNode(eachSwitch).eth)
            machinesConnected = list(graph.returnNode(eachSwitch).connections)
            trunk = ""
            if "swp50" in interfaces:
                index = interfaces.index("swp50")
                interfaces.pop(index)
                trunk = "swp50"
                for eachVlan in range(0,2):
                    file.write("sudo ip link add name br"+str(eachVlan)+" type bridge\n")
                    file.write("sudo ip link set dev br"+str(eachVlan)+" up\n")
                    file.write("sudo ip link add link "+trunk+" name "+trunk+"."+str((eachVlan+1)*100)+" type vlan id "+str((eachVlan+1)*100)+"\n")
                    file.write("sudo ip link set dev "+trunk+"."+str((eachVlan+1)*100)+" up\n")
                    file.write("sudo ip link set dev "+trunk+"."+str((eachVlan+1)*100)+" master br"+str(eachVlan)+"\n")
            else:
                for eachVlan in range(0,2):
                    file.write("sudo ip link add name br"+str(eachVlan)+" type bridge\n")
            half1 = open("../../../Automate/Guest_Scripts/Color_Information/Blue.cnfg", "a")
            half2 = open("../../../Automate/Guest_Scripts/Color_Information/Red.cnfg", "a")
            for index, eachInterface in enumerate(interfaces):
                if index < len(eachInterface)/2:
                    file.write("sudo ip link set dev "+eachInterface+" master br0\n")
                    half1.write(eachSwitch+" "+machinesConnected[index]+"\n")
                else:
                    file.write("sudo ip link set dev "+eachInterface+" master br1\n")
                    half2.write(eachSwitch+" "+machinesConnected[index]+"\n")
            half1.close()
            half2.close()
        else:
            file = open("../../../Automate/Guest_Scripts/"+eachSwitch+".cnfg", "a")
            file.write("sudo ip link add name br0 type bridge\n")
            file.write("sudo ip link set dev br0 up\n")
            interfaces = graph.returnNode(eachSwitch).eth
            for eachEth in interfaces:
                file.write("sudo ip link set dev "+eachEth+" master br0\n")



def setupRouters(routers, routerTable, serviceByMachine_dict, graph):
    print("seting up routers")
    bgpAsNumber_dict = {}
    autonomous_system = 75
    for eachRouter in routerTable:
        bgpAsNumber_dict[eachRouter] = "76"+str(autonomous_system)
        autonomous_system = autonomous_system + 1
    for eachRouter in routerTable:
        file = open("../../../Automate/Guest_Scripts/"+eachRouter+".cnfg", "a")
        file.write("sudo sysctl -w net.ipv4.ip_forward=1\n")
        if "bgp=True" in serviceByMachine_dict[eachRouter]:
            applyBgp(eachRouter, file, bgpAsNumber_dict, graph)
        else:
            for eachNetwork in routerTable[eachRouter]:
                otherIPlist = []
                myEth = routerTable[eachRouter][eachNetwork]
                indexEth = graph.returnNode(eachRouter).eth.index(myEth)
                myIP = graph.returnNode(eachRouter).ip[indexEth]
                otherMachine = graph.returnNode(eachRouter).connections[indexEth]
                otherIPlist = graph.returnNode(otherMachine).ip
                otherIPindex = checkIfItemIsSimilar(otherIPlist,returnNetworkName(myIP))
                if otherIPindex != None:
                    otherIP = graph.returnNode(otherMachine).ip[otherIPindex].replace("/24","")
                    file.write("sudo ip route add "+eachNetwork+" via " + otherIP + " dev " + myEth + "\n")

def applyBgp(router, file, bgpAsNumber, graph):
    file.write("sudo apt-get -y install quagga\n")
    file.write("sudo yum install quagga\n")
    shutil.copyfile("../../../Automate/Host_Scripts/Base_Files/bgpd.conf", "../../../Automate/Guest_Scripts/BGP_Information/"+router+"_bgpd.conf")
    shutil.copyfile("../../../Automate/Host_Scripts/Base_Files/zebra.conf", "../../../Automate/Guest_Scripts/BGP_Information/"+router+"_zebra.conf")
    shutil.copyfile("../../../Automate/Host_Scripts/Base_Files/vtysh.conf", "../../../Automate/Guest_Scripts/BGP_Information/"+router+"_vtysh.conf")

    bgpFile = open("../../../Automate/Guest_Scripts/BGP_Information/"+router+"_bgpd.conf", "r" , newline='\n')
    indexBGP = 0
    linesBGP = bgpFile.readlines()
    for i, eachLine in  enumerate(linesBGP):
        if "!router bgp 7675" in eachLine:
            indexBGP = i+1
    bgpFile.close()

    zebraFile = open("../../../Automate/Guest_Scripts/BGP_Information/"+router+"_zebra.conf", "r" , newline='\n')
    indexZebra = 0
    linesZebra = zebraFile.readlines()
    for i, eachLine in  enumerate(linesZebra):
        if "enable" in eachLine:
            indexZebra = i+1
    zebraFile.close()

    bgpAddedText = "router bgp " + bgpAsNumber[router] + "\n"
    bgpAddedText = bgpAddedText + "bgp router-id " + graph.returnNode(router).ip[0].replace("/24","") +"\n"
    connections = graph.returnNode(router).connections
    ips = graph.returnNode(router).ip
    eths = graph.returnNode(router).eth
    for eachIp in ips:
        networkName = returnNetworkName(eachIp)+"0/24"
        bgpAddedText = bgpAddedText + "  network "+  networkName+"\n"

    for i, eachConnection in enumerate(connections):
        if "Router" in eachConnection:
            currentIP = ips[i]
            indexIP = checkIfItemIsSimilar(graph.returnNode(eachConnection).ip, returnNetworkName(currentIP))
            ip = graph.returnNode(eachConnection).ip[indexIP]
            bgpAddedText = bgpAddedText + "  neighbor " + ip.replace("/24","") + " remote-as "+ bgpAsNumber[eachConnection] +"\n"

    zebraAddedText = "!\n"
    for i, eachIp in enumerate(ips):
        zebraAddedText = zebraAddedText + "interface "+  eths[i]+"\n"
        zebraAddedText = zebraAddedText + "ip address "+  eachIp+"\n!\n"

    linesBGP.insert(indexBGP, bgpAddedText)
    linesZebra.insert(indexZebra, zebraAddedText)

    bgpFile = open("../../../Automate/Guest_Scripts/BGP_Information/"+router+"_bgpd.conf", "w" , newline='\n')
    linesBGP = "".join(linesBGP)
    bgpFile.write(linesBGP)
    bgpFile.close()

    zebraFile = open("../../../Automate/Guest_Scripts/BGP_Information/"+router+"_zebra.conf", "w" , newline='\n')
    linesZebra = "".join(linesZebra)
    zebraFile.write(linesZebra)
    zebraFile.close()

    thisBgpFile = "/vagrant/Guest_Scripts/BGP_Information/"+router+"_bgpd.conf"
    thisZebraFile =  "/vagrant/Guest_Scripts/BGP_Information/"+router+"_zebra.conf"
    thisVtyshFile =  "/vagrant/Guest_Scripts/BGP_Information/"+router+"_vtysh.conf"
    file.write("sudo cat " +thisBgpFile+" > /etc/quagga/bgpd.conf\n")
    file.write("sudo cat " +thisZebraFile+" > /etc/quagga/zebra.conf\n")
    file.write("sudo cat " +thisVtyshFile+" > /etc/quagga/vtysh.conf\n")
    file.write("sudo chown quagga:quagga /etc/quagga/*.conf\n")
    file.write("sudo chown quagga:quaggavty /etc/quagga/vtysh.conf\n")
    file.write("sudo chmod 640 /etc/quagga/*.conf\n")
    file.write("sudo service bgpd start\n")
    file.write("sudo service zebra start\n")

#def applyVxlanRouters(routers, routerTable, serviceByMachine_dict, graph):
#    print("applying Vxlan")
#    vxlanNetworkInterfaces = []
#    for eachRouter in routerTable:
#        if "vxlan=True" in serviceByMachine_dict[eachRouter]:
#            file = open("../../../Automate/Guest_Scripts/"+eachRouter+".cnfg", "a")
#
#            if not ("bgp=True" in serviceByMachine_dict[eachRouter]):
#                #creates default gateway to send vxlan ip package
#                indexEth = graph.returnNode(eachRouter).eth.index("eth100")
#                network = graph.returnNode(eachRouter).ip[indexEth]
#                otherMachine = graph.returnNode(eachRouter).connections[indexEth]
#                indexIP = checkIfItemIsSimilar(graph.returnNode(otherMachine).ip, returnNetworkName(network))
#                ip = graph.returnNode(otherMachine).ip[indexIP]
#                file.write("sudo ip route del 0/0\n")
#                file.write("sudo ip route add 0/0 via " + ip.replace("/24","") + " dev eth100 \n")
#
#
#            file.write("sudo ip link add br0 type bridge\n")
#            file.write("sudo ip link set br0 up\n")
#            file.write("sudo ip link add name vxlan10 type vxlan id 10 dev eth100 group 239.1.1.1 dstport 4789\n")
#
#            correctEth = ""
#            interfaces = list(graph.returnNode(eachRouter).eth)
#            connections = graph.returnNode(eachRouter).connections
#            for i, eachMachine in enumerate(connections):
#                if "Server" in eachMachine:
#                    if "vxlanIp=True" in serviceByMachine_dict[eachMachine]:
#                        correctEth = graph.returnNode(eachRouter).eth[i]
#
#            if correctEth == "":
#                if "eth50" in interfaces:
#                    file.write("sudo ip link set dev eth50 master br0\n")
#                    indexNetworkConnection = graph.returnNode(eachRouter).eth.index("eth50")
#            else:
#                file.write("sudo ip link set dev "+correctEth+" master br0\n")
#                indexNetworkConnection = graph.returnNode(eachRouter).eth.index(correctEth)
#            ipNetwork = graph.returnNode(eachRouter).ip[indexNetworkConnection]
#            vxlanNetworkInterfaces.append(ipNetwork)
#            file.write("sudo ip link set dev vxlan10 master br0\n")
#            file.write("sudo ip link set vxlan10 up\n")
#    return vxlanNetworkInterfaces


def applyVxlanRouters(routers, routerTable, serviceByMachine_dict, graph):
    print("applying Vxlan")
    vxlanNetworkInterfaces = []
    vxlanEndPoints = []

    for eachRouter in routerTable:
        if "vxlan=True" in serviceByMachine_dict[eachRouter]:
            indexEth = graph.returnNode(eachRouter).eth.index("eth100")
            network = graph.returnNode(eachRouter).ip[indexEth]
            vxlanEndPoints.append(network)
            print(eachRouter, network)
            continue


    for eachRouter in routerTable:
        if "vxlan=True" in serviceByMachine_dict[eachRouter]:
            file = open("../../../Automate/Guest_Scripts/"+eachRouter+".cnfg", "a")

            if not ("bgp=True" in serviceByMachine_dict[eachRouter]):
                #creates default gateway to send vxlan ip package
                indexEth = graph.returnNode(eachRouter).eth.index("eth100")
                network = graph.returnNode(eachRouter).ip[indexEth]
                otherMachine = graph.returnNode(eachRouter).connections[indexEth]
                indexIP = checkIfItemIsSimilar(graph.returnNode(otherMachine).ip, returnNetworkName(network))
                ip = graph.returnNode(otherMachine).ip[indexIP]
                file.write("sudo ip route del 0/0\n")
                file.write("sudo ip route add 0/0 via " + ip.replace("/24","") + " dev eth100 \n")


            file.write("sudo ip link add br0 type bridge\n")
            file.write("sudo ip link set br0 up\n")
            print(vxlanEndPoints)
            if vxlanEndPoints[0] in graph.returnNode(eachRouter).ip:
                file.write("sudo ip link add vxlan10 type vxlan id 100 local " + vxlanEndPoints[0].replace("/24","") + " remote " + vxlanEndPoints[1].replace("/24","") + " dev eth100 dstport 4789\n")
            else:
                file.write("sudo ip link add vxlan10 type vxlan id 100 local " + vxlanEndPoints[1].replace("/24","") + " remote " + vxlanEndPoints[0].replace("/24","") + " dev eth100 dstport 4789\n")

            correctEth = ""
            interfaces = list(graph.returnNode(eachRouter).eth)
            connections = graph.returnNode(eachRouter).connections
            for i, eachMachine in enumerate(connections):
                if "Server" in eachMachine:
                    if "vxlanIp=True" in serviceByMachine_dict[eachMachine]:
                        correctEth = graph.returnNode(eachRouter).eth[i]
                        file.write("sudo ip link set dev "+correctEth+" master br0\n")
                        indexNetworkConnection = graph.returnNode(eachRouter).eth.index(correctEth)
                        ipNetwork = graph.returnNode(eachRouter).ip[indexNetworkConnection]
                        vxlanNetworkInterfaces.append(ipNetwork)
            if correctEth == "":
                if "eth50" in interfaces:
                    file.write("sudo ip link set dev eth50 master br0\n")
                    indexNetworkConnection = graph.returnNode(eachRouter).eth.index("eth50")
                    ipNetwork = graph.returnNode(eachRouter).ip[indexNetworkConnection]
                    vxlanNetworkInterfaces.append(ipNetwork)
            file.write("sudo ip link set dev vxlan10 master br0\n")
            file.write("sudo ip link set vxlan10 up\n")
    return vxlanNetworkInterfaces

def applyVxlanServers(servers, serviceByMachine_dict, vxlanNetworkInterfaces, graph):
    print("applying Vxlan")
    j = 1
    for i, eachServer in enumerate(servers):
        if "vxlanIp=True" in serviceByMachine_dict[eachServer]:
            file = open("../../../Automate/Guest_Scripts/"+eachServer+".cnfg", "a")
            networks = list(graph.returnNode(eachServer).ip)
            for index, eachNetwork in enumerate(networks):
                networkName = returnNetworkName(eachNetwork)
                if checkIfItemIsSimilar(vxlanNetworkInterfaces, networkName) != None:
                    correctEth =  graph.returnNode(eachServer).eth[index]
                    otherMachine = graph.returnNode(eachServer).connections[index]
                    file.write("sudo ip addr add 100.100.100."+str(j)+"/24 dev "+correctEth+"\n")
                    color = open("../../../Automate/Guest_Scripts/Color_Information/Yellow.cnfg", "a")
                    color.write(otherMachine+" "+eachServer+"\n")
                    color.close()
                    j=j+1

def main():
    allMachines_list = getAllMachineNames()
    allNetworks_list = getAllIpNetworks()
    allMachinesConneted_List = getSimplesConnections()
    typeOfMachines_dict = getTypeMachines(allMachinesConneted_List)
    machinesByType_dict = getMachineTypes(allMachinesConneted_List)
    interfaceInfo_dict = getIpAndEth(machinesByType_dict)
    checkForUserToggleOff(machinesByType_dict,typeOfMachines_dict,allMachinesConneted_List)
    serviceByMachine_dict = getServicesByMachine(machinesByType_dict)
    machineByService_dict = getMachineByService(serviceByMachine_dict)
    graph = getAllNodes(machinesByType_dict, interfaceInfo_dict)
    linkEdges(graph, allMachinesConneted_List)
    routersRouterTable_dict = getRouterTables(graph, machinesByType_dict["Routers"],allNetworks_list)
    runRoutesCleanUp(routersRouterTable_dict, graph, machinesByType_dict["Routers"],allNetworks_list)
    runRoutesCleanUp(routersRouterTable_dict, graph, machinesByType_dict["Routers"],allNetworks_list)
    writeStaticRoutes(routersRouterTable_dict, machinesByType_dict["Routers"],allNetworks_list)

    addDefaultGateWayServers(machinesByType_dict["Servers"] , graph)
    setupSwitchs(machinesByType_dict["Switchs"], serviceByMachine_dict, graph)
    setupRouters(machinesByType_dict["Routers"], routersRouterTable_dict, serviceByMachine_dict, graph)
    vxlanNetworkInterfaces = applyVxlanRouters(machinesByType_dict["Routers"], routersRouterTable_dict, serviceByMachine_dict, graph)
    applyVxlanServers(machinesByType_dict["Servers"], serviceByMachine_dict, vxlanNetworkInterfaces, graph)

    run(allMachines_list, serviceByMachine_dict, graph)
    print()

#-----------------------------------------------------
if __name__ == '__main__': # chamada da funcao principal
    main()
