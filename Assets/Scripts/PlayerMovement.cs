using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {


    public MovementSettings movementSettings;
    public InputSettings inputSettings;

    private Rigidbody playerRigidbody;
    private Vector3 velocity;
    private Quaternion targetRotation;

    private float forwardInput;
    private float sidewaysInput;
    private float turnInput;    
    private float jumpInput;

    private void Awake() {
        velocity = Vector3.zero;
        forwardInput = sidewaysInput = turnInput = jumpInput = 0;
        targetRotation = transform.rotation;
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        GetInput();
        Turn();
    }

    void FixedUpdate() {
        IsGrounded();
        Run();
        Jump();
    }

    void GetInput() {
        if (inputSettings.FORWARD_AXIS.Length != 0) forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);
        if (inputSettings.SIDEWAYS_AXIS.Length != 0) sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);
        if (inputSettings.TURN_AXIS.Length != 0) turnInput = Input.GetAxis(inputSettings.TURN_AXIS);
        if (inputSettings.JUMP_AXIS.Length != 0) jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
    }

    void Run() {
        velocity.z = forwardInput * movementSettings.runVelocity;
        velocity.x = sidewaysInput * movementSettings.runVelocity;

        velocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = transform.TransformDirection(velocity);
    }

    void Turn() {
        if (turnInput != 0f) {
            targetRotation *= Quaternion.AngleAxis(movementSettings.rotateVelocity * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    void Jump() {
        if (jumpInput != 0 && IsGrounded()) {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, movementSettings.jumpVelocity, playerRigidbody.velocity.z);
        }
    }

    bool IsGrounded() {

        RaycastHit hitInfo;

        float sphereRadius = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.9f;
        bool hit = Physics.SphereCast(transform.position, sphereRadius, -transform.up, out hitInfo, movementSettings.distanceToGround, movementSettings.ground);
        return hit;
    }

}

[System.Serializable]
public class MovementSettings {
    public float runVelocity = 12;
    public float rotateVelocity = 100;
    public float jumpVelocity = 8;
    public float distanceToGround = 1.3f;
    public LayerMask ground = 0;
}

[System.Serializable]
public class InputSettings {
    public string FORWARD_AXIS = "Vertical";
    public string SIDEWAYS_AXIS = "Horizontal";
    public string TURN_AXIS = "Mouse X";
    public string JUMP_AXIS = "Jump";
}