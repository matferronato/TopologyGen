import os
import time

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
            index = thisGraph.returnNode(firstMachine).ip.index(firstIP)
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
            time.sleep(30)
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
                    connectedIP =  graph.returnNode(family.me).ip[i]
                    myTree.addNode(child, graph.returnNode(family.me).eth[i], family.me)
                    if(networkName in connectedIP):
                        return returnPlan(myTree, child)
                else:
                    killedFamilyMembers[child] = 1
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
        print(eachRouter)
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
                        print("old eth", routeTable[eachRouter][eachNetwork])
                        newEth = findEth(graph,eachRouter,eachNetwork, 1, otherMachine)
                        routeTable[eachRouter][eachNetwork] = newEth
                        print("new eth", newEth)
                        routeTable[eachRouter][eachNetwork]
                        print(eachNetwork)
                        print(eachRouter, routeTable[eachRouter][eachNetwork], "via", otherMachine)
                        print(otherMachine, routeTable[otherMachine][eachNetwork], "via", myMachine)
        print("")

def main():
    allMachines_list = getAllMachineNames()
    allNetworks_list = getAllIpNetworks()
    typeOfMachines_dict = getTypeMachines(allMachines_list)
    machinesByType_dict = getMachineTypes(allMachines_list)
    interfaceInfo_dict = getIpAndEth(machinesByType_dict)
    checkForUserToggleOff(machinesByType_dict,typeOfMachines_dict,allMachines_list)
    serviceByMachine_dict = getServicesByMachine(machinesByType_dict)
    machineByService_dict = getMachineByService(serviceByMachine_dict)
    graph = getAllNodes(machinesByType_dict, interfaceInfo_dict)
    linkEdges(graph, allMachines_list)
    routersRouterTable_dict = getRouterTables(graph, machinesByType_dict["Routers"],allNetworks_list)
    runRoutesCleanUp(routersRouterTable_dict, graph, machinesByType_dict["Routers"],allNetworks_list)
    print("you're welcome : )")


#-----------------------------------------------------
if __name__ == '__main__': # chamada da funcao principal
    main()
