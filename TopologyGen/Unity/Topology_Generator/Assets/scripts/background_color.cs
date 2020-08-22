using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class background_color : MonoBehaviour
{
    Material currentMaterial;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);

    }

    // Update is called once per frame
    void Update()
    {
        if(button_handler.startRunning == true)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        } else if (button_handler.allowLines == true) 
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
        } else
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
    }
}
