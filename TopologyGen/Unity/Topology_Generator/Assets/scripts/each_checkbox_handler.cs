using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class each_checkbox_handler : MonoBehaviour
{
    public int myId;
    static bool selected;
    static string myParent;
    // Start is called before the first frame update
    void Awake()
    {

    }

    void Start()
    {
 
    }

    public static void toggleService(bool value, string stringId, string parentId)
    {
        string objName = stringId.Replace("checkBox", "");
        int myId = int.Parse(objName);
        if (!value)
        {
            if (parentId.Contains("Switch") && optSwitch_controller.blockToggleFunctions == true) {
                optSwitch_controller.BoolServiceList[myId] = false; 
            }
            else if (parentId.Contains("Server") && optServer_controller.blockToggleFunctions == true)
            {
                optServer_controller.BoolServiceList[myId] = false;
            }
            else if (parentId.Contains("Router") && optRouter_controller.blockToggleFunctions == true)
            {
                optRouter_controller.BoolServiceList[myId] = false;
            }
        }
        else
        {
            if (parentId.Contains("Switch") && optSwitch_controller.blockToggleFunctions == true) { 
                optSwitch_controller.BoolServiceList[myId] = true;
            }
            else if (parentId.Contains("Server") && optServer_controller.blockToggleFunctions == true)
            {
                optServer_controller.BoolServiceList[myId] = true;
            }
            else if (parentId.Contains("Router") && optRouter_controller.blockToggleFunctions == true)
            {
                optRouter_controller.BoolServiceList[myId] = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
