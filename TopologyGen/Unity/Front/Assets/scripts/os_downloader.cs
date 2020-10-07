using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using UnityEngine.SceneManagement;

public class os_downloader : MonoBehaviour
{
    public GameObject PastButton;
    public InputField thisTextInput;
    public Dropdown get_thisDropdown;
    public static Dropdown thisDropdown;

    static GameObject thisName;
    public static List<string> OSOptionsList = new List<string>();
    public static List<string> OSversionOptionsList = new List<string>();
    static int thisSetup;
    int page = 1;

    bool OsFound = false;

    // Start is called before the first frame update
    void Awake()
    {
        runScript("");
        //thisName = get_thisName;
        thisDropdown = get_thisDropdown;
        OSOptionsList = getAvaiableOS();
        OSversionOptionsList = getAvaiableOSversion();
        PopulateDropdown(thisDropdown,OSOptionsList);
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

    void runScript(string value) {
      string strCmdText ="";
      //GameView
      if (menu_controller.OnGameRunning == false)
      {
          string getAvaiableOs = "gnome-terminal -x bash -ic ' cd ../../source/Bash/; ./run_downloader.sh " + value + " ;'";
          UnityEngine.Debug.Log(getAvaiableOs);
          ExecuteCommand(getAvaiableOs);
      }
      //GameBuild
      if (menu_controller.OnGameRunning == true)
      {
          string getAvaiableOs = "gnome-terminal -x bash -ic ' cd ../../../source/Bash/; ./run_downloader.sh " + value + " ;'";
          ExecuteCommand(getAvaiableOs);
      }
      Thread.Sleep(2000);
      OsFound = true;
    }
    // Update is called once per frame
    void Update()
    {
      if (OsFound == true)
      {
        OSOptionsList.Clear();
        OSversionOptionsList.Clear();
        OSOptionsList = getAvaiableOS();
        OSversionOptionsList = getAvaiableOSversion();
        PopulateDropdown(thisDropdown,OSOptionsList);
        OsFound = false;
      }
      if(page > 1){PastButton.SetActive(true);} else {PastButton.SetActive(false);}

    }

    public void GetNext(){
      page = page + 1;
      string appendUrl = "?page="+page.ToString();
      runScript(appendUrl);
    }

    public void GetPast(){
      page = page - 1;
      string appendUrl = "?page="+page.ToString();
      runScript(appendUrl);
    }

    public void RunDownload(){
      string myOs = thisDropdown.options[thisDropdown.value].text;
      string myOsVersion = OSversionOptionsList[thisDropdown.value];
      if (menu_controller.OnGameRunning == false)
      {
          string getAvaiableOs = "gnome-terminal -x bash -ic ' vagrant box add " + myOs + " ; vagrant mutate "+myOs+" libvirt ; echo "+myOs+" >> ../../Automate/vagrant_box/boxes.txt ; echo "+myOsVersion+" >> ../../Automate/vagrant_box/boxes_version.txt '";
          UnityEngine.Debug.Log(getAvaiableOs);
          ExecuteCommand(getAvaiableOs);
      }
      //GameBuild
      if (menu_controller.OnGameRunning == true)
      {
          string getAvaiableOs = "gnome-terminal -x bash -ic ' vagrant box add " + myOs + " ; vagrant mutate "+myOs+" libvirt ; echo "+myOs+" >> ../../../Automate/vagrant_box/boxes.txt ; echo "+myOsVersion+" >> ../../../Automate/vagrant_box/boxes_version.txt '";
          ExecuteCommand(getAvaiableOs);
      }
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
        string path = "";
        //GameView
        if (menu_controller.OnGameRunning == false)
        {
            path = @"../../Automate/vagrant_box/boxes_to_download_v.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../Automate/vagrant_box/boxes_to_download_v.txt";
        }
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
        List<string> thisList = new List<string> ();
        string line;
        //GameView
        string path = "";
        if (menu_controller.OnGameRunning == false)
        {
            path = @"../../Automate/vagrant_box/boxes_to_download.txt";
        }
        //GameBuild
        if (menu_controller.OnGameRunning == true)
        {
            path = @"../../../Automate/vagrant_box/boxes_to_download.txt";
        }
        using (StreamReader os_file = new StreamReader(path))
        {
            while ((line = os_file.ReadLine()) != null)
            {
                thisList.Add(line);
            }
        }
        return thisList;
    }

    public void SearchOS()
    {
        string thisText = thisTextInput.text;
        string appendUrl = "!"+thisText;
        thisTextInput.text = "";
        runScript(appendUrl);
    }

    public void RunTopology()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
