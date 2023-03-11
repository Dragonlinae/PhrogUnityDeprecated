using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode.Transports.UTP;

public class NetworkUI : MonoBehaviour
{
    public GameObject canvas;
    public TMP_InputField ipInput;
    public TMP_InputField portInput;
    private NetworkManager netManager;

    // Start is called before the first frame update
    void Start()
    {
        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        netManager = GetComponentInParent<NetworkManager>();
        // Disable Singleplayer Frog Controller
        foreach (var player in FindObjectsOfType<SingleFrogController>())
        {
            player.enabled = false;
        }
    }

    // When host button is pressed
    public void Host()
    {
        foreach (var player in FindObjectsOfType<SingleFrogController>())
        {
            player.enabled = true;
        }
        // Start the server
        netManager.StartHost();
        // Hide the canvas
        canvas.SetActive(false);
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // When client button is pressed
    public void Client()
    {
        foreach (var player in FindObjectsOfType<SingleFrogController>())
        {
            player.enabled = true;
        }
        // Retrieve the ip and port from the input fields
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ipInput.text, ushort.Parse(portInput.text));
        // Start the client
        netManager.StartClient();
        // Hide the canvas
        canvas.SetActive(false);
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
