using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class button_handler : MonoBehaviour
{
    public GameObject ServerButton;
    public GameObject SwitchButton;
    public GameObject RouterButton;
    public GameObject Position2d;
    public GameObject StartButton;
    public GameObject StopButton;

    public static Queue<GameObject> serverObjQueue = new Queue<GameObject>();
    public static Queue<GameObject> switchObjQueue = new Queue<GameObject>();
    public static Queue<GameObject> routerObjQueue = new Queue<GameObject>();
    public static List<GameObject> lineObjList = new List<GameObject>();
    public static List<Tuple<GameObject, GameObject>> connectionsObjList= new List<Tuple<GameObject, GameObject>>();

    public static Queue serverNameQueue = new Queue();
    public static Queue switchNameQueue = new Queue();
    public static Queue routerNameQueue = new Queue();

    public static int totalServerNumber;
    public static int totalSwitchNumber;
    public static int totalRouterNumber;
    public bool createServer;
    public bool createSwitch;
    public bool createRouter;

    public static bool allowLines;
    public static bool startRunning;
    public static bool optionRunning;

    public List<string> possibleNetworks = new List<string>();
    public List<GameObject> allMachines = new List<GameObject>();

    public static float MenuPositionX;
    public static float upperBoundary;
    public static float bottomBoundary;
    public static float leftBoundary;
    public static float rightBoundary;

    // Start is called before the first frame update
    void setScreenResolutionValues()
    {
        int x = Screen.width;
        int y = Screen.height;
        MenuPositionX = 558;
        upperBoundary = 56f;
        bottomBoundary = 0.1f;
        leftBoundary = 0.2f;
        rightBoundary = 56f;
        if (x == 800 && y == 600)
        {
            MenuPositionX = 558;
            upperBoundary = 56f;
            bottomBoundary = 0.1f;
            leftBoundary = 0.2f;
            rightBoundary = 56f;
        }
    }

    void Start()
    {
        setScreenResolutionValues();
        totalServerNumber = 0;
        totalSwitchNumber = 0;
        totalRouterNumber = 0;
        allowLines = false;
        startRunning = false;
        optionRunning = false;
        createServer = false;
        createSwitch = false;
        createRouter = false;
        StartButton.SetActive(true);
        StopButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        checkIfKeyPressed();
        if (Input.mousePosition.x < 558)
        {
            if (createServer == true)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (magnetic_dop.deletedServer == true)
                    {
                        magnetic_dop.deletedServer = false;
                    }
                    else
                    {
                        serverNameQueue.Enqueue(totalServerNumber);
                        totalServerNumber = totalServerNumber + 1;
                    }
                    var thisServerIcon = Instantiate(ServerButton, new Vector2(Input.mousePosition.x, Input.mousePosition.y), Quaternion.identity);
                    thisServerIcon.transform.parent = gameObject.transform;
                    serverObjQueue.Enqueue(thisServerIcon);
                }
            }
            else if (createSwitch == true)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (magnetic_dop.deletedSwitch == true)
                    {
                        magnetic_dop.deletedSwitch = false;
                    }
                    else
                    {
                        switchNameQueue.Enqueue(totalSwitchNumber);
                        totalSwitchNumber = totalSwitchNumber + 1;
                    }
                    var thisSwitchIcon = Instantiate(SwitchButton, new Vector2(Input.mousePosition.x, Input.mousePosition.y), Quaternion.identity);
                    thisSwitchIcon.transform.parent = gameObject.transform;
                    switchObjQueue.Enqueue(thisSwitchIcon);
                }
            }
            else if (createRouter == true)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    if (magnetic_dop.deletedRouter == true)
                    {
                        magnetic_dop.deletedRouter = false;
                    }
                    else
                    {
                        routerNameQueue.Enqueue(totalRouterNumber);
                        totalRouterNumber = totalRouterNumber + 1;
                    }
                    var thisRouterIcon = Instantiate(RouterButton, new Vector2(Input.mousePosition.x, Input.mousePosition.y), Quaternion.identity);
                    thisRouterIcon.transform.parent = gameObject.transform;
                    routerObjQueue.Enqueue(thisRouterIcon);
                }
            }
        }
    }


    public void checkIfKeyPressed()
    {
        if (optionRunning == false)
        {
            if (Input.GetKeyDown("w"))
            {
                OnClickSwitch();
            }
            else if (Input.GetKeyDown("c"))
            {
                OnClickLines();
            }
            else if (Input.GetKeyDown("r"))
            {
                OnClickRouter();
            }
            else if (Input.GetKeyDown("s"))
            {
                OnClickServer();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject Linesbutton = GameObject.Find("Lines_Button");
                GameObject Routerbutton = GameObject.Find("Router_Button");
                GameObject Switchbutton = GameObject.Find("Switch_Button");
                GameObject Serverbutton = GameObject.Find("Server_Button");
                allowLines = false;
                createServer = false;
                createSwitch = false;
                createRouter = false;
                Serverbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
                Switchbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
                Routerbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
                Linesbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (startRunning == false)
                {
                    OnClickRun();
                }

            }
        }
        else
        {
            if (Input.anyKey)
            {
                allowLines = false;
                createServer = false;
                createSwitch = false;
                createRouter = false;
            }
        }
    }

    public void OnClickServer()
    {
        GameObject Linesbutton = GameObject.Find("Lines_Button");
        GameObject Routerbutton = GameObject.Find("Router_Button");
        GameObject Switchbutton = GameObject.Find("Switch_Button");
        GameObject Serverbutton = GameObject.Find("Server_Button");
        if (createServer == true) { 
            createServer = false;
            Serverbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        }
        else {
            allowLines = false;
            createServer = true;
            createSwitch = false;
            createRouter = false;
            Serverbutton.GetComponent<Image>().color = new Color32(135, 135, 135, 255);
            Switchbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            Routerbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            Linesbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        }
    }

    public void OnClickSwitch()
    {
        GameObject Linesbutton = GameObject.Find("Lines_Button");
        GameObject Routerbutton = GameObject.Find("Router_Button");
        GameObject Serverbutton = GameObject.Find("Server_Button");
        GameObject Switchbutton = GameObject.Find("Switch_Button");
        if (createSwitch == true) { 
            createSwitch = false;
            Switchbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        }
        else
        {
            allowLines = false;
            createServer = false;
            createSwitch = true;
            createRouter = false;
            Switchbutton.GetComponent<Image>().color = new Color32(135, 135, 135, 255);
            Serverbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            Routerbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            Linesbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        }
    }

    public void OnClickRouter()
    {
        GameObject Linesbutton = GameObject.Find("Lines_Button");
        GameObject Serverbutton = GameObject.Find("Server_Button");
        GameObject Switchbutton = GameObject.Find("Switch_Button");
        GameObject Routerbutton = GameObject.Find("Router_Button");
        if (createRouter == true) { 
            createRouter = false;
            Routerbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        }
        else
        {
            allowLines = false;
            createServer = false;
            createSwitch = false;
            createRouter = true;
            Routerbutton.GetComponent<Image>().color = new Color32(20, 20, 20, 255);
            Serverbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            Switchbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            Linesbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        }
    }
    public void OnClickLines()
    {
        GameObject Routerbutton = GameObject.Find("Router_Button");
        GameObject Serverbutton = GameObject.Find("Server_Button");
        GameObject Switchbutton = GameObject.Find("Switch_Button");
        GameObject Linesbutton = GameObject.Find("Lines_Button");
        if (allowLines == true) { 
            allowLines = false;
            Linesbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        } 
        else { 
            allowLines = true;
            createServer = false;
            createSwitch = false;
            createRouter = false;
            Linesbutton.GetComponent<Image>().color = new Color32(135, 135, 135, 255);
            Serverbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            Switchbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
            Routerbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        }
    }

    public void OnClickRun()
    {
        allMachines.Clear();
        possibleNetworks.Clear();
        setupTopologyFiles();
        startRunning = true;
        string strCmdText ="";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            strCmdText = "/C ..\\..\\Windows\\bash.exe /mnt/c/Users/matheus_ferronato/MyProjects/TCC/TopologyGen/TopologyGen/create.run --l0 " + totalServerNumber.ToString() + " --l1 " + totalSwitchNumber.ToString() + " --l2 " + totalRouterNumber.ToString();
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            strCmdText = "/C ..\\..\\..\\Windows\\bash.exe /mnt/c/Users/matheus_ferronato/MyProjects/TCC/TopologyGen/TopologyGen/create.run --l0 " + totalServerNumber.ToString() + " --l1 " + totalSwitchNumber.ToString() + " --l2 " + totalRouterNumber.ToString();
        }
        System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        //StartCoroutine(readNameFile());
        StopButton.SetActive(true);
        StartButton.SetActive(false);
    }

    public void OnClickStop()
    {
        startRunning = false;
        StopButton.SetActive(false);
        StartButton.SetActive(true);
    }

    public void clearAll()
    {
        totalServerNumber = 0;
        totalSwitchNumber = 0;
        totalRouterNumber = 0;
        connectionsObjList.Clear();
        possibleNetworks.Clear();
        allMachines.Clear();
        int currentTotal = serverObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(serverObjQueue.Dequeue()); }
        currentTotal = switchObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(switchObjQueue.Dequeue()); }
        currentTotal = routerObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(routerObjQueue.Dequeue()); }
        for (int i = 0; i < lineObjList.Count; i++)
        {
            GameObject currentLine = lineObjList[i];
            Destroy(currentLine);
        }
        lineObjList.Clear();

    }

    public void writeDotFile()
    {
        List<string> Lines = new List<string>();
        string ConnectionText = "\"!\":\"@\" -- \"$\":\"%\"";
        string CreationText = "\"%\" [function=\"leaf\" vagrant=\"eth1\" os=\"!\" version=\"@\" memory=\"(\" config=\"./helper_scripts/config_production_switch.sh\" ]";
        Lines.Add("graph vx {");
        string path = "";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"C:..\\..\\TopologyInfo\\topology.dot";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"C:..\\..\\..\\TopologyInfo\\topology.dot";
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            foreach (GameObject machine in allMachines)
            {
                string currentStringCreation = CreationText;
                string currentName = machine.name;
                string currentOS = machine.GetComponent<drag_and_drop>().OS;
                string currentOSversion = machine.GetComponent<drag_and_drop>().OSversion;
                string currentMemory = machine.GetComponent<drag_and_drop>().memory.ToString();
                currentStringCreation = currentStringCreation.Replace("%", currentName).Replace("!", currentOS).Replace("@", currentOSversion).Replace("(", currentMemory);
                Lines.Add(currentStringCreation);
            }
            foreach (var connection in connectionsObjList)
            {
                int index1 = connection.Item1.GetComponent<drag_and_drop>().connections.IndexOf(connection.Item2);
                int index2 = connection.Item2.GetComponent<drag_and_drop>().connections.IndexOf(connection.Item1);
                string eth1 = connection.Item1.GetComponent<drag_and_drop>().eth[index1];
                string eth2 = connection.Item2.GetComponent<drag_and_drop>().eth[index2];
                string currentStringConnection = ConnectionText;
                currentStringConnection = currentStringConnection.Replace("!", connection.Item1.name).Replace("@", eth1).Replace("$", connection.Item2.name).Replace("%", eth2);
                Lines.Add(currentStringConnection);
            }
            Lines.Add("}");
            foreach (string line in Lines)
            {
                file.Write(line);
                file.Write("\n");
            }
        }
    }

    public void setupTopologyFiles()
    {
        createConnectionFile();
        writeDotFile();
    }

    public void cleanIPfromSwitchs()
    {
        int currentTotal = switchObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) {
            var thisSwitch = switchObjQueue.Dequeue();
            for (int j = 0; j < thisSwitch.GetComponent<drag_and_drop>().ip.Count; j++)
            {
                thisSwitch.GetComponent<drag_and_drop>().ip[j] = "0.0.0.0";
            }
            switchObjQueue.Enqueue(thisSwitch);
        }
    }

    public void fixIP()
    {
        if (routerObjQueue.Count == 0)
        {
            int currentTotal = serverObjQueue.Count;
            for (int i = 0; i < currentTotal; i++)
            {
                var thisServer = serverObjQueue.Dequeue();
                for (int j = 0; j < thisServer.GetComponent<drag_and_drop>().ip.Count; j++)
                {
                    thisServer.GetComponent<drag_and_drop>().ip[j] = "1.1.1.Y/24";
                }
                serverObjQueue.Enqueue(thisServer);
            }
        }
    }

    public void writeCorrectIPInterface(GameObject thisMachine, int index, string ip)
    {
        string currentEth = thisMachine.GetComponent<drag_and_drop>().eth[index];
        thisMachine.GetComponent<drag_and_drop>().attatchedText[index].GetComponent<Text>().text = "IP = " + ip + "\n" + currentEth;
    }

    public void changeDefaultIPIdentificator()
    {
        string defaultIP = "X.X.X.Y/24";
        
        for(int i = 1; i < address_manager.currentValue; i++)
        {
            string currentIP = defaultIP.Replace("X", i.ToString());
            possibleNetworks.Add(currentIP);
        }
        foreach(string thisIp in possibleNetworks)
        {
            int i = 3;
            bool flagFirstRouter = false;
            foreach(GameObject thisMachine in allMachines)
            {
                for (int indexIP = 0; indexIP < thisMachine.GetComponent<drag_and_drop>().ip.Count; indexIP++)
                {
                    string connectionIP = thisMachine.GetComponent<drag_and_drop>().ip[indexIP];
                    if (connectionIP.Contains(thisIp))
                    {
                        if (thisMachine.name.Contains("Router"))
                        {
                            if(flagFirstRouter == false)
                            {
                                string newIP = thisIp.Replace("Y", "1");
                                thisMachine.GetComponent<drag_and_drop>().ip[indexIP] = newIP;
                                flagFirstRouter = true;
                            }
                            else
                            {
                                string newIP = thisIp.Replace("Y", "2");
                                thisMachine.GetComponent<drag_and_drop>().ip[indexIP] = newIP;
                            }
                        }
                        else
                        {
                            string newIP = thisIp.Replace("Y", i.ToString());
                            i++;
                            thisMachine.GetComponent<drag_and_drop>().ip[indexIP] = newIP;
                        }
                    }
                }
            }
        }
    }

    public void getAllMachines()
    {
        int currentTotal = routerObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) {
            GameObject oneRouter = routerObjQueue.Dequeue();
            allMachines.Add(oneRouter);
            routerObjQueue.Enqueue(oneRouter);
        }
        currentTotal = serverObjQueue.Count;
        for (int i = 0; i < currentTotal; i++)
        {
            GameObject oneServer = serverObjQueue.Dequeue();
            allMachines.Add(oneServer);
            serverObjQueue.Enqueue(oneServer);
        }
        currentTotal = switchObjQueue.Count;
        for (int i = 0; i < currentTotal; i++)
        {
            GameObject oneSwitch = switchObjQueue.Dequeue();
            allMachines.Add(oneSwitch);
            switchObjQueue.Enqueue(oneSwitch);
        }
    }

    public void writeAllNetworks()
    {
        List<string> Lines = new List<string>();
        string path = "";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"..\\..\\Automate\\Host_Scripts\\all_ips.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"..\\..\\..\\Automate\\Host_Scripts\\all_ips.txt";
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            foreach (string thisIp in possibleNetworks)
            {
                string newIP = thisIp.Replace("Y", "0");
                Lines.Add(newIP);
            }
            foreach (string line in Lines)
            {
                file.Write(line);
                file.Write("\n");
            }
        }
    }

    public void writeAllMachines()
    {
        List<string> Lines = new List<string>();
        string path = "";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"..\\..\\Automate\\Host_Scripts\\all_machines.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"..\\..\\..\\Automate\\Host_Scripts\\all_machines.txt";
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            foreach (GameObject machine in allMachines)
            {
                Lines.Add(machine.name);
            }
            foreach (string line in Lines)
            {
                file.Write(line);
                file.Write("\n");
            }
        }
    }

    public void writeDetailedIPInfo()
    {
        List<string> Lines = new List<string>();
        string path = "";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"..\\..\\Automate\\Host_Scripts\\ip_info.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"..\\..\\..\\Automate\\Host_Scripts\\ip_info.txt";
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            foreach (GameObject thisMachine in allMachines)
            {
                for (int index = 0; index < thisMachine.GetComponent<drag_and_drop>().connections.Count; index++)
                {
                    string ipConnection = thisMachine.GetComponent<drag_and_drop>().ip[index];
                    string ethConnection = thisMachine.GetComponent<drag_and_drop>().eth[index];
                    Lines.Add(thisMachine.name + " " + ipConnection + " " + ethConnection);
                }

            }
            foreach (string line in Lines)
            {
                file.Write(line);
                file.Write("\n");
            }
        }
    }

    public void writeDetailedMachineInfo()
    {
        List<string> Lines = new List<string>();
        string path = "";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"..\\..\\Automate\\Host_Scripts\\machine_info.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"..\\..\\..\\Automate\\Host_Scripts\\machine_info.txt";
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            foreach (GameObject thisMachine in allMachines)
            {
                string machineName = thisMachine.name;
                string OS = thisMachine.GetComponent<drag_and_drop>().OS;
                string OSversion = thisMachine.GetComponent<drag_and_drop>().OSversion;
                string memory = thisMachine.GetComponent<drag_and_drop>().memory.ToString() +"MB";
                string type;
                if (thisMachine.tag == "Server") { type = "server"; }
                else if (thisMachine.tag == "Switch") { type = "switch"; }
                else { type = "router"; }
                string status;
                if (thisMachine.GetComponent<drag_and_drop>().machineSetup == 1) { status = "True"; }
                else { status = "False"; }
                status = type + "=" + status;

                string protocols = "";
                if (thisMachine.tag == "Server") {
                    for (int index = 0; index < optServer_controller.ServicesOptionsList.Count ; index++)
                    {
                        protocols = protocols + " " + optServer_controller.ServicesOptionsList[index] + "=" + thisMachine.GetComponent<drag_and_drop>().services[index].ToString();
                    }
                }
                else if (thisMachine.tag == "Switch") 
                {
                    for (int index = 0; index < optSwitch_controller.ServicesOptionsList.Count; index++)
                    {
                        protocols = protocols + " " + optSwitch_controller.ServicesOptionsList[index] + "=" + thisMachine.GetComponent<drag_and_drop>().services[index].ToString();
                    }
                }
                else 
                {
                    {
                        for (int index = 0; index < optRouter_controller.ServicesOptionsList.Count; index++)
                        {
                            protocols = protocols + " " + optRouter_controller.ServicesOptionsList[index] + "=" + thisMachine.GetComponent<drag_and_drop>().services[index].ToString();
                        }
                    }
                }
                Lines.Add(machineName + " " + OS + " " + OSversion + " " + memory + " " + status + " " + protocols);
            }
            foreach (string line in Lines)
            {
                file.Write(line);
                file.Write("\n");
            }
        }
    }

    public void writePairConnections()
    {
        List<string> Lines = new List<string>();
        string path = "";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"C:..\\..\\Automate\\Host_Scripts\\connections.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"C:..\\..\\Automate\\Host_Scripts\\connections.txt";
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            foreach (var connection in connectionsObjList)
            {
                Lines.Add(connection.Item1.name + " " + connection.Item2.name);
            }
            foreach (string line in Lines)
            {
                file.Write(line);
                file.Write("\n");
            }
        }
    }


    public void writePairConnectionsDetailed()
    {
        List<string> Lines = new List<string>();
        string path = "";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"..\\..\\Automate\\Host_Scripts\\connections_detailed.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"..\\..\\..\\Automate\\Host_Scripts\\connections_detailed.txt";
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            foreach (var connection in connectionsObjList)
            {
                int index1 = connection.Item1.GetComponent<drag_and_drop>().connections.IndexOf(connection.Item2);
                int index2 = connection.Item2.GetComponent<drag_and_drop>().connections.IndexOf(connection.Item1);
                string IP1 = connection.Item1.GetComponent<drag_and_drop>().ip[index1];
                string IP2 = connection.Item2.GetComponent<drag_and_drop>().ip[index2];
                string eth1 = connection.Item1.GetComponent<drag_and_drop>().eth[index1];
                string eth2 = connection.Item2.GetComponent<drag_and_drop>().eth[index2];
                writeCorrectIPInterface(connection.Item1, index1, IP1);
                writeCorrectIPInterface(connection.Item2, index2, IP2);
                Lines.Add(connection.Item1.name + " " +IP1 + " " + eth1 + " " + connection.Item2.name + " " + IP2 + " " + eth2);
            }
            foreach (string line in Lines)
            {
                file.Write(line);
                file.Write("\n");

            }
        }
    }


    public void createConnectionFile()
    {
        getAllMachines();
        fixIP();
        cleanIPfromSwitchs();
        changeDefaultIPIdentificator();
        writeAllMachines();
        writeAllNetworks();
        writeDetailedIPInfo();
        writeDetailedMachineInfo();
        writePairConnectionsDetailed();
        //StopButton.SetActive(true);
        //StartButton.SetActive(false);
    }



    IEnumerator readNameFile()
    {
        string line;
        yield return new WaitForSeconds(5);
        string path="";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"..\\..\\Automate\\Host_Scripts\\locker.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"..\\..\\..\\Automate\\Host_Scripts\\locker.txt";
        }
        using (StreamReader reader = new StreamReader(path)) { line = reader.ReadLine(); }
        while(line == "closed")
        {
            yield return new WaitForSeconds(2);
            using (StreamReader reader = new StreamReader(path)) { line = reader.ReadLine(); }
        }
        //GameView
        //path = @"..\\..\\Automate\\Host_Scripts\\allMachinesNames.txt";
        //GameBuild
        //path = @"..\\..\\..\\Automate\\Host_Scripts\\allMachinesNames.txt";
        //using (StreamReader name_file = new StreamReader(path))
        //{
        //    while ((line = name_file.ReadLine()) != null)
        //    {
        //        if (line.Contains("server"))
        //        {
        //            var thisServer = serverObjQueue.Dequeue();
        //            Text thisServerName = thisServer.GetComponentInChildren<Text>();
        //            thisServer.name = line;
        //            thisServerName.text = line;
        //            serverObjQueue.Enqueue(thisServer);
        //        }
        //        else if (line.Contains("switch"))
        //        {
        //            var thisSwitch = switchObjQueue.Dequeue();
        //            Text thisSwitchrName = thisSwitch.GetComponentInChildren<Text>();
        //            thisSwitch.name = line;
        //            thisSwitchrName.text = line;
        //            switchObjQueue.Enqueue(thisSwitch);
        //        }
        //        else if (line.Contains("router"))
        //        {
        //            var thisRouter = routerObjQueue.Dequeue();
        //            Text thisRouterName = thisRouter.GetComponentInChildren<Text>();
        //            thisRouter.name = line;
        //            thisRouterName.text = line;
        //            routerObjQueue.Enqueue(thisRouter);
        //        }
        //
        //    }
        //}
    }
}
