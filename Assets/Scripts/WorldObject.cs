using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour {

	public Transform originPoint;

	private void Awake() {
		if (originPoint == null) {
			originPoint = new GameObject("originPoint point of: " + gameObject.name).transform;
			originPoint.position = transform.position;
			originPoint.rotation = transform.rotation;
		}
	}

	void ReSpawn() {
		transform.position = originPoint.position;
		transform.rotation = originPoint.rotation;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Death Zone") {
			ReSpawn();
		}
	}
}