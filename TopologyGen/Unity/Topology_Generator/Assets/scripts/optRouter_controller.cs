﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class optRouter_controller : MonoBehaviour
{
    static GameObject thisRouter;
    public GameObject get_thisName;
    public InputField thisTextInput;
    public Dropdown get_thisDropdown;
    public static Dropdown thisDropdown;
    public GameObject get_toggleOn;
    public GameObject get_toggleOff;
    public GameObject chekBoxContainer;
    public Toggle defaultToggle;

    public static GameObject toggleOn;
    public static GameObject toggleOff;
    static GameObject thisName;
    public static List<string> OSOptionsList = new List<string>();
    public static List<string> OSversionOptionsList = new List<string>();
    static int thisSetup;
    public static List<string> ServicesOptionsList = new List<string>();
    public static List<Toggle> CheckBoxObjectList = new List<Toggle>();
    public static List<bool> BoolServiceList = new List<bool>();

    public static bool blockToggleFunctions;

    // Start is called before the first frame update
    void Awake()
    {
        thisName = get_thisName;
        toggleOn = get_toggleOn;
        toggleOff = get_toggleOff;
        thisDropdown = get_thisDropdown;
        OSOptionsList = getAvaiableOS();
        OSversionOptionsList = getAvaiableOSversion();
        PopulateDropdown(thisDropdown, OSOptionsList);
        ServicesOptionsList = getAvaiableCheckBoxOptions();
        PopulateCheckBox(ServicesOptionsList);
        blockToggleFunctions = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PopulateCheckBox(List<string> list)
    {
        int i = 0;
        foreach (string service in list)
        {
            var thisCheckBox = Instantiate(defaultToggle, new Vector2(chekBoxContainer.transform.position.x, chekBoxContainer.transform.position.y), Quaternion.identity);
            thisCheckBox.transform.parent = chekBoxContainer.transform;
            thisCheckBox.name = "checkBox" + i.ToString();
            thisCheckBox.GetComponentInChildren<Text>().text = service;
            thisCheckBox.isOn = false;
            thisCheckBox.transform.position = new Vector2(thisCheckBox.transform.position.x, thisCheckBox.transform.position.y - (i * 20));
            thisCheckBox.onValueChanged.AddListener((bool toggled) => { each_checkbox_handler.toggleService(toggled ? true : false, thisCheckBox.name, chekBoxContainer.name); });
            CheckBoxObjectList.Add(thisCheckBox);
            BoolServiceList.Add(false);
            i++;
        }
    }

    public List<string> getAvaiableCheckBoxOptions()
    {
        List<string> thisList = new List<string>();
        string line;
        //GameView
        string path = @"..\\..\\Vagrant\\vagrant_box\\router_services.txt";
        //GameBuild
        //string path = @"..\\..\\..\\Vagrant\\vagrant_box\\router_services.txt";
        using (StreamReader os_file = new StreamReader(path))
        {
            while ((line = os_file.ReadLine()) != null)
            {
                thisList.Add(line);
            }
        }
        return thisList;
    }

    public void PopulateDropdown(Dropdown dropdown, List<string> optionsArray)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(optionsArray);
    }

    public List<string> getAvaiableOSversion()
    {
        List<string> thisList = new List<string>();
        string line;
        //GameView
        string path = @"..\\..\\Vagrant\\vagrant_box\\boxes_version.txt";
        //GameBuild
        //string path = @"..\\..\\..\\Vagrant\\vagrant_box\\boxes_version.txt";
        using (StreamReader os_file = new StreamReader(path))
        {
            while ((line = os_file.ReadLine()) != null)
            {
                thisList.Add(line);
            }
        }
        return thisList;
    }

    public List<string> getAvaiableOS()
    {
        List<string> thisList = new List<string>();
        string line;
        //GameView
        string path = @"..\\..\\Vagrant\\vagrant_box\\boxes.txt";
        //GameBuild
        //string path = @"..\\..\\..\\Vagrant\\vagrant_box\\boxes.txt";
        using (StreamReader os_file = new StreamReader(path))
        {
            while ((line = os_file.ReadLine()) != null)
            {
                thisList.Add(line);
            }
        }
        return thisList;
    }

    public void togle()
    {
        if (thisSetup == 1)
        {
            thisSetup = 0;
            toggleOn.SetActive(false);
            toggleOff.SetActive(true);
        }
        else
        {
            thisSetup = 1;
            toggleOn.SetActive(true);
            toggleOff.SetActive(false);
        }
    }

    public GameObject returnChildrenByName(GameObject parent, string name)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    public void giveNewName()
    {
        string thisText = thisTextInput.text;
        if (thisText == "")
            return;
        else
        {
            thisRouter.name = "Router_" + thisText;
            thisRouter.GetComponentInChildren<Text>().text = thisRouter.name;
            thisTextInput.text = "";
        }
    }

    public void giveOSOption()
    {
        thisRouter.GetComponent<drag_and_drop>().OS = thisDropdown.options[thisDropdown.value].text;
    }



    public void resetMenuIcons()
    {
        GameObject Linesbutton = GameObject.Find("Lines_Button");
        GameObject Routerbutton = GameObject.Find("Router_Button");
        GameObject Switchbutton = GameObject.Find("Switch_Button");
        GameObject Serverbutton = GameObject.Find("Server_Button");
        Serverbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        Switchbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        Routerbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
        Linesbutton.GetComponent<Image>().color = new Color32(255, 255, 225, 255);
    }

    public static void setCurrentRouter(GameObject otherRouter)
    {
        int i = 0;
        blockToggleFunctions = true;
        thisRouter = otherRouter;
        thisName.GetComponent<Text>().text = thisRouter.name;
        thisSetup = thisRouter.GetComponent<drag_and_drop>().machineSetup;
        thisDropdown.value = OSOptionsList.FindIndex(a => a.Contains(thisRouter.GetComponent<drag_and_drop>().OS));
        if (thisRouter.GetComponent<drag_and_drop>().machineSetup == 1)
        {
            toggleOn.SetActive(true);
            toggleOff.SetActive(false);
        }
        else
        {
            toggleOn.SetActive(false);
            toggleOff.SetActive(true);
        }
        List<bool> clonedList = new List<bool>(thisRouter.GetComponent<drag_and_drop>().services);
        BoolServiceList = clonedList;
        foreach (Toggle checkbox in CheckBoxObjectList)
        {
            if (BoolServiceList[i] == true)
            {
                checkbox.isOn = true;
            }
            else
            {
                checkbox.isOn = false;
            }
            i++;
        }
    }

    public void check()
    {
        giveNewName();
        giveOSOption();
        thisRouter.GetComponent<drag_and_drop>().machineSetup = thisSetup;
        List<bool> clonedList = new List<bool>(BoolServiceList);
        thisRouter.GetComponent<drag_and_drop>().services = clonedList;
        close();
    }

    public void close()
    {
        blockToggleFunctions = true;
        int i = 0;
        foreach (Toggle checkbox in CheckBoxObjectList)
        {
            checkbox.isOn = false;
            BoolServiceList[i] = false;
            i++;
        }
        menu_controller.Menu.SetActive(true);
        menu_controller.Options_Server.SetActive(false);
        menu_controller.Options_Switch.SetActive(false);
        menu_controller.Options_Router.SetActive(false);
        button_handler.optionRunning = false;
        resetMenuIcons();
    }
}
