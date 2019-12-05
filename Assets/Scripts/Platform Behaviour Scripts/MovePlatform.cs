using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour {

    [Tooltip("How long in seconds it takes to move to the target.")]
    public float time = 1;
    private float passedTime = 0f;
    [SerializeField]
    private float speed = 0;

    public Transform target;
    private Vector3 targetPoint;
    private Vector3 originalPoint;
    private bool goingToTarget = true;


    void Start() {
        originalPoint = transform.position;
    }

    void Update() {
        Move();

        speed = (originalPoint - target.position).magnitude / time;
    }

    private void Move() {
        Debug.DrawLine(transform.position, originalPoint, Color.blue);
        Debug.DrawLine(transform.position, target.position, Color.red);

        if (goingToTarget == true) {
            //transform.position = Vector3.Slerp(originalPoint, target.position, passedTime / time);
            transform.position = Vector3.Lerp(originalPoint, target.position, passedTime / time);
            passedTime += Time.deltaTime;

            if (passedTime > time) {
                passedTime = 0;
                goingToTarget = !goingToTarget;
            }
        }
        else {
            transform.position = Vector3.Lerp(target.position, originalPoint, passedTime / time);
            transform.position = Vector3.Lerp(target.position, originalPoint, passedTime / time);
            passedTime += Time.deltaTime;

            if (passedTime > time) {
                passedTime = 0;
                goingToTarget = !goingToTarget;
            }
        }
    }
}