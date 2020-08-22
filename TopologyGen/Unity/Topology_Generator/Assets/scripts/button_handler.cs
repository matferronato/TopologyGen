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


    // Start is called before the first frame update
    void Start()
    {
        totalServerNumber = 0;
        totalSwitchNumber = 0;
        totalRouterNumber = 0;
        allowLines = false;
        startRunning = false;
        StartButton.SetActive(true);
        StopButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickServer()
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
        var thisServerIcon = Instantiate(ServerButton, new Vector2(Position2d.transform.position.x, Position2d.transform.position.y), Quaternion.identity);
        thisServerIcon.transform.parent = gameObject.transform;
        serverObjQueue.Enqueue(thisServerIcon);
    }

    public void OnClickSwitch()
    {
        if (magnetic_dop.deletedSwitch == true) {
            magnetic_dop.deletedSwitch = false;
        }
        else
        {
            switchNameQueue.Enqueue(totalSwitchNumber);
            totalSwitchNumber = totalSwitchNumber + 1;
        }
        var thisSwitchIcon = Instantiate(SwitchButton, new Vector2(Position2d.transform.position.x, Position2d.transform.position.y), Quaternion.identity);
        thisSwitchIcon.transform.parent = gameObject.transform;
        switchObjQueue.Enqueue(thisSwitchIcon);
    }

    public void OnClickRouter()
    {
        if (magnetic_dop.deletedRouter== true)
        {
            magnetic_dop.deletedRouter = false;
        }
        else
        {
            routerNameQueue.Enqueue(totalRouterNumber);
            totalRouterNumber = totalRouterNumber + 1;
        }
        var thisRouterIcon = Instantiate(RouterButton, new Vector2(Position2d.transform.position.x, Position2d.transform.position.y), Quaternion.identity);
        thisRouterIcon.transform.parent = gameObject.transform;
        routerObjQueue.Enqueue(thisRouterIcon);
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
        //else
        //{
        //    int currentTotal = serverObjQueue.Count;
        //    for (int i = 0; i < currentTotal; i++) //para todos os servidores
        //    {
        //        var thisServer = serverObjQueue.Dequeue();//para cada conexao no servidor
        //        for (int j = 0; j < thisServer.GetComponent<drag_and_drop>().connections.Count; j++)
        //        {
        //            if (thisServer.GetComponent<drag_and_drop>().ip[j] == "0.0.0.0") //se ainda n tenho ip
        //            {
        //                GameObject aux = thisServer.GetComponent<drag_and_drop>().connections[j];
        //                GameObject greatAux = aux;
        //                int l = 0;
        //                while (thisServer.GetComponent<drag_and_drop>().ip[j] == "0.0.0.0") //enquanto minha conexao for 0
        //                {
        //                    for (int k = 0; k < aux.GetComponent<drag_and_drop>().connections.Count; k++) //para cada conexão conectada a mim
        //                    {
        //                        if(aux.GetComponent<drag_and_drop>().ip[k] != "0.0.0.0") //se conexao tem ip
        //                        {
        //                            thisServer.GetComponent<drag_and_drop>().ip[j] = aux.GetComponent<drag_and_drop>().ip[k];
        //                            break;
        //                        }
        //                    }
        //                    if(l < aux.GetComponent<drag_and_drop>().connections.Count)
        //                    {
        //                        aux = greatAux.GetComponent<drag_and_drop>().connections[0];
        //                    } else
        //                    {
        //                        break;
        //                    }
        //                    
        //                }
        //            }
        //        }
        //        serverObjQueue.Enqueue(thisServer);
        //    }
        //}
    }

    public void createConnectionFile()
    {
        fixIP();
        cleanIPfromSwitchs();
        List<string> Lines = new List<string>();
        foreach (var connection in connectionsObjList)
        {
            //Lines.Add(connection.Item1.name + "-" + connection.Item2.name);
            int index = connection.Item1.GetComponent<drag_and_drop>().connections.IndexOf(connection.Item2);
            string IP1 = connection.Item1.GetComponent<drag_and_drop>().ip[index];
            index = connection.Item2.GetComponent<drag_and_drop>().connections.IndexOf(connection.Item1);
            string IP2 = connection.Item2.GetComponent<drag_and_drop>().ip[index];
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

    public void OnClickLines()
    {
        if (allowLines == true) { allowLines = false; } else { allowLines = true; }
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
