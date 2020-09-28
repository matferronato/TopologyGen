using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class teste_to_discart : MonoBehaviour
{
    Text thisText;
    // Start is called before the first frame update
    void Start()
    {
      thisText = GetComponent<Text>();
      thisText.text = "oi";
    }

    // Update is called once per frame
    void Update()
    {
      thisText.text = "x = " + Input.mousePosition.x.ToString() + " y = " + Input.mousePosition.y.ToString() + " " + Screen.width.ToString() + "x" + Screen.height.ToString();
      //thisText.text = Screen.width.ToString() + "x" + Screen.height.ToString();
    }
}
