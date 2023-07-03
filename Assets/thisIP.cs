using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Mirror;
using TMPro;
using UnityEngine;

public class thisIP : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    void Start()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                text.text = ip.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
