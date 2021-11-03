using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    
    public PlayerMovement player_server;

    public static bool autoSimulation = false;

    public float latency;

    private uint tick;

    private Queue<inputMSG> serviceQueue = new Queue<inputMSG>();

    public void SendToServer(inputMSG inputMSG) {
        serviceQueue.Enqueue(inputMSG);
    }

    void FixedUpdate() {

        if (serviceQueue.Count == 0) return;

        inputMSG inputMSG = serviceQueue.Peek();

        if ((inputMSG.tick + latency) == tick) {

            Rigidbody rb = player_server.GetComponent<Rigidbody>();
            player_server.MovePlayer(player_server.GetComponent<Rigidbody>(), inputMSG.inputs);

            Physics.Simulate(Time.fixedDeltaTime);

            StateMSG msg = new StateMSG();
            msg.position = rb.position;
            msg.rotation = rb.rotation;
            msg.velocity = rb.velocity;
            msg.angularVel = rb.angularVelocity;

            msg.tick = this.tick;

            ServerSend.PlayerPosition(msg);

            serviceQueue.Dequeue();
        }

        this.tick++;
    }
}

public class StateMSG {
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public Vector3 angularVel;
    public uint tick;

    public StateMSG(Vector3 _position, Quaternion _rotation, Vector3 _velocity, Vector3 _angularVel, uint _tick) {
        tick = _tick;
        position = _position;
        rotation = _rotation;
        velocity = _velocity;
        angularVel = _angularVel;
    }

    public StateMSG() {
        position = Vector3.zero;
        rotation = Quaternion.identity;
        velocity = Vector3.zero;
        angularVel = Vector3.zero;
        tick = 0;
    }
}