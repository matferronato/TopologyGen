using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class magnetic_dop : MonoBehaviour, IDropHandler
{
    public static bool deletedServer = false;
    public static bool deletedSwitch = false;
    public static bool deletedRouter = false;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (eventData.pointerDrag.tag == "Server") {
                if (eventData.pointerDrag.GetComponent<drag_and_drop>().linked == false)
                {
                    button_handler.serverNameQueue.Enqueue(eventData.pointerDrag.GetComponent<drag_and_drop>().myUniquelNumber);
                    button_handler.serverObjQueue = new Queue<GameObject>(button_handler.serverObjQueue.Where(x => x != eventData.pointerDrag));
                    deletedServer = true;
                }
            }
            else if (eventData.pointerDrag.tag == "Switch") {
                if (eventData.pointerDrag.GetComponent<drag_and_drop>().linked == false)
                {
                    button_handler.switchNameQueue.Enqueue(eventData.pointerDrag.GetComponent<drag_and_drop>().myUniquelNumber);
                    button_handler.switchObjQueue = new Queue<GameObject>(button_handler.switchObjQueue.Where(x => x != eventData.pointerDrag));
                    deletedSwitch = true;
                }
            }
            else if (eventData.pointerDrag.tag == "Router") {
                if (eventData.pointerDrag.GetComponent<drag_and_drop>().linked == false)
                {
                button_handler.routerNameQueue.Enqueue(eventData.pointerDrag.GetComponent<drag_and_drop>().myUniquelNumber);
                button_handler.routerObjQueue = new Queue<GameObject>(button_handler.routerObjQueue.Where(x => x != eventData.pointerDrag));
                deletedRouter = true;
                }
            }
            Destroy(eventData.pointerDrag);
        }
    }
}
