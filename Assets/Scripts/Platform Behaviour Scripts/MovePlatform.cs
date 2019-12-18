using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour {

    [Tooltip("How long in seconds it takes to move to the target.")]
    public float time = 1;
	[Tooltip("How long the platform waits on both ends.")]
	public float waitTime = 0.5f;
    private float passedTime = 0f;
    [SerializeField]
    private float speed = 0;

    public Transform target;
    private Vector3 targetPoint;
    private Vector3 originalPoint;
	public bool moving = true;
    private bool goingToTarget = true;


    void Start() {
        originalPoint = transform.position;
    }

    void FixedUpdate() {
        Move();

        speed = (originalPoint - target.position).magnitude / time;
    }

    private void Move() {
        Debug.DrawLine(transform.position, originalPoint, Color.blue);
        Debug.DrawLine(transform.position, target.position, Color.red);
		if (moving == true) {

			if (goingToTarget == true) {
				//transform.position = Vector3.Slerp(originalPoint, target.position, passedTime / time);
				transform.position = Vector3.Lerp(originalPoint, target.position, passedTime / time);
				passedTime += Time.deltaTime;

				if (passedTime > time) {
					passedTime = 0;
					StartCoroutine(ReverseMovment());
				}
			}
			else {
				transform.position = Vector3.Lerp(target.position, originalPoint, passedTime / time);
				transform.position = Vector3.Lerp(target.position, originalPoint, passedTime / time);
				passedTime += Time.deltaTime;

				if (passedTime > time) {
					passedTime = 0;
					StartCoroutine(ReverseMovment());
				}
			}
		}
    }

	private IEnumerator ReverseMovment() {
		moving = false;

		yield return new WaitForSeconds(waitTime);

		goingToTarget = !goingToTarget;

		moving = true;
	}
}