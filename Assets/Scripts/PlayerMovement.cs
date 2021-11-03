using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Input inputs;

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            Physics.IgnoreCollision(collision.transform.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    public void MovePlayer(Rigidbody rb, Inputs inputs) {
        if (inputs.up) rb.AddForce(Vector3.forward * 20);
        if (inputs.down) rb.AddForce(Vector3.back * 20);
        if (inputs.left) rb.AddForce(Vector3.left * 20);
        if (inputs.right) rb.AddForce(Vector3.right * 20);
        if (inputs.jump) rb.AddForce(Vector3.up * 20);
    }
}

