
def provideHostnames(machines, protocol):
    for eachMachine in machines:
        try:
            if "hostname=True" in protocol[eachMachine]:
                file = open("../../../Automate/Host_Scripts/ip_info.txt", "r" , newline='\n')
                lines = file.readlines()
                dict = {}
                for line in lines:
                    line = line.split()
                    dict[line[0]] = line[1]
                file.close()

                file = open("../../../Automate/User_Scripts/tmp/hosts.txt", "w" , newline='\n')
                for eachKey in dict:
                    file.write(dict[eachKey].replace("/24","") + " " + eachKey + "\n")
                file.close()
                file = open("../../../Automate/Guest_Scripts/"+eachMachine+".cnfg", "a")
                file.write("cat /vagrant/User_Scripts/tmp/hosts.txt > /etc/hosts")
                file.close()
        except:
            continue
