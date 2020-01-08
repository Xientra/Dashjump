using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Enemy : MonoBehaviour {


    public float speed; 
	Rigidbody enemyRigidbody;
	[Tooltip("When the player jumps onto the Enemy the player jump force * this value will be applied.")]
	public float bumpSpeed = 1f;

	Collider col;

    void Awake() { 
        enemyRigidbody = gameObject.GetComponent<Rigidbody>();
		col = GetComponent<Collider>();
    }


    void FixedUpdate() { 
        enemyRigidbody.velocity = new Vector3(speed, enemyRigidbody.velocity.y, 0);

		
		if (speed != 0)
			CheckPlatform();
    }

	public void OnDeath() {
		Destroy(this.gameObject);
	}

	private void CheckPlatform() {

		Vector3 center = col.bounds.center;
		Vector3 extents = col.bounds.extents;
		Vector3 size = col.bounds.size;

		// forward
		//if (Physics.Raycast(center + transform.forward * extents.z * 1.1f, -transform.up, size.y, LayerMask.NameToLayer("Ground"))) {
		//	//enemyRigidbody.velocity = Vector3.Reflect(enemyRigidbody.velocity, transform.forward);
		//	Debug.Log("forward");
		//}
		//// back
		//if (Physics.Raycast(center + -transform.forward * extents.z * 1.1f, -transform.up, size.y, LayerMask.NameToLayer("Ground"))) {
		//	//enemyRigidbody.velocity = Vector3.Reflect(enemyRigidbody.velocity, -transform.forward);
		//	Debug.Log("-forward");
		//}
		//// right
		//if (Physics.Raycast(center + transform.right * extents.x * 1.1f, -transform.up, size.y, LayerMask.NameToLayer("Ground"))) {
		//	//enemyRigidbody.velocity = Vector3.Reflect(enemyRigidbody.velocity, transform.right);
		//	Debug.Log("right");
		//}
		//// left	
		//if (Physics.Raycast(center + -transform.right * extents.x * 1.1f, -transform.up, size.y, LayerMask.NameToLayer("Ground"))) {
		//	//enemyRigidbody.velocity = Vector3.Reflect(enemyRigidbody.velocity, -transform.right);
		//	Debug.Log("-right");
		//}

		RaycastHit hitInfo;
		Physics.Raycast(center, -transform.up, out hitInfo, size.y, 8);

		Debug.Log(hitInfo.collider.name);


		//Vector3 startPoint = center + -transform.right * extents.x * 1.1f;

		//Debug.DrawLine(startPoint, startPoint + -transform.up * size.y, Color.magenta);
	}
}