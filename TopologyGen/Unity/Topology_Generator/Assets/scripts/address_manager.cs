using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class address_manager : MonoBehaviour
{

    public static string IP = "X.X.X.Y/24";
    public static string currentIp;
    public static int currentValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void AddressManager()
    {
        currentIp = IP.Replace("X", currentValue.ToString());
        currentValue = currentValue + 1;
    }

    public static string provideAddress()
    {
        AddressManager();
        return currentIp;
    }
}
