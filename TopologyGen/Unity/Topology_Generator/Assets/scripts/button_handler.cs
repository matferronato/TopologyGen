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
        Debug.Log(serverObjQueue.Count);


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
        string strCmdText;
        strCmdText = "/C ..\\..\\Windows\\bash.exe /mnt/c/Users/matheus_ferronato/MyProjects/TCC/TopologyGen/TopologyGen/create.run --l0 " + totalServerNumber.ToString() + " --l1 " + totalSwitchNumber.ToString() + " --l2 " + totalRouterNumber.ToString();
        System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        startRunning = true;
        StartCoroutine(readNameFile());
        StopButton.SetActive(true);
        StartButton.SetActive(false);
    }

    public void OnClickStop()
    {
        startRunning = false;
        StopButton.SetActive(false);
        StartButton.SetActive(true);
        totalServerNumber = 0;
        totalSwitchNumber = 0;
        totalRouterNumber = 0;
        int currentTotal = serverObjQueue.Count;
        for ( int i = 0; i < currentTotal; i++) { Destroy(serverObjQueue.Dequeue()); }
        currentTotal = switchObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(switchObjQueue.Dequeue()); }
        currentTotal = routerObjQueue.Count;
        for (int i = 0; i < currentTotal; i++) { Destroy(routerObjQueue.Dequeue()); }
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
                    thisServerName.text = line;
                    serverObjQueue.Enqueue(thisServer);
                }
                else if (line.Contains("switch"))
                {
                    var thisSwitch = switchObjQueue.Dequeue();
                    Text thisSwitchrName = thisSwitch.GetComponentInChildren<Text>();
                    thisSwitchrName.text = line;
                    switchObjQueue.Enqueue(thisSwitch);
                }
                else if (line.Contains("router"))
                {
                    var thisRouter = routerObjQueue.Dequeue();
                    Text thisRouterName = thisRouter.GetComponentInChildren<Text>();
                    thisRouterName.text = line;
                    routerObjQueue.Enqueue(thisRouter);
                }

            }
        }
    }
}
