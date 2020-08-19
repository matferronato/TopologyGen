using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class magnetic_dop : MonoBehaviour, IDropHandler
{
    public static bool deletedServer = false;
    public static bool deletedSwitch = false;
    public static bool deletedRouter = false;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.tag == "Server") {
                button_handler.serverNameQueue.Enqueue(eventData.pointerDrag.GetComponent<drag_and_drop>().myUniquelNumber);
                deletedServer = true;
            }
            else if (eventData.pointerDrag.tag == "Switch") {
                button_handler.switchNameQueue.Enqueue(eventData.pointerDrag.GetComponent<drag_and_drop>().myUniquelNumber);
                deletedSwitch = true;
            }
            else if (eventData.pointerDrag.tag == "Router") {
                button_handler.routerNameQueue.Enqueue(eventData.pointerDrag.GetComponent<drag_and_drop>().myUniquelNumber);
                deletedRouter = true;
            }
            Destroy(eventData.pointerDrag);
        }
    }
}
