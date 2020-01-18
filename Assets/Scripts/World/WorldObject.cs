using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WorldObject : MonoBehaviour {

	[HideInInspector]
	public Rigidbody rb;


	public Transform originPoint;
	public float velocityMultiplierOnReSpawn = 0.5f;


	private void Awake() {
		rb = GetComponent<Rigidbody>();

		if (originPoint == null) {
			originPoint = new GameObject("originPoint point of: " + gameObject.name).transform;
			originPoint.position = transform.position;
			originPoint.rotation = transform.rotation;
			originPoint.SetParent(this.transform.parent);
		}
	}

	void ReSpawn() {
		transform.position = originPoint.position;
		transform.rotation = originPoint.rotation;

		rb.velocity *= velocityMultiplierOnReSpawn;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Death Zone") {
			ReSpawn();
		}
	}
}