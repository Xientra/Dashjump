using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    public Camera playerCamera;
    private Transform cameraAnchor;
    private Quaternion cameraAnchorRotationOffset = Quaternion.identity;

    public Transform spawnPoint;

    [Header("Settings: ")]
    public MovementSettings movementSettings;
    public InputSettings inputSettings;


    private Rigidbody playerRigidbody;
    private Vector3 velocity;
    private Quaternion targetRotation;

    private float forwardInput;
    private float sidewaysInput;
    private Vector2 turnInput;    
    private float jumpInput;
    private bool dashInput;

    // platform stuff
    private Transform platformStandingOn;
    private Vector3 colLastPos;


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
        cameraAnchorRotationOffset = transform.rotation;

        Spawn();
    }

    public void Spawn() {
        transform.position = spawnPoint.position;
        transform.rotation = Quaternion.identity;
    }

    public void OnDeath() {
        Spawn();
		
    }

    void Update() {
        GetInput();

        

        Turn();

        UpdateCameraAnchor();
    }

    void FixedUpdate() {
        if (Dash() == false) {

            Move();
            Jump();


            UpdatePositionBasedOnPlatform();

        }

    }


    void GetInput() {
        if (inputSettings.FORWARD_AXIS.Length != 0) forwardInput = Input.GetAxis(inputSettings.FORWARD_AXIS);

        if (inputSettings.SIDEWAYS_AXIS.Length != 0) sidewaysInput = Input.GetAxis(inputSettings.SIDEWAYS_AXIS);

        if (inputSettings.TURN_AXIS_X.Length != 0) turnInput.x = Input.GetAxis(inputSettings.TURN_AXIS_X);
        if (inputSettings.TURN_AXIS_Y.Length != 0) turnInput.y = -Input.GetAxis(inputSettings.TURN_AXIS_Y);

        if (inputSettings.JUMP_AXIS.Length != 0) jumpInput = Input.GetAxisRaw(inputSettings.JUMP_AXIS);
        if (Input.GetKeyDown(KeyCode.LeftShift)) dashInput = true;
    }

    void Move() {
        velocity.z = forwardInput * movementSettings.runVelocity;
        velocity.x = sidewaysInput * movementSettings.runVelocity;

        velocity.y = playerRigidbody.velocity.y;
        playerRigidbody.velocity = transform.TransformDirection(velocity);
    }

    void Turn() {
        if (turnInput.x != 0f) {
            targetRotation *= Quaternion.AngleAxis(movementSettings.rotateVelocity * turnInput.x * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;

        if (turnInput.y != 0f) {
            cameraAnchorRotationOffset *= Quaternion.AngleAxis(movementSettings.rotateVelocity * turnInput.y * Time.deltaTime, Vector3.right);
        }
    }

    void UpdateCameraAnchor() {

        cameraAnchor.SetPositionAndRotation(transform.position, transform.rotation * cameraAnchorRotationOffset);
    }

    void Jump() {
        bool _isGrounded = IsGrounded();

        // if jump is pressed player is on ground
        if (jumpInput != 0f && _isGrounded) {
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, movementSettings.jumpVelocity, playerRigidbody.velocity.z);
        }
        
        // this is still jumping but if jumping is not pressed add lowjumpForce
        if (jumpInput == 0 && IsGrounded() == false && !(playerRigidbody.velocity.y < 0)) {
            // playerRigidbody.AddForce(-transform.up * movementSettings.lowjumpForce);
            playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, playerRigidbody.velocity.y * movementSettings.lowJumpMultiplier, playerRigidbody.velocity.z);
        }
        

        // this is basicially additional gravity (is done with the physics component Constant Force)
        //if (_isGrounded == false) {
        //    playerRigidbody.AddForce(-transform.up * movementSettings.additionalFallingForce);
        //}


        if (_isGrounded == true) {
            movementSettings.jumpingMovementSpeed = 1;
        }
        else {
            movementSettings.jumpingMovementSpeed = movementSettings.jumpingMovementSpeedMultiplier;
        }
    }

    bool IsGrounded() {


        float avgSize = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.9f;

        Vector3 boxSize = new Vector3(avgSize / 2, movementSettings.distanceToGround, avgSize / 2);
        bool hit = Physics.BoxCast(transform.position, boxSize / 2, -transform.up, transform.rotation, transform.lossyScale.y + boxSize.y / 2, movementSettings.ground);

        return hit;
    }

    /*
    private void OnDrawGizmos() {
        float avgSize = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.9f;
        Vector3 boxSize = new Vector3(avgSize, movementSettings.distanceToGround, avgSize);

        Gizmos.color = new Color(1, 0, 0, 0.7f);
        Gizmos.DrawCube(transform.position + (-transform.up * (transform.lossyScale.y + boxSize.y / 2)), boxSize);
    }
    */

    private bool Dash() {

        if (dashInput == true) {
            Vector3 targetPos = transform.position + cameraAnchor.forward * movementSettings.dashDistance;
            StartCoroutine(dashing(targetPos));
            return true;
        }

        return false;
    }

    private IEnumerator dashing(Vector3 targetPosition) {

        Debug.DrawLine(transform.position, targetPosition);

        const float INTERVAL = 0.05f;
        int steps = (int)(movementSettings.dashTime / INTERVAL);

        for (int i = 0; i < steps; i++) {
            //float iFrom0to1 = (1 / steps) * i;

            //Debug.Log(i + " " + iFrom0to1);

            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.9f);
            yield return new WaitForSeconds(INTERVAL);
        }

    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Death Zone") {
            Spawn();
        }
    }

    private void OnTriggerExit(Collider other) {
        
    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.tag == "Enemy") {
			Enemy enemy = collision.gameObject.GetComponent<Enemy>();
			Collider col = collision.gameObject.GetComponent<Collider>();
			Collider mycol = this.gameObject.GetComponent<Collider>();

			if (mycol.bounds.center.y - mycol.bounds.extents.y > col.bounds.center.y + 0.5f * col.bounds.extents.y) {

				// add enemy.bumpSpeed to player
				playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, enemy.bumpSpeed, playerRigidbody.velocity.z);

				// kill enemy
				enemy.OnDeath();
			}
		}


		if (collision.transform.GetComponent<MovePlatform>() != null) {
            //Debug.Log("collision enter: " + collision.transform.name);

            //this.transform.parent = collision.transform;

            //platformStandingOn = collision.transform;
            //colLastPos = collision.transform.position;

        }
    }


    private void OnCollisionStay(Collision collision) {
        /*
        if (collision.transform.GetComponent<MovePlatform>() != null) {
            transform.position += -colLastPos + collision.transform.position;
            colLastPos = collision.transform.position;
        }
        */
    }

    private void OnCollisionExit(Collision collision) {
        
        /*
        if (transform.IsChildOf(collision.transform)) {
            this.transform.parent = null;
        }
        transform.parent = null;
        */
        
        /*
        if (platformStandingOn != null) {
            //Debug.Log("collision exit: " + collision.transform.name);
            platformStandingOn = null;
        }
        platformStandingOn = null;
        */
    }

    private void UpdatePositionBasedOnPlatform() {
        Debug.Log(platformStandingOn);
        platformStandingOn = CheckForPlatform();



        if (platformStandingOn != null) {
            if ((-colLastPos + platformStandingOn.position).magnitude > 1) {
                colLastPos = platformStandingOn.position;
            }

            transform.position += -colLastPos + platformStandingOn.position;
            colLastPos = platformStandingOn.position;
        }
    }

    // still suboptimal!!!
    private Transform CheckForPlatform() {

        float avgSize = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.9f;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(avgSize / 2, avgSize, avgSize / 2), -transform.up, transform.rotation, movementSettings.distanceToGround, movementSettings.ground);

        Transform result = null;

        foreach (RaycastHit hit in hits) {
            if (hit.transform.GetComponent<MovePlatform>() != null) {
                result = hit.transform;
            }
        }

        return result;
    }
}

[System.Serializable]
public class MovementSettings {
    public float runVelocity = 12;

    public float rotateVelocity = 100;

    [Header("Jumping: ")]
    public float jumpVelocity = 8;
    public float distanceToGround = 1.3f;
    public LayerMask ground = 0;
    public float lowjumpForce = 10f;
    [Range(0f, 1f)]
    public float lowJumpMultiplier = 0.9f;
    public float additionalFallingForce = 2f;
    [Range(0f, 1f)]
    public float jumpingMovementSpeedMultiplier = 0.5f;
    [HideInInspector]
    public float jumpingMovementSpeed = 1;

    [Header("Dashing: ")]

    public float dashDistance = 7f;
    public float dashTime = 1f;
}

[System.Serializable]
public class InputSettings {
    public string FORWARD_AXIS = "Vertical";
    public string SIDEWAYS_AXIS = "Horizontal";
    public string TURN_AXIS_X = "Mouse X";
    public string TURN_AXIS_Y = "Mouse Y";
    public string JUMP_AXIS = "Jump";
}