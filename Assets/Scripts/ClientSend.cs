using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    public static void SendInputToServer(inputMSG _inputMSG) {
        FindObjectOfType<PlayerManager>().SendToServer(_inputMSG);
    }
}
