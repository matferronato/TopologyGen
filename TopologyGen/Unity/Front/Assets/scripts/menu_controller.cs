using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu_controller : MonoBehaviour
{
    public static bool OnGameRunning = true;

    public GameObject get_Menu;
    public GameObject get_Options_Server;
    public GameObject get_Options_Switch;
    public GameObject get_Options_Router;

    public static GameObject Menu;
    public static GameObject Options_Server;
    public static GameObject Options_Switch;
    public static GameObject Options_Router;
    // Start is called before the first frame update
    void Start()
    {
        Menu = get_Menu;
        Options_Server = get_Options_Server;
        Options_Switch = get_Options_Switch;
        Options_Router = get_Options_Router;

        Menu.SetActive(true);
        Options_Server.SetActive(false);
        Options_Switch.SetActive(false);
        Options_Router.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
