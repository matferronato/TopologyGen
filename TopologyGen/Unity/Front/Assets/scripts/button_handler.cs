using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class button_handler : MonoBehaviour
{
    public GameObject getLine;
    static GameObject defaultLine;
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
    public static int totalMachines;
    public bool createServer;
    public bool createSwitch;
    public bool createRouter;

    public static bool allowLines;
    public static bool startRunning;
    public static bool waitingToRunning;
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
        else if (x == 1920 && y == 1080)
        {
            MenuPositionX = 1330.0f;
            upperBoundary = 1070.0f;
            bottomBoundary = 64.0f;
            leftBoundary = 74.0f;
            rightBoundary = 1919.0f;
        }
    }

    void Start()
    {
        setScreenResolutionValues();
        totalServerNumber = 0;
        totalSwitchNumber = 0;
        totalRouterNumber = 0;
        totalMachines = 0;
        allowLines = false;
        startRunning = false;
        waitingToRunning = false;
        optionRunning = false;
        createServer = false;
        createSwitch = false;
        createRouter = false;
        StartButton.SetActive(true);
        StopButton.SetActive(false);
        defaultLine = getLine;
    }

    // Update is called once per frame
    void Update()
    {
        totalMachines = totalServerNumber + totalSwitchNumber + totalRouterNumber;

        checkIfKeyPressed();
        if (Input.mousePosition.x < MenuPositionX)
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

    public static void ExecuteCommand(string command)
    {
        Process proc = new System.Diagnostics.Process ();
        proc.StartInfo.FileName = "/bin/bash";
        proc.StartInfo.Arguments = "-c \" " + command + " \"";
        proc.StartInfo.UseShellExecute = false;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.Start ();

        while (!proc.StandardOutput.EndOfStream) {
            Console.WriteLine (proc.StandardOutput.ReadLine ());
        }
    }

    public void OnClickRun()
    {
        if(totalMachines > 0)
        {
            allMachines.Clear();
            possibleNetworks.Clear();
            setupTopologyFiles();
            string strCmdText ="";
            //GameView
            if (menu_controller.OnGameRunning == false)
            {
              ExecuteCommand("gnome-terminal -x bash -ic '../../create.run; bash'");
            }
            //GameBuild
            if (menu_controller.OnGameRunning == true)
            {
              ExecuteCommand("gnome-terminal -x bash -ic '../../../create.run; bash'");
            }
            StartCoroutine(readNameFile());
            StopButton.SetActive(true);
            StartButton.SetActive(false);
        }
    }

    public void OnClickStop()
    {
        startRunning = false;
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            ExecuteCommand("gnome-terminal -x bash -ic ' cd ../../Automate/ ; vagrant destroy -f; bash'");
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            ExecuteCommand("gnome-terminal -x bash -ic 'cd ../../../Automate/ ; vagrant destroy -f; bash'");
        }
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
        address_manager.reset();

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
            path = @"../../TopologyInfo/topology.dot";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../TopologyInfo/topology.dot";
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
                currentStringCreation = currentStringCreation.Replace("%", currentName).Replace("!", currentOS).Replace("@", currentOSversion).Replace("(", (Int16.Parse(currentMemory)*500).ToString());
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
        if(address_manager.currentValue != 1){
          for(int i = 1; i < address_manager.currentValue; i++)
          {
              string currentIP = defaultIP.Replace("X", i.ToString());
              possibleNetworks.Add(currentIP);
          }
        } else{
          string currentIP = defaultIP.Replace("X", "1".ToString());
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
            path = @"../../Automate/Host_Scripts/all_ips.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../Automate/Host_Scripts/all_ips.txt";
        }
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            if(possibleNetworks.Count > 0){
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
            else {
              file.Write("1.1.1.0/24");
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
            path = @"../../Automate/Host_Scripts/all_machines.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../Automate/Host_Scripts/all_machines.txt";
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
            path = @"../../Automate/Host_Scripts/ip_info.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../Automate/Host_Scripts/ip_info.txt";
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
            path = @"../../Automate/Host_Scripts/machine_info.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../Automate/Host_Scripts/machine_info.txt";
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
            path = @"../../Automate/Host_Scripts/Save/ZZZAll_Connections.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @".././../../Automate/Host_Scripts/Save/ZZZAll_Connections.txt";
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
            path = @"../../Automate/Host_Scripts/connections_detailed.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../Automate/Host_Scripts/connections_detailed.txt";
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
    }


    IEnumerator readNameFile()
    {
        waitingToRunning = true;
        string line;
        yield return new WaitForSeconds(5);
        string path="";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"../../Automate/Host_Scripts/locker.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../Automate/Host_Scripts/locker.txt";
        }
        using (StreamReader reader = new StreamReader(path)) { line = reader.ReadLine(); }
        while(line == "closed")
        {
            yield return new WaitForSeconds(2);
            using (StreamReader reader = new StreamReader(path)) { line = reader.ReadLine(); }
        }
        waitingToRunning = false;
        startRunning = true;
    }

    public void RunOsMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void saveState()
    {
      string path = "";
      //GameView
      if (menu_controller.OnGameRunning == false)
      {
          path = @"../../Automate/Host_Scripts/Save/";
      }
      //GameBuild
      if (menu_controller.OnGameRunning == true)
      {
          path = @"../../../Automate/Host_Scripts/Save/";
      }
      System.IO.DirectoryInfo dir = new DirectoryInfo(path);
      foreach (FileInfo file in dir.GetFiles())
      {
        file.Delete();
      }
      writePairConnections();
      getAllMachines();
      foreach (GameObject machine in allMachines)
      {
        List<string> Lines = new List<string>();
        string this_name = machine.name;
        float posX = machine.transform.position.x;
        float posY = machine.transform.position.y;
        float posZ = machine.transform.position.z;
        float mousePosX = machine.GetComponent<drag_and_drop>().position.Item1;
        float mousePosY = machine.GetComponent<drag_and_drop>().position.Item2;
        bool this_linked = machine.GetComponent<drag_and_drop>().linked;
        List<string> this_ip = machine.GetComponent<drag_and_drop>().ip;
        List<string> this_eth = machine.GetComponent<drag_and_drop>().eth;
        string this_OS = machine.GetComponent<drag_and_drop>().OS;
        string this_OSVersion = machine.GetComponent<drag_and_drop>().OSversion;
        int this_memory = machine.GetComponent<drag_and_drop>().memory;
        int this_machineSetup = machine.GetComponent<drag_and_drop>().machineSetup;
        int this_serverConnectedNumber= machine.GetComponent<drag_and_drop>().serverConnectedNumber;
        int this_switchConnectedNumber = machine.GetComponent<drag_and_drop>().switchConnectedNumber;
        int this_routerConnectedNumber = machine.GetComponent<drag_and_drop>().routerConnectedNumber;
        List<bool> this_services = machine.GetComponent<drag_and_drop>().services;
        Tuple<float, float> this_position = machine.GetComponent<drag_and_drop>().position;
        List<GameObject> this_connections = machine.GetComponent<drag_and_drop>().connections;
        List<GameObject> this_attatchedText = machine.GetComponent<drag_and_drop>().attatchedText;
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path+ this_name +".txt"))
        {
          Lines.Add("name"); Lines.Add(this_name);Lines.Add("####################################");
          Lines.Add("linked");if (this_linked == true){Lines.Add("true");} else{Lines.Add("false");}Lines.Add("####################################");
          Lines.Add("ip");
          foreach (string ip in this_ip)
          {
            Lines.Add(ip);
          }
          Lines.Add("####################################");
          Lines.Add("eth");
          foreach (string eth in this_eth)
          {
            Lines.Add(eth);
          }
          Lines.Add("####################################");
          Lines.Add("OS"); Lines.Add(this_OS);Lines.Add("####################################");
          Lines.Add("OSVersion"); Lines.Add(this_OSVersion);Lines.Add("####################################");
          Lines.Add("Memory"); Lines.Add(this_memory.ToString());Lines.Add("####################################");
          Lines.Add("machineSetup"); Lines.Add(this_machineSetup.ToString());Lines.Add("####################################");
          Lines.Add("serverConnectedNumber"); Lines.Add(this_serverConnectedNumber.ToString());Lines.Add("####################################");
          Lines.Add("switchConnectedNumber"); Lines.Add(this_switchConnectedNumber.ToString());Lines.Add("####################################");
          Lines.Add("routerConnectedNumber"); Lines.Add(this_routerConnectedNumber.ToString());Lines.Add("####################################");
          Lines.Add("services");
          foreach (bool eachService in this_services)
          {
            if(eachService == true){Lines.Add("true");} else{Lines.Add("false");}
          }
          Lines.Add("####################################");
          Lines.Add("pos");
          Lines.Add(posX.ToString());Lines.Add(posY.ToString());Lines.Add(posZ.ToString());
          Lines.Add("####################################");
          Lines.Add("mousePos");
          Lines.Add(mousePosX.ToString());Lines.Add(mousePosY.ToString());
          Lines.Add("####################################");
          foreach (string line in Lines)
          {
              file.Write(line);
              file.Write("\n");
          }
        }
      }
      allMachines.Clear();
    }

    public void loadState()
    {
      clearAll();
      string path = "";
      //GameView
      if (menu_controller.OnGameRunning == false)
      {
          path = @"../../Automate/Host_Scripts/Save/";
      }
      //GameBuild
      if (menu_controller.OnGameRunning == true)
      {
          path = @"../../../Automate/Host_Scripts/Save/";
      }
      DirectoryInfo dir = new DirectoryInfo(path);
      FileInfo[] Files = dir.GetFiles("*.txt");
      string str = "";
      string line = "";
      foreach(FileInfo file in Files )
      {
        GameObject thisObj;
        if(file.Name.Contains("ZZZAll_Connections")){
          foreach (string reader in System.IO.File.ReadAllLines(path+file.Name)){
              string[] stringList = reader.Split(null);
              string machine_A = stringList[0];
              string machine_B = stringList[1];
              GameObject objA = GameObject.Find(machine_A);
              GameObject objB = GameObject.Find(machine_B);
              Tuple<GameObject, GameObject> thisConnection = new Tuple<GameObject, GameObject>(objA, objB);
              float mousePosAx = objA.GetComponent<drag_and_drop>().position.Item1;
              float mousePosAy = objA.GetComponent<drag_and_drop>().position.Item2;
              float mousePosBx = objB.GetComponent<drag_and_drop>().position.Item1;
              float mousePosBy = objB.GetComponent<drag_and_drop>().position.Item2;
              line_creation.createTextInterface(objA, new Vector3(mousePosAx, mousePosAy, 0f), objB, new Vector3(mousePosBx, mousePosBy, 0f));
              line_creation.setupConnections(objA, objB);
              var thisLine = Instantiate(defaultLine, new Vector3(0f, 0f, 0f), Quaternion.identity);
              thisLine.name = "Line_" + objA.name + "_" + objB.name;
              var linkedline = thisLine.GetComponent<LineRenderer>();
              linkedline.SetPosition(0, new Vector3(mousePosAx, mousePosAy, 0f));
              linkedline.SetPosition(1, new Vector3(mousePosBx, mousePosBy, 0f));
              lineObjList.Add(thisLine);
              connectionsObjList.Add(thisConnection);
          }
          continue;
        }
        else if(file.Name.Contains("Server")){
          if (magnetic_dop.deletedServer == true)
          {
              magnetic_dop.deletedServer = false;
          }
          else
          {
              serverNameQueue.Enqueue(totalServerNumber);
              totalServerNumber = totalServerNumber + 1;
          }
          thisObj = Instantiate(ServerButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
          thisObj.transform.parent = gameObject.transform;
        }
        else if(file.Name.Contains("Switch")){
          if (magnetic_dop.deletedSwitch == true)
          {
              magnetic_dop.deletedSwitch = false;
          }
          else
          {
              switchNameQueue.Enqueue(totalSwitchNumber);
              totalSwitchNumber = totalSwitchNumber + 1;
          }
          thisObj = Instantiate(SwitchButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
          thisObj.transform.parent = gameObject.transform;
        }
        else {
          if (magnetic_dop.deletedRouter == true)
          {
              magnetic_dop.deletedRouter = false;
          }
          else
          {
              routerNameQueue.Enqueue(totalRouterNumber);
              totalRouterNumber = totalRouterNumber + 1;
          }
          thisObj = Instantiate(RouterButton, new Vector2(0.0f, 0.0f), Quaternion.identity);
          thisObj.transform.parent = gameObject.transform;
        }
        string aux = "";
        using (StreamReader reader = new StreamReader(path+file.Name)){
          aux = reader.ReadLine(); //read name
          thisObj.name = reader.ReadLine(); //read name value
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and linked
          UnityEngine.Debug.Log(thisObj.GetComponent<drag_and_drop>().linked);
          if(reader.ReadLine() == "true"){ thisObj.GetComponent<drag_and_drop>().linked = true;} //read linked value}
          else{thisObj.GetComponent<drag_and_drop>().linked = false;}
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and ip
          while((line = reader.ReadLine()) != null && (line != "####################################"))
          {
            thisObj.GetComponent<drag_and_drop>().ip.Add(line);
          }
          aux = reader.ReadLine();
          while((line = reader.ReadLine()) != null && (line != "####################################"))
          {
            thisObj.GetComponent<drag_and_drop>().eth.Add(line);
          }
          aux = reader.ReadLine(); //read OS
          thisObj.GetComponent<drag_and_drop>().OS = reader.ReadLine();//read OS value
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and OSversion
          thisObj.GetComponent<drag_and_drop>().OSversion = reader.ReadLine();//read OS versio value
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and memory
          thisObj.GetComponent<drag_and_drop>().memory = Int16.Parse(reader.ReadLine());
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and machineSetup
          thisObj.GetComponent<drag_and_drop>().machineSetup = Int16.Parse(reader.ReadLine());
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and serverConnectedNumber
          thisObj.GetComponent<drag_and_drop>().serverConnectedNumber = Int16.Parse(reader.ReadLine());
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and routerConnectedNumber
          thisObj.GetComponent<drag_and_drop>().routerConnectedNumber = Int16.Parse(reader.ReadLine());
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and switchConnectedNumber
          thisObj.GetComponent<drag_and_drop>().switchConnectedNumber = Int16.Parse(reader.ReadLine());
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and services
          int i = 0;
          while((line = reader.ReadLine()) != null && (line != "####################################"))
          {
            if(line == "true") {thisObj.GetComponent<drag_and_drop>().services[i] = true;}
            else {thisObj.GetComponent<drag_and_drop>().services[i] = false;}
            i = i+1;
          }
          aux = reader.ReadLine();
          float posX =  float.Parse(reader.ReadLine());
          float posY =  float.Parse(reader.ReadLine());
          float posZ =  float.Parse(reader.ReadLine());
          thisObj.transform.position = new Vector3(posX,posY,posZ);
          aux = reader.ReadLine(); aux = reader.ReadLine(); //read separator and mousePos
          float mousePosX =  float.Parse(reader.ReadLine());
          float mousePosY =  float.Parse(reader.ReadLine());
          thisObj.GetComponent<drag_and_drop>().position = Tuple.Create(mousePosX, mousePosY);
          if(file.Name.Contains("Server")){serverObjQueue.Enqueue(thisObj);}
          if(file.Name.Contains("Switch")){switchObjQueue.Enqueue(thisObj);}
          if(file.Name.Contains("Router")){routerObjQueue.Enqueue(thisObj);}
          UnityEngine.Debug.Log(thisObj.GetComponent<drag_and_drop>().linked);
        }
      }
    }


}
