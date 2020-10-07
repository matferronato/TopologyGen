from All_hostname import provideHostnames
from Switch_CustomVlan import provideCustomVlan

def run(machines, protocol, graph):
    provideHostnames(machines, protocol)
    provideCustomVlan(machines, protocol, graph)
