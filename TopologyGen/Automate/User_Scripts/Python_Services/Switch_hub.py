
def setAsHub(machines, protocol):
    for eachMachine in machines:
        try:
            if "hub=True" in protocol[eachMachine]:
                file = open("../../../Automate/Guest_Scripts/"+eachMachine+".cnfg", "a")
                file.write("sudo apt-get -y install bridge-utils\n")
                file.write("brctl stp br0 off\n")
                file.write("brctl setageing br0 0\n")
                file.write("brctl setfd br0 0\n")
                file.close()
        except:
            continue
