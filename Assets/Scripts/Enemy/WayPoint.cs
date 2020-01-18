using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
	public void OnDrawGizmos() { 
		Gizmos.DrawSphere(gameObject.transform.position, 1f); 
	}
}
