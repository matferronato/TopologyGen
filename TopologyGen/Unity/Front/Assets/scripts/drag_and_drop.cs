﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Diagnostics;
using System.ComponentModel;

public class drag_and_drop :  MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int myUniquelNumber = 0;

    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public bool selected;
    public bool linked;

    public List<GameObject> connections = new List<GameObject>();
    public List<string> ip = new List<string>();
    public List<string> eth = new List<string>();
    public List<GameObject> attatchedText = new List<GameObject>();
    public string OS;
    public string OSversion;
    public int memory;
    public int machineSetup;
    public List<bool> services = new List<bool>();

    public int serverConnectedNumber;
    public int switchConnectedNumber;
    public int routerConnectedNumber;

    public Tuple<float, float> position = new Tuple<float, float>(0f, 0f);


    private void Awake()
    {
        linked = false;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (gameObject.tag == "Server") {
            myUniquelNumber = (int)button_handler.serverNameQueue.Dequeue();
            Text name = gameObject.GetComponentInChildren<Text>();
            gameObject.name= "Server-" + myUniquelNumber.ToString();
            name.text = " Server-" + myUniquelNumber.ToString();

            OS = optServer_controller.OSOptionsList[0];
            OSversion = optServer_controller.OSversionOptionsList[0];
            machineSetup = 1;
            memory = 1;
            foreach (string service in optServer_controller.ServicesOptionsList)
            {
                services.Add(false);
            }
        }
        else if (gameObject.tag == "Switch") {
            myUniquelNumber = (int)button_handler.switchNameQueue.Dequeue();
            Text name = gameObject.GetComponentInChildren<Text>();
            gameObject.name = "Switch-" + myUniquelNumber.ToString();
            name.text = " Switch-" + myUniquelNumber.ToString();

            OS = optSwitch_controller.OSOptionsList[0];
            OSversion = optSwitch_controller.OSversionOptionsList[0];
            machineSetup = 1;
            memory = 1;
            foreach (string service in optSwitch_controller.ServicesOptionsList) {
                services.Add(false);
            }
        }
        else if (gameObject.tag == "Router") {
            myUniquelNumber = (int)button_handler.routerNameQueue.Dequeue();
            Text name = gameObject.GetComponentInChildren<Text>();
            gameObject.name = "Router-" + myUniquelNumber.ToString();
            name.text = " Router-" + myUniquelNumber.ToString();

            OS = optRouter_controller.OSOptionsList[0];
            OSversion = optRouter_controller.OSversionOptionsList[0];
            machineSetup = 1;
            memory = 1;
            foreach (string service in optRouter_controller.ServicesOptionsList)
            {
                services.Add(false);
            }
        }

    }

    void Start()
    {
        Canvas[] canvas_list = GetComponentsInParent<Canvas>();
        canvas = canvas_list[canvas_list.Length - 1];
        selected = false;
        serverConnectedNumber = 1;
        switchConnectedNumber = 50;
        routerConnectedNumber = 100;

    }

    void Update()
    {
        var viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        //Debug.Log(viewPortPos);
        //if (viewPortPos.y < button_handler.bottomBoundary || viewPortPos.x < button_handler.leftBoundary || viewPortPos.y > button_handler.upperBoundary || viewPortPos.x > button_handler.rightBoundary)
        //if (Input.mousePosition.y < button_handler.bottomBoundary || Input.mousePosition.x < button_handler.leftBoundary || Input.mousePosition.y > button_handler.upperBoundary || Input.mousePosition.x > button_handler.rightBoundary)
        //{
        //    UnityEngine.Object.Destroy(gameObject);
        //}
        if (selected == true)
        {
            canvasGroup.alpha = .2f;
        }


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        if (button_handler.allowLines == false)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        UnityEngine.Debug.Log(linked);
        if (linked == true)
        {
          UnityEngine.Debug.Log("AAA");
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, line_creation.Zvalue));
            for (int i = 0; i < connections.Count; i++)
            {
                GameObject machineConnected = connections[i];
                int index = -1;
                for(int j =0; j < button_handler.connectionsObjList.Count; j++)
                {
                    Tuple<GameObject,GameObject> connection = button_handler.connectionsObjList[j];
                    if (connection.Item1 == gameObject && connection.Item2 == machineConnected)
                    {
                        index = j;
                    }
                }
                if (index < 0)
                {
                    for (int j = 0; j < button_handler.connectionsObjList.Count; j++)
                    {
                        Tuple<GameObject, GameObject> connection = button_handler.connectionsObjList[j];
                        if (connection.Item1 == machineConnected && connection.Item2 == gameObject)
                        {
                            index = j;
                        }
                    }
                }
                position = Tuple.Create(mousePos.x, mousePos.y);
                GameObject currentLine = button_handler.lineObjList[index];
                currentLine.GetComponent<LineRenderer>().SetPosition(0, new Vector3(mousePos.x, mousePos.y, 0f));
                currentLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(machineConnected.GetComponent<drag_and_drop>().position.Item1, machineConnected.GetComponent<drag_and_drop>().position.Item2, 0f));
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1) && button_handler.startRunning == false)
        {
            button_handler.optionRunning = true;
            menu_controller.Menu.SetActive(false);
            menu_controller.Options_Switch.SetActive(false);
            menu_controller.Options_Server.SetActive(false);
            menu_controller.Options_Router.SetActive(false);
            if (this.gameObject.tag == "Switch")
            {
                menu_controller.Options_Switch.SetActive(true);
                optSwitch_controller.setCurrentSwitch(this.gameObject);
            }
            else if (this.gameObject.tag == "Server")
            {
                menu_controller.Options_Server.SetActive(true);
                optServer_controller.setCurrentServer(this.gameObject);
            }
            else if (this.gameObject.tag == "Router")
            {
                menu_controller.Options_Router.SetActive(true);
                optRouter_controller.setCurrentRouter(this.gameObject);
            }
        }
    }

    public void OnClick()
    {
       if( button_handler.startRunning == true )
        {
          if (menu_controller.OnGameRunning == false)
          {
              string turnOnMachine = "gnome-terminal -x bash -ic ' echo " +  gameObject.name + "; cd ../../Automate/ ; vagrant ssh " + gameObject.name + "; bash'";
              ExecuteCommand(turnOnMachine);
          }
          //GameBuild
          if (menu_controller.OnGameRunning == true)
          {
            string turnOnMachine = "gnome-terminal -x bash -ic ' cd ../../../Automate/ ; vagrant ssh " + gameObject.name + "'";
            ExecuteCommand(turnOnMachine);
          }
        }
        if (button_handler.allowLines == true )
        {
            selected = true;
            line_creation.generateLine(this.gameObject);
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

}
