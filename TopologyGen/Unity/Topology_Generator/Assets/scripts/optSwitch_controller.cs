using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class optSwitch_controller : MonoBehaviour
{
    static GameObject thisSwitch;
    public GameObject get_thisName;
    public InputField thisTextInput;
    public Dropdown thisDropdown;
    public GameObject get_toggleOn;
    public GameObject get_toggleOff;

    public static GameObject toggleOn;
    public static GameObject toggleOff;
    static GameObject thisName;
    public static List<string> OSOptionsList;
    static int thisSetup;
    // Start is called before the first frame update
    void Start()
    {
        thisName = get_thisName;
        OSOptionsList = getAvaiableOS();
        PopulateDropdown(thisDropdown,OSOptionsList);
        toggleOn = get_toggleOn;
        toggleOff = get_toggleOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopulateDropdown(Dropdown dropdown, List<string> optionsArray)
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(optionsArray);
    }

    public List<string> getAvaiableOS()
    {
        List<string> thisList = new List<string> ();
        string line;
        string path = @"..\\..\\Vagrant\\vagrant_box\\boxes.txt";
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
        if(thisSetup == 1)
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

    public static void setCurrentSwitch(GameObject otherSwitch)
    {
        thisSwitch = otherSwitch;
        thisName.GetComponent<Text>().text = thisSwitch.name;
        thisSetup = thisSwitch.GetComponent<drag_and_drop>().machineSetup;
        if (thisSwitch.GetComponent<drag_and_drop>().machineSetup == 1)
        {
            toggleOn.SetActive(true);
            toggleOff.SetActive(false);
        }
        else
        {
            toggleOn.SetActive(false);
            toggleOff.SetActive(true);
        }

    }

    public void giveNewName()
    {
        string thisText = thisTextInput.text;
        if (thisText == "")
            return;
        else
        {
            thisSwitch.name = "Switch_" + thisText;
            thisSwitch.GetComponentInChildren<Text>().text = thisSwitch.name;
            thisTextInput.text = "";
        }
    }

    public void giveOSOption()
    {
        Debug.Log("old os = " + thisSwitch.GetComponent<drag_and_drop>().OS);
        thisSwitch.GetComponent<drag_and_drop>().OS = thisDropdown.options[thisDropdown.value].text;
        Debug.Log("new os = " + thisSwitch.GetComponent<drag_and_drop>().OS);
    }

    public void check()
    {
        giveNewName();
        giveOSOption();
        close();
        thisSwitch.GetComponent<drag_and_drop>().machineSetup = thisSetup;

    }

    public void close()
    {
        menu_controller.Menu.SetActive(true);
        menu_controller.Options_Server.SetActive(false);
        menu_controller.Options_Switch.SetActive(false);
        menu_controller.Options_Router.SetActive(false);
    }
}
