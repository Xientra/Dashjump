using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlatform : MonoBehaviour {

    public float speed = 10f;
    public bool isLocal = true;
    public Vector3 rotationAxis = Vector3.right;
    public Quaternion originalRotation;

    void Start() {
        originalRotation = transform.rotation;
    }

    void FixedUpdate() {
        Rotate();
    }

    private void Rotate() {
        transform.Rotate(rotationAxis, speed * Time.deltaTime);
        //transform.Rota
    }
}