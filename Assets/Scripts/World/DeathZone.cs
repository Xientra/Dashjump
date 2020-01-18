using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    public bool allwaysDrawBoxGizmo = true;

    //The gizmos in here will always be drawn.
    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "deathzone");

        if (allwaysDrawBoxGizmo == true) {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawCube(transform.position, transform.lossyScale);
        }
    }

    // The gizmos in here will only be drawn when the object is selected.
    void OnDrawGizmosSelected() {

        if (allwaysDrawBoxGizmo == false) {
            Gizmos.color = new Color(1, 0, 0, 0.3f);
            Gizmos.DrawCube(transform.position, transform.lossyScale);
        }
    }
}