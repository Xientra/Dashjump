using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ForcePush : MonoBehaviour {

	public bool active = true;
	public float timeActive = 0.5f;

	public float falloff = 1f;
	public float force = 10;

	private Collider col;

	private void Awake() {
		col = GetComponent<Collider>();
		
	}

	private void Start() {
		StartCoroutine(Deactivate(timeActive));
	}

	private void OnTriggerEnter(Collider other) {

		if (active == true) {
			WorldObject wo = other.GetComponent<WorldObject>();
			if (wo != null) {

				wo.rb.AddExplosionForce(force, transform.position, col.bounds.extents.z * 2 * falloff);
			}
		}
	}

	IEnumerator Deactivate(float time) {
		yield return new WaitForSeconds(time);
		active = false;
	}

	/*
	private void OnDrawGizmos() {
		Debug.DrawLine(transform.position, transform.position + transform.forward * GetComponent<Collider>().bounds.extents.z * 2);
	}
	*/
}