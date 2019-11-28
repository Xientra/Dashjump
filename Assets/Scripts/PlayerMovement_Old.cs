using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Old : MonoBehaviour {

	public Camera playerCamera;
	private Transform cameraAnchor;

	[SerializeField]
	private Vector3 speed;

	private Rigidbody rb;

	public PlayerSettings playerSettings;

	void Start() {
		rb = GetComponent<Rigidbody>();
		cameraAnchor = playerCamera.transform.parent;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		CameraMovement();
		CharacterMovement();
		CharacterJump();
	}

	void CameraMovement() {
		cameraAnchor.position = transform.position;

		cameraAnchor.Rotate(new Vector3(-Input.GetAxis("Mouse Y")  * playerSettings.rotationSpeed * Time.deltaTime, Input.GetAxis("Mouse X") * playerSettings.rotationSpeed * Time.deltaTime, 0));

		float xRot = cameraAnchor.rotation.eulerAngles.x;
		//Debug.Log(xRot);
		//if (xRot > playerSettings.maxRotation) xRot = playerSettings.maxRotation;
		//if (xRot < playerSettings.minRotation) xRot = playerSettings.minRotation;
		//xRot = Mathf.Clamp(xRot, playerSettings.minRotation, playerSettings.maxRotation);
		cameraAnchor.rotation = Quaternion.Euler(xRot, cameraAnchor.rotation.eulerAngles.y, 0);
	}

	void CharacterMovement() {

		if (Input.GetKey(KeyCode.W)) {
			speed = Vector3.Lerp(speed, new Vector3(speed.x, speed.y, playerSettings.movementSpeed), playerSettings.accelerationSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.S)) {
			speed = Vector3.Lerp(speed, new Vector3(speed.x, speed.y, -playerSettings.movementSpeed), playerSettings.accelerationSpeed * Time.deltaTime);
		}
		else {
			speed = Vector3.Lerp(speed, new Vector3(speed.x, speed.y, 0), playerSettings.decelerationSpeed * Time.deltaTime);
		}

		if (Input.GetKey(KeyCode.A)) {
			speed = Vector3.Lerp(speed, new Vector3(-playerSettings.movementSpeed, speed.y, speed.z), playerSettings.accelerationSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.D)) {
			speed = Vector3.Lerp(speed, new Vector3(playerSettings.movementSpeed, speed.y, speed.z), playerSettings.accelerationSpeed * Time.deltaTime);
		}
		else {
			speed = Vector3.Lerp(speed, new Vector3(0, speed.y, speed.z), playerSettings.decelerationSpeed * Time.deltaTime);
		}

		//Vector3 vel = new Vector3(cameraAnchor.right * speed.x, rb.velocity.y, cameraAnchor.forward * speed.z);
		Vector3 newVel = Quaternion.Euler(0, cameraAnchor.eulerAngles.y, 0) * speed;

		rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z);
	}

	void CharacterJump() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			rb.velocity += new Vector3(0, playerSettings.jumpStrength, 0);
		}
	}
}

[System.Serializable]
public class PlayerSettings {
	public float movementSpeed = 10f;
	public float accelerationSpeed = 10f;
	public float decelerationSpeed = 15f;

	[Space]

	public float jumpStrength = 25f;

	[Space]

	public float rotationSpeed = 10f;
	public float maxRotation = 60;
	public float minRotation = -20;
}