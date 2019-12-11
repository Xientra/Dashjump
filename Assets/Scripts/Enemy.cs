using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {


    public float speed; Rigidbody enemyRigidbody;
	[Tooltip("When the player jumps onto the Enemy the player jump force * this value will be applied.")]
	public float bumpSpeed = 1f;

    void Awake() { 
        enemyRigidbody = gameObject.GetComponent<Rigidbody>(); 
    }


    void FixedUpdate() { 
        enemyRigidbody.velocity = new Vector3(speed, enemyRigidbody.velocity.y, 0); 
    }

	public void OnDeath() {
		Destroy(this.gameObject);
	}
}