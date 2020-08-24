using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class drag_and_drop :  MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int myUniquelNumber = 0;

    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public bool selected;
    public bool linked;
    
    public List<GameObject> connections = new List<GameObject>();
    public List<string> ip = new List<string>();
    public List<string> eth = new List<string>();
    public List<GameObject> attatchedText = new List<GameObject>();
    public string OS;
    public string OSversion;
    public int machineSetup;
    public List<bool> services = new List<bool>();

    public int serverConnectedNumber;
    public int switchConnectedNumber;
    public int routerConnectedNumber;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (gameObject.tag == "Server") {
            myUniquelNumber = (int)button_handler.serverNameQueue.Dequeue();
            Text name = gameObject.GetComponentInChildren<Text>();
            gameObject.name= "Server_" + myUniquelNumber.ToString();
            name.text = " Server_" + myUniquelNumber.ToString();

            OS = optServer_controller.OSOptionsList[0];
            OSversion = optServer_controller.OSversionOptionsList[0];
            machineSetup = 1;
            foreach (string service in optServer_controller.ServicesOptionsList)
            {
                services.Add(false);
            }
        }
        else if (gameObject.tag == "Switch") { 
            myUniquelNumber = (int)button_handler.switchNameQueue.Dequeue();
            Text name = gameObject.GetComponentInChildren<Text>();
            gameObject.name = "Switch_" + myUniquelNumber.ToString();
            name.text = " Switch_" + myUniquelNumber.ToString();
            
            OS = optSwitch_controller.OSOptionsList[0];
            OSversion = optSwitch_controller.OSversionOptionsList[0];
            machineSetup = 1;
            foreach (string service in optSwitch_controller.ServicesOptionsList) {
                services.Add(false);
            }
        }
        else if (gameObject.tag == "Router") {
            myUniquelNumber = (int)button_handler.routerNameQueue.Dequeue();
            Text name = gameObject.GetComponentInChildren<Text>();
            gameObject.name = "Router_" + myUniquelNumber.ToString();
            name.text = " Router_" + myUniquelNumber.ToString();

            OS = optRouter_controller.OSOptionsList[0];
            OSversion = optRouter_controller.OSversionOptionsList[0];
            machineSetup = 1;
            foreach (string service in optRouter_controller.ServicesOptionsList)
            {
                services.Add(false);
            }
        }
        
    }

    void Start()
    {
        Canvas[] canvas_list = GetComponentsInParent<Canvas>();
        canvas = canvas_list[canvas_list.Length - 1];
        selected = false;
        linked = false;
        serverConnectedNumber = 1;
        switchConnectedNumber = 50;
        routerConnectedNumber = 100;

    }

    void Update()
    {
        var viewPortPos = Camera.main.WorldToViewportPoint(transform.position);
        //Debug.Log(viewPortPos);
        if (viewPortPos.y < 0.1 || viewPortPos.x < 0.2 || viewPortPos.y > 56 || viewPortPos.x > 56 )
        {
            UnityEngine.Object.Destroy(gameObject);
        }
        if (selected == true)
        {
            canvasGroup.alpha = .2f;
        }


    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        if (button_handler.allowLines == false && linked == false)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        } 
     }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetMouseButtonDown(1) && button_handler.startRunning == false)
        {
            button_handler.optionRunning = true;
            menu_controller.Menu.SetActive(false);
            menu_controller.Options_Switch.SetActive(false);
            menu_controller.Options_Server.SetActive(false);
            menu_controller.Options_Router.SetActive(false);
            if (this.gameObject.tag == "Switch")
            {
                menu_controller.Options_Switch.SetActive(true);
                optSwitch_controller.setCurrentSwitch(this.gameObject);
            }
            else if (this.gameObject.tag == "Server")
            {
                menu_controller.Options_Server.SetActive(true);
                optServer_controller.setCurrentServer(this.gameObject);
            }
            else if (this.gameObject.tag == "Router")
            {
                menu_controller.Options_Router.SetActive(true);
                optRouter_controller.setCurrentRouter(this.gameObject);
            }
        }
    }

    public void OnClick()
    {
       if( button_handler.startRunning == true )
        {
            string strCmdText;
            //GameView
            strCmdText = "/C C:\\Users\\matheus_ferronato\\MyProjects\\TCC\\TopologyGen\\TopologyGen\\Windows\\bash.exe";
            //GameBuild
            //strCmdText = "/C C:\\Users\\matheus_ferronato\\MyProjects\\TCC\\TopologyGen\\TopologyGen\\Windows\\bash.exe";
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);
        }
        if (button_handler.allowLines == true )
        {
            selected = true;
            Debug.Log(this.gameObject.name);
            line_creation.generateLine(this.gameObject);
        }
    }


}
