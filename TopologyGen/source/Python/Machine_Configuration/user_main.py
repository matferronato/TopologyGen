from All_hostname import provideHostnames
from Switch_CustomVlan import provideCustomVlan
from Switch_hub import setAsHub

def run(machines, protocol, graph):
    provideHostnames(machines, protocol)
    provideCustomVlan(machines, protocol, graph)
    setAsHub(machines, protocol)
