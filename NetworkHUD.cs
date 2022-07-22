using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkHUD : MonoBehaviour
{
    NetworkManager manager;
    public InputField inputfield;
    public GameObject canvas;

    public void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void hostjoin()
    {
        manager.StartHost();
        

        canvas.SetActive(false);
    }

    public void host()
    {
        manager.StartServer();

        canvas.SetActive(false);
    }

    public void client()
    {
        manager.networkAddress = inputfield.text;
        manager.StartClient();

        canvas.SetActive(false);
    }

}
