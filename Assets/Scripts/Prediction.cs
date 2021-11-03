using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prediction : MonoBehaviour
{
    public static bool autoSimulation = false;

    public PlayerMovement player_client;

    private uint tick_number;

    private ClientState[] client_state_buffer = new ClientState[1024];
    private Inputs[] client_input_buffer = new Inputs[1024];
    Inputs current_inputs;

    void Awake() {
        Physics.autoSimulation = false;
        for (int i = 0; i < client_state_buffer.Length; i++) {
            client_state_buffer[i] = new ClientState();
        }
    }

    void FixedUpdate() {
        current_inputs = SetInputs();
        inputMSG _inputMSG = new inputMSG(tick_number, current_inputs);

        ClientSend.SendInputToServer(_inputMSG);

        uint buffer_slot = this.tick_number % 1024;
        client_input_buffer[buffer_slot] = current_inputs;
        client_state_buffer[buffer_slot].position = player_client.GetComponent<Rigidbody>().position;
        client_state_buffer[buffer_slot].rotation = player_client.GetComponent<Rigidbody>().rotation;

        player_client.MovePlayer(player_client.GetComponent<Rigidbody>(), current_inputs);
        Physics.Simulate(Time.fixedDeltaTime);
        tick_number++;
    }

    public void SendToClient(StateMSG msg) {
        Rigidbody player_rigidbody = player_client.GetComponent<Rigidbody>();
        player_rigidbody.position = msg.position;
        player_rigidbody.rotation = msg.rotation;
        player_rigidbody.velocity = msg.velocity;
        player_rigidbody.angularVelocity = msg.angularVel;
    }

    public Inputs SetInputs() {
        Inputs inputs = new Inputs();
        inputs.up = Input.GetKey(KeyCode.W);
        inputs.down = Input.GetKey(KeyCode.S);
        inputs.left = Input.GetKey(KeyCode.A);
        inputs.right = Input.GetKey(KeyCode.D);
        inputs.jump = Input.GetKey(KeyCode.Space);

        return inputs;
    }
}


public class Inputs {
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool jump;

    public Inputs(bool _up, bool _down, bool _left, bool _right, bool _jump) {
        up = _up;
        down = _down;
        left = _left;
        right = _right;
        jump = _jump;
    }

    public Inputs() {
        up = false;
        down = false;
        left = false;
        right = false;
        jump = false;
    }
}

public class inputMSG {
    public uint tick;
    public Inputs inputs;

    public inputMSG(uint _tick, Inputs _inputs) {
        tick = _tick;
        inputs = _inputs;
    }
}

public class ClientState {
    public uint tick;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;

    public ClientState(uint _tick, Vector3 _position, Quaternion _rotation, Vector3 _velocity) {
        tick = _tick;
        position = _position;
        rotation = _rotation;
        velocity = _velocity;
    }

    public ClientState() {
        tick = 0;
        position = Vector3.zero;
        velocity = Vector3.zero;
        rotation = Quaternion.identity;
    }
}