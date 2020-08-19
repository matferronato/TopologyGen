using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class line_drawer : MonoBehaviour
{

    private LineRenderer lineRend;
    private Vector2 mousePos;
    private Vector2 startMousePos;
    public float Zvalue = 0;

    // Start is called before the first frame update
    void Start()
    {
        lineRend = GetComponent <LineRenderer>();
        lineRend.positionCount = 2;

    }

    // Update is called once per frame
    void Update()
    {
        if (button_handler.allowLines == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Zvalue));
            }
            if (Input.GetMouseButton(0))
            {
                mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Zvalue));
                lineRend.SetPosition(0, new Vector3(startMousePos.x, startMousePos.y, 0f));
                lineRend.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0f));
            }
        }
    }
}
