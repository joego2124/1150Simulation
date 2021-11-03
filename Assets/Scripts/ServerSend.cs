using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend : MonoBehaviour
{
    public static void PlayerPosition(StateMSG msg) {
        FindObjectOfType<Prediction>().SendToClient(msg);
    }
}
