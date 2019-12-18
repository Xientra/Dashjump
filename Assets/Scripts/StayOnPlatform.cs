using UnityEngine;

public class StayOnPlatform : MonoBehaviour {

	public float distanceToGround = 1.3f;
	public LayerMask ground = 0;

	public bool localRotation = false;

	private Transform platformStandingOn;
	private Vector3 colLastPos;

	private void Awake() {
		//ground = LayerMask.NameToLayer("Ground");
	}

	private void FixedUpdate() {
		UpdatePositionBasedOnPlatform();
	}

	public void UpdatePositionBasedOnPlatform() {
		platformStandingOn = CheckForPlatform();


		if (platformStandingOn != null) {
			if ((-colLastPos + platformStandingOn.position).magnitude > 1) {
				colLastPos = platformStandingOn.position;
			}

			transform.position += -colLastPos + platformStandingOn.position;
			colLastPos = platformStandingOn.position;
		}
	}

	private Transform CheckForPlatform() {
		Vector3 downDirection = Vector3.down;
		if (localRotation)
			downDirection = -transform.up;


		float avgSize = ((transform.lossyScale.x + transform.lossyScale.z) / 2) * 0.9f;
		RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(avgSize / 2, avgSize, avgSize / 2), downDirection, transform.rotation, distanceToGround, ground);

		Transform result = null;

		foreach (RaycastHit hit in hits) {
			if (hit.transform.GetComponent<MovePlatform>() != null) {
				result = hit.transform;
			}
		}

		return result;
	}
}