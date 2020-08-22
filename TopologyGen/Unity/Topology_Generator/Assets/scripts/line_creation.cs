using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System.Linq;

public class line_creation : MonoBehaviour
{
    public GameObject getLine;
    static GameObject defaultLine;
    public static GameObject endPointA;
    public static GameObject endPointB;
    public static float Zvalue;
    public float publicZValue;
    private static Vector2 mousePos;
    private static Vector2 startMousePos;


    static bool objA;

    // Start is called before the first frame update
    void Start()
    {
        objA = true;
        defaultLine = getLine;
    }

    // Update is called once per frame
    void Update()
    {
        Zvalue = publicZValue;
    }

    public static bool IsEmpty<T>(List<T> list)
    {
        if (list == null) { return true; }
        return !list.Any();
    }

    public static void generateLine(GameObject otherGameObject)
    {
        if( objA == true)
        {
            endPointA = otherGameObject;
            objA = false;
            startMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Zvalue));
        }
        else
        {
            endPointB = otherGameObject;
            endPointA.GetComponent<drag_and_drop>().selected = false;
            endPointB.GetComponent<drag_and_drop>().selected = false;
            endPointA.GetComponent<drag_and_drop>().linked = true;
            endPointB.GetComponent<drag_and_drop>().linked = true;
            endPointA.GetComponent<CanvasGroup>().alpha = 1f;
            endPointB.GetComponent<CanvasGroup>().alpha = 1f;

            if (endPointA == endPointB) { objA = true;  return; }

            if (endPointB.name.Contains("Router"))
            {
                GameObject aux = endPointB;
                endPointB = endPointA;
                endPointA = aux;
            }
            if (endPointA.name.Contains("Server") && endPointB.name.Contains("Switch"))
            {
                GameObject aux = endPointB;
                endPointB = endPointA;
                endPointA = aux;
            }

            Tuple<GameObject, GameObject> thisConnection = new Tuple<GameObject, GameObject>(endPointA, endPointB);
            if (!button_handler.connectionsObjList.Contains(thisConnection) && !button_handler.connectionsObjList.Contains(new Tuple<GameObject, GameObject>(thisConnection.Item2, thisConnection.Item1))) {
                button_handler.connectionsObjList.Add(thisConnection);

                mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Zvalue));
                var thisLine = Instantiate(defaultLine, new Vector3(0f, 0f, 0f), Quaternion.identity);
                var line = thisLine.GetComponent<LineRenderer>();
                line.SetPosition(0, new Vector3(startMousePos.x, startMousePos.y, 0f));
                line.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));
                button_handler.lineObjQueue.Enqueue(thisLine);

                setupConnections(endPointA, endPointB);
            }

            objA = true;
        }
    }

    public static void checkIfNullIp(string IP, GameObject objectB)
    {
        if (!objectB.GetComponent<drag_and_drop>().ip.Contains("0.0.0.0")) { return; }
        GameObject thisMachine = objectB;
        for (int i = 0; i < thisMachine.GetComponent<drag_and_drop>().ip.Count; i++)
        {
            if(thisMachine.GetComponent<drag_and_drop>().ip[i] == "0.0.0.0")
                {
                    thisMachine.GetComponent<drag_and_drop>().ip[i] = IP;
                    checkIfNullIp(IP, thisMachine.GetComponent<drag_and_drop>().connections[i]);
                }    
        }
    }

    public static bool listDoesNotOnlyContain<T>(ref List<T> list, ref T value)
    {
        bool isEmptyA = IsEmpty(list);
        if (isEmptyA) { return false; }
            for (int i = 0; i < list.Count; i++)
        {
            if(list[i].Equals(value))
            {
                return false;
            }
        }
        return true;
    }

    public static string returnAnyValueExcept(List<string> list, string value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].Equals(value))
            {
                return list[i];
            }
        }
        return "not_found";
    }

    public static void setupConnections(GameObject objectA, GameObject objectB)
    {
        string IP;
        string nullIp = "0.0.0.0";
        if (objectA.name.Contains("Router"))
        {
            objectA.GetComponent<drag_and_drop>().connections.Add(objectB);
            objectB.GetComponent<drag_and_drop>().connections.Add(objectA);
            IP = address_manager.provideAddress();
            objectA.GetComponent<drag_and_drop>().ip.Add(IP);
            objectB.GetComponent<drag_and_drop>().ip.Add(IP);
            checkIfNullIp(IP, objectB);
        }
        else if (objectA.name.Contains("Switch"))
        {
            int index = objectA.GetComponent<drag_and_drop>().connections.FindIndex(a => a.name.Contains("Router"));
            objectA.GetComponent<drag_and_drop>().connections.Add(objectB);
            objectB.GetComponent<drag_and_drop>().connections.Add(objectA);
            if (index >= 0)
            {
                objectA.GetComponent<drag_and_drop>().ip.Add(objectA.GetComponent<drag_and_drop>().ip[index]);
                objectB.GetComponent<drag_and_drop>().ip.Add(objectA.GetComponent<drag_and_drop>().ip[index]);
                checkIfNullIp(objectA.GetComponent<drag_and_drop>().ip[index], objectB);
            }
            else if (listDoesNotOnlyContain(ref objectA.GetComponent<drag_and_drop>().ip, ref nullIp))
            {
                IP = returnAnyValueExcept(objectA.GetComponent<drag_and_drop>().ip, nullIp);
                objectA.GetComponent<drag_and_drop>().ip.Add(IP);
                objectB.GetComponent<drag_and_drop>().ip.Add(IP);
                checkIfNullIp(IP, objectB);
            }
            else if (listDoesNotOnlyContain(ref objectB.GetComponent<drag_and_drop>().ip, ref nullIp))
            {
                IP = returnAnyValueExcept(objectB.GetComponent<drag_and_drop>().ip, nullIp);
                objectA.GetComponent<drag_and_drop>().ip.Add(IP);
                objectB.GetComponent<drag_and_drop>().ip.Add(IP);
                checkIfNullIp(IP, objectA);
            }
            else
            {
                objectA.GetComponent<drag_and_drop>().ip.Add("0.0.0.0");
                objectB.GetComponent<drag_and_drop>().ip.Add("0.0.0.0");
            }
        }
        else if (objectA.name.Contains("Server"))
        {
            objectA.GetComponent<drag_and_drop>().connections.Add(objectB);
            objectB.GetComponent<drag_and_drop>().connections.Add(objectA);
            if (listDoesNotOnlyContain(ref objectA.GetComponent<drag_and_drop>().ip, ref nullIp))
            {
                IP = returnAnyValueExcept(objectA.GetComponent<drag_and_drop>().ip, nullIp);
                objectA.GetComponent<drag_and_drop>().ip.Add(IP);
                objectB.GetComponent<drag_and_drop>().ip.Add(IP);
                checkIfNullIp(IP, objectB);
            }
            else if (listDoesNotOnlyContain(ref objectB.GetComponent<drag_and_drop>().ip, ref nullIp))
            {
                IP = returnAnyValueExcept(objectB.GetComponent<drag_and_drop>().ip, nullIp);
                objectA.GetComponent<drag_and_drop>().ip.Add(IP);
                objectB.GetComponent<drag_and_drop>().ip.Add(IP);
                checkIfNullIp(IP, objectA);
            } else
            {
                objectA.GetComponent<drag_and_drop>().ip.Add("0.0.0.0");
                objectB.GetComponent<drag_and_drop>().ip.Add("0.0.0.0");
            }
        }
    }

}
