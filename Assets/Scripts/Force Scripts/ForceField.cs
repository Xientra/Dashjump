using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ForceField : MonoBehaviour {

	private Collider col;
	public new ParticleSystem particleSystem;



	public bool active = true;
	public float force = 10;
	public AnimationCurve falloff;
	public AnimationCurve dragFalloff;

	public float inFieldDragAddition = 3f;
	[Range(0, 5)]
	public float inFieldAngularDragAddition = 1f;

	[Range(0f, 1f)]
	public float positionChangeSpeed = 0.1f;

	[HideInInspector]
	public Vector3 desiredPosition;

	private void Awake() {
		col = GetComponent<Collider>();
	}

	private void Start() {
		if (particleSystem == null)
			particleSystem = GetComponent<ParticleSystem>();
		if (particleSystem == null)
			particleSystem = GetComponentInChildren<ParticleSystem>();

		desiredPosition = transform.position;
	}

	private void FixedUpdate() {
		if (active)
			transform.position = Vector3.Lerp(transform.position, desiredPosition, positionChangeSpeed);
	}

	private void OnTriggerStay(Collider other) {

		if (active == true) {
			WorldObject wo = other.GetComponent<WorldObject>();
			if (wo != null) {

				//Debug.Log(col.bounds.extents.z);
				//Debug.Log((-other.transform.position + transform.position).magnitude / col.bounds.extents.z);

				//Debug.DrawLine(transform.position, other.transform.position, Color.red);
				//Debug.DrawLine(transform.position, transform.position + (-other.transform.position + transform.position).normalized * (1 / col.bounds.extents.z), Color.blue);
				//wo.rb.AddForce((-other.transform.position + transform.position).normalized * force * falloff.Evaluate((1 / col.bounds.extents.z) * (-other.transform.position + transform.position).magnitude));
				wo.rb.AddForce((-other.transform.position + transform.position).normalized * force);

				wo.rb.velocity *= (1 - Time.deltaTime * inFieldDragAddition);
			}
		}
	}
}