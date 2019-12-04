using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour {

    public enum BehaviourTypes { none, move, rotate }
    public BehaviourTypes behaviourType = BehaviourTypes.none;

    [Header("General:")]
    [Tooltip("How long in seconds it takes to complete the behaviour.")]
    public float time = 1;

    [Header("Move:")]
    public Transform target;
    private Vector3 targetPoint;
    private Vector3 originalPoint;
    private bool goingToTarget = true;
    private float passedTime = 0f;

    [Header("Rotate:")]
    public bool isLocal = true;
    public Vector3 rotationAngle;

    void Start() {
        originalPoint = transform.position;
    }

    void Update() {
        switch (behaviourType) {
            case (BehaviourTypes.move):
                Move();
                break;
            case (BehaviourTypes.rotate):
                Rotate();
                break;
        }
    }

    private void Move() {
        Debug.DrawLine(transform.position, originalPoint, Color.blue);
        Debug.DrawLine(transform.position, target.position, Color.red);

        if (goingToTarget == true) {
            Debug.Log(passedTime / time);
            transform.position = Vector3.Slerp(originalPoint, target.position, (1 / time) * passedTime);
            passedTime += Time.deltaTime;

            if (passedTime > time) {
                passedTime = 0;
                goingToTarget = !goingToTarget;
            }
        }
        else {
            Debug.Log(passedTime / time);
            transform.position = Vector3.Slerp(target.position, originalPoint, (1 / time) * passedTime);
            passedTime += Time.deltaTime;

            if (passedTime > time) {
                passedTime = 0;
                goingToTarget = !goingToTarget;
            }
        }
    }

    private void Rotate() {

    }
}