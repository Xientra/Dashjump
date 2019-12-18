using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ForceField : MonoBehaviour {

	private Collider col;


	public bool active = true;
	public float force = 10;
	public float falloff = 1f;
	

	private void Awake() {
		col = GetComponent<Collider>();

	}

	private void Start() {
		
	}

	private void OnTriggerStay(Collider other) {

		if (active == true) {
			WorldObject wo = other.GetComponent<WorldObject>();
			if (wo != null) {
				wo.rb.AddForce((-other.transform.position + transform.position).normalized * force);
				//wo.rb.AddExplosionForce(force, transform.position, col.bounds.extents.z * 2 * falloff);
			}
		}
	}
}