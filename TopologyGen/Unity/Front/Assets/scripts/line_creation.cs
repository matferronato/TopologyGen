using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using System.Linq;

public class line_creation : MonoBehaviour
{
    public GameObject getTextPrefab;
    public GameObject getLine;
    static GameObject defaultLine;
    public static GameObject textPrefab;
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
        textPrefab = getTextPrefab;
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

            Tuple<GameObject, GameObject> thisConnection = new Tuple<GameObject, GameObject>(endPointA, endPointB);
            if (!button_handler.connectionsObjList.Contains(thisConnection) && !button_handler.connectionsObjList.Contains(new Tuple<GameObject, GameObject>(thisConnection.Item2, thisConnection.Item1))) {
                createTextInterface(endPointA, startMousePos, endPointB, mousePos);
                mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Zvalue));
                if (endPointB.name.Contains("Router"))
                {
                    GameObject aux = endPointB;
                    endPointB = endPointA;
                    endPointA = aux;
                    Vector2 vectAux = mousePos;
                    mousePos = startMousePos;
                    startMousePos = vectAux;
                }
                if (endPointA.name.Contains("Server") && endPointB.name.Contains("Switch"))
                {
                    GameObject aux = endPointB;
                    endPointB = endPointA;
                    endPointA = aux;
                    Vector2 vectAux = mousePos;
                    mousePos = startMousePos;
                    startMousePos = vectAux;
                }
                thisConnection = Tuple.Create(endPointA, endPointB);
                button_handler.connectionsObjList.Add(thisConnection);

                var thisLine = Instantiate(defaultLine, new Vector3(0f, 0f, 0f), Quaternion.identity);
                thisLine.name = "Line_" + endPointA.name + "_" + endPointB.name;
                var line = thisLine.GetComponent<LineRenderer>();
                line.SetPosition(0, new Vector3(startMousePos.x, startMousePos.y, 0f));
                line.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));
                button_handler.lineObjList.Add(thisLine);
                endPointA.GetComponent<drag_and_drop>().position = Tuple.Create(startMousePos.x, startMousePos.y);
                endPointB.GetComponent<drag_and_drop>().position = Tuple.Create(mousePos.x, mousePos.y);
                setupConnections(endPointA, endPointB);
            }

            objA = true;
        }
    }

    public static void createTextInterface(GameObject objA, Vector2 posA, GameObject objB, Vector2 posB)
    {
        int deslocXA = 0;
        int deslocYA = 0;
        int deslocXB = 0;
        int deslocYB = 0;
        if (posA.x < posB.x) { deslocXA = 75; deslocXB = -20; }
        if (posA.x > posB.x) { deslocXA = -20; deslocXB = 75; }
        if (posA.y < posB.y) { deslocYA = 0; deslocYB = -90; }
        if (posA.y > posB.y) { deslocYA = -90; deslocYB = 0; }
        
        GameObject tempTextBox = Instantiate(textPrefab, new Vector3(posA.x, posA.y, Zvalue), Quaternion.identity);
        tempTextBox.transform.SetParent(objA.transform, false);
        int thisInterface = objA.GetComponent<drag_and_drop>().attatchedText.Count;
        tempTextBox.name = tempTextBox.transform.parent.name + "_ipText_" + thisInterface.ToString();
        tempTextBox.transform.position = new Vector2(tempTextBox.transform.position.x + deslocXA, tempTextBox.transform.position.y + deslocYA); 
        tempTextBox.GetComponent<Text>().color = new Color32(0,0,0, 255);
        tempTextBox.GetComponent<Text>().text = "IP X.X.X.X\nEthX";
        objA.GetComponent<drag_and_drop>().attatchedText.Add(tempTextBox);
        
        tempTextBox = Instantiate(textPrefab, new Vector3(posB.x, posB.y, Zvalue), Quaternion.identity);
        tempTextBox.transform.SetParent(objB.transform, false);
        thisInterface = objB.GetComponent<drag_and_drop>().attatchedText.Count;
        tempTextBox.name = tempTextBox.transform.parent.name + "_ipText_" + thisInterface.ToString();
        tempTextBox.transform.position = new Vector2(tempTextBox.transform.position.x + deslocXB, tempTextBox.transform.position.y + deslocYB);
        tempTextBox.GetComponent<Text>().color = new Color32(0, 0, 0, 255);
        tempTextBox.GetComponent<Text>().text = "IP X.X.X.X\nEthX";
        objB.GetComponent<drag_and_drop>().attatchedText.Add(tempTextBox);
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

    public static void provideEthInterface(GameObject objA, GameObject objB)
    {
        if (objB.tag == "Server")
        {
            int currentValue = objA.GetComponent<drag_and_drop>().serverConnectedNumber;
            objA.GetComponent<drag_and_drop>().serverConnectedNumber++;
            if (objA.tag == "Switch") { objA.GetComponent<drag_and_drop>().eth.Add("swp" + currentValue.ToString()); }
            else { objA.GetComponent<drag_and_drop>().eth.Add("eth" + currentValue.ToString()); }
        }
        else if (objB.tag == "Switch")
        {
            int currentValue = objA.GetComponent<drag_and_drop>().switchConnectedNumber;
            objA.GetComponent<drag_and_drop>().switchConnectedNumber++;
            if (objA.tag == "Switch") { objA.GetComponent<drag_and_drop>().eth.Add("swp" + currentValue.ToString()); }
            else { objA.GetComponent<drag_and_drop>().eth.Add("eth" + currentValue.ToString()); }
        }
        else if (objB.tag == "Router")
        {
            int currentValue = objA.GetComponent<drag_and_drop>().routerConnectedNumber;
            objA.GetComponent<drag_and_drop>().routerConnectedNumber++;
            if (objA.tag == "Switch") { objA.GetComponent<drag_and_drop>().eth.Add("swp" + currentValue.ToString()); }
            else { objA.GetComponent<drag_and_drop>().eth.Add("eth" + currentValue.ToString()); }
        }
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
        provideEthInterface(objectA, objectB);
        provideEthInterface(objectB, objectA);
    }

}
