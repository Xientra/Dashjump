using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    public Camera playerCamera;
    private Transform cameraAnchor;


    public MovementSettings movementSettings;
    public InputSettings inputSettings;

    private Rigidbody playerRigidbody;
    private Vector3 velocity;
    private Quaternion targetRotation;

    private float forwardInput;
    private float sidewaysInput;
    private Vector2 turnInput;    
    private float jumpInput;

    private void Awake() {
        velocity = Vector3.zero;
        forwardInput = 0;
        sidewaysInput = 0;
        turnInput.x = 0;
        turnInput.y = 0;
        jumpInput = 0;

        targetRotation = transform.rotation;

        playerRigidbody = gameObject.GetComponent<Rigidbody>();

        if (playerCamera != null) cameraAnchor = playerCamera.transform.parent;
        if (cameraAnchor.parent != null) cameraAnchor.SetParent(null);
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        GetInput();
        Turn();
        UpdateCameraAnchor();
    }

    void FixedUpdate() {
        Move();
        Jump();
    }

    void GetInput() {
        if (inputSettings.FORWARD_AXIS.Length != 0) forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);

        if (inputSettings.SIDEWAYS_AXIS.Length != 0) sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);

        if (inputSettings.TURN_AXIS_X.Length != 0) turnInput.x = Input.GetAxis(inputSettings.TURN_AXIS_X);
        //if (inputSettings.TURN_AXIS_Y.Length != 0) turnInput.y = Input.GetAxis(inputSettings.TURN_AXIS_Y);

        if (inputSettings.JUMP_AXIS.Length != 0) jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
    }

    void Move() {
        velocity.z = forwardInput * movementSettings.runVelocity;
        velocity.x = sidewaysInput * movementSettings.runVelocity;

        velocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = transform.TransformDirection(velocity);
    }

    void Turn() {
        if (turnInput.x != 0f || turnInput.y != 0f) {
            targetRotation *= Quaternion.AngleAxis(movementSettings.rotateVelocity * turnInput.x * Time.deltaTime, Vector3.up);
            //targetRotation *= Quaternion.AngleAxis(movementSettings.rotateVelocity * turnInput.y * Time.deltaTime, Vector3.right);
        }
        transform.rotation = targetRotation;
    }

    void UpdateCameraAnchor() {
        cameraAnchor.SetPositionAndRotation(transform.position, transform.rotation);
    }

    void Jump() {
        bool _isGrounded = IsGrounded();

        if (jumpInput != 0f && _isGrounded) {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, movementSettings.jumpVelocity, playerRigidbody.velocity.z);
        }

        
        // this means falling
        //if (playerRigidbody.velocity.y < 0) {
        //    playerRigidbody.AddForce(-transform.up * movementSettings.additionalFallingForce);
        //}
        
        // this is still jumping but if jumping is not pressed add lowjumpForce
        if (jumpInput == 0 && IsGrounded() == false && !(playerRigidbody.velocity.y < 0)) {
            playerRigidbody.AddForce(-transform.up * movementSettings.lowjumpForce);
        }
        

        if (_isGrounded == false) {
            playerRigidbody.AddForce(-transform.up * movementSettings.additionalFallingForce);
        }

        if (_isGrounded == true) {
            movementSettings.jumpingMovementSpeed = 1;
        }
        else {
            movementSettings.jumpingMovementSpeed = movementSettings.jumpingMovementSpeedMultiplier;
        }
    }

    bool IsGrounded() {

        RaycastHit hitInfo;

        float sphereRadius = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.9f;
        bool hit = Physics.SphereCast(transform.position, sphereRadius, -transform.up, out hitInfo, movementSettings.distanceToGround, movementSettings.ground);
        return hit;
    }


    private void OnTriggerEnter(Collider other) {
        
    }
}

[System.Serializable]
public class MovementSettings {
    public float runVelocity = 12;

    public float rotateVelocity = 100;

    public float jumpVelocity = 8;
    public float distanceToGround = 1.3f;
    public LayerMask ground = 0;
    public float lowjumpForce = 10f;
    public float additionalFallingForce = 2f;
    [Range(0f, 1f)]
    public float jumpingMovementSpeedMultiplier = 0.5f;
    [HideInInspector]
    public float jumpingMovementSpeed = 1;
}

[System.Serializable]
public class InputSettings {
    public string FORWARD_AXIS = "Vertical";
    public string SIDEWAYS_AXIS = "Horizontal";
    public string TURN_AXIS_X = "Mouse X";
    public string TURN_AXIS_Y = "Mouse Y";
    public string JUMP_AXIS = "Jump";
}