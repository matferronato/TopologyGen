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
    public static Queue<GameObject> lineObjQueue = new Queue<GameObject>();
    public static List<Tuple<GameObject, GameObject>> connectionsObjList= new List<Tuple<GameObject, GameObject>>();

    public static Queue serverNameQueue = new Queue();
    public static Queue switchNameQueue = new Queue();
    public static Queue routerNameQueue = new Queue();

    public static int totalServerNumber;
    public static int totalSwitchNumber;
    public static int totalRouterNumber;

    public static bool allowLines;
    public static bool startRunning;

    public bool createServer;
    public bool createSwitch;
    public bool createRouter;


    // Start is called before the first frame update
    void Start()
    {
        totalServerNumber = 0;
        totalSwitchNumber = 0;
        totalRouterNumber = 0;
        allowLines = false;
        startRunning = false;
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
        if (Input.GetKeyDown("s"))
        {
            OnClickSwitch();
        }
        else if (Input.GetKeyDown("w"))
        {
            OnClickLines();
        }
        else if (Input.GetKeyDown("r"))
        {
            OnClickRouter();
        }
        else if (Input.GetKeyDown("n"))
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
            if(startRunning == false)
            {
                OnClickRun();
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
            Routerbutton.GetComponent<Image>().color = new Color32(135, 135, 135, 255);
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
        createConnection();
        startRunning = true;
        //string strCmdText;
        //strCmdText = "/C ..\\..\\Windows\\bash.exe /mnt/c/Users/matheus_ferronato/MyProjects/TCC/TopologyGen/TopologyGen/create.run --l0 " + totalServerNumber.ToString() + " --l1 " + totalSwitchNumber.ToString() + " --l2 " + totalRouterNumber.ToString();
        //System.Diagnostics.Process.Start("CMD.exe", strCmdText);
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

    public void createConnection()
    {
        createConnectionFile();
        string path = @".\\test.txt";
        //using (StreamReader name_file = new StreamReader(path))
        //{
        //
        //}
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

    public void createConnectionFile()
    {
        fixIP();
        cleanIPfromSwitchs();
        List<string> Lines = new List<string>();
        foreach (var connection in connectionsObjList)
        {
            int index = connection.Item1.GetComponent<drag_and_drop>().connections.IndexOf(connection.Item2);
            string IP1 = connection.Item1.GetComponent<drag_and_drop>().ip[index];
            writeCorrectIPInterface(connection.Item1, index, IP1);
            index = connection.Item2.GetComponent<drag_and_drop>().connections.IndexOf(connection.Item1);
            string IP2 = connection.Item2.GetComponent<drag_and_drop>().ip[index];
            writeCorrectIPInterface(connection.Item2, index, IP2);
            Lines.Add(connection.Item1.name + "- IP = " + IP1 + " => " + connection.Item2.name + "- IP = " + IP2);
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:.\\test.txt"))
        {
            foreach (string line in Lines)
            {
             file.WriteLine(line);
            }
        }
    }

    public void clearAll()
    {
        totalServerNumber = 0;
        totalSwitchNumber = 0;
        totalRouterNumber = 0;
        connectionsObjList.Clear();
        int currentTotal = serverObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(serverObjQueue.Dequeue()); }
        currentTotal = switchObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(switchObjQueue.Dequeue()); }
        currentTotal = routerObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(routerObjQueue.Dequeue()); }
        currentTotal = lineObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(lineObjQueue.Dequeue()); }
    }

    IEnumerator readNameFile()
    {
        string line;
        yield return new WaitForSeconds(5);
        string path = @"..\\..\\Automate\\Host_Scripts\\locker.txt";
        using (StreamReader reader = new StreamReader(path)) { line = reader.ReadLine(); }
        while(line == "closed")
        {
            yield return new WaitForSeconds(2);
            using (StreamReader reader = new StreamReader(path)) { line = reader.ReadLine(); }
        }
        path = @"..\\..\\Automate\\Host_Scripts\\allMachinesNames.txt";
        using (StreamReader name_file = new StreamReader(path))
        {
            while ((line = name_file.ReadLine()) != null)
            {
                if (line.Contains("server"))
                {
                    var thisServer = serverObjQueue.Dequeue();
                    Text thisServerName = thisServer.GetComponentInChildren<Text>();
                    thisServer.name = line;
                    thisServerName.text = line;
                    serverObjQueue.Enqueue(thisServer);
                }
                else if (line.Contains("switch"))
                {
                    var thisSwitch = switchObjQueue.Dequeue();
                    Text thisSwitchrName = thisSwitch.GetComponentInChildren<Text>();
                    thisSwitch.name = line;
                    thisSwitchrName.text = line;
                    switchObjQueue.Enqueue(thisSwitch);
                }
                else if (line.Contains("router"))
                {
                    var thisRouter = routerObjQueue.Dequeue();
                    Text thisRouterName = thisRouter.GetComponentInChildren<Text>();
                    thisRouter.name = line;
                    thisRouterName.text = line;
                    routerObjQueue.Enqueue(thisRouter);
                }

            }
        }
    }
}
