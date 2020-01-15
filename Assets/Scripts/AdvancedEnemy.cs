using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AdvancedEnemy : MonoBehaviour {

	[SerializeField]
	private float ChaseSpeed;
	[SerializeField]
	private float NormalSpeed;
	[SerializeField]
	private GameObject Prey;
	private Rigidbody enemyRigidbody;

	public enum Behaviour { LineOfSight, Intercept, PatternMovement, ChasePatternMovement, Hide }
	public Behaviour behaviour;

	[SerializeField]
	private List<WayPoint> wayPoints;
	private int currentWayPoint = 0;

	[SerializeField]
	private float ChaseEvadeDistance = 10;

	private float distanceThreshold;

	void Awake() {
		enemyRigidbody = GetComponent<Rigidbody>();
	}


	private void FixedUpdate() {
		bool playerInRange = Vector3.Distance(gameObject.transform.position, Prey.transform.position) < ChaseEvadeDistance;

		switch (behaviour) {
			case Behaviour.LineOfSight: //Exercise 1#
				if (playerInRange)
					ChaseLineOfSight(Prey.transform.position, ChaseSpeed);
				break;
			case Behaviour.Intercept: //Exercise 2
				if (playerInRange)
					Intercept(Prey.transform.position);
				break;
			case Behaviour.PatternMovement: //Exercise 3
				PatternMovement();
				break;
			case Behaviour.ChasePatternMovement: //Exercise 4
				if (playerInRange) {
					ChaseLineOfSight(Prey.transform.position, ChaseSpeed);
				}
				else {
					PatternMovement();
				}
				break;
			case Behaviour.Hide: //Exercise 5
				if (PlayerVisible(Prey.transform.position)) {
					ChaseLineOfSight(Prey.transform.position, ChaseSpeed);
				}
				else {
					PatternMovement();
				}
				break;
			default:
				break;
		}
	}

	private void ChaseLineOfSight(Vector3 targetPosition, float Speed) {
		Vector3 direction = (targetPosition - transform.position).normalized;
		//direction.Normalize();

		enemyRigidbody.velocity = new Vector3(direction.x * Speed, enemyRigidbody.velocity.y, direction.z * Speed);
	}

	private void Intercept(Vector3 targetPosition) {
		Vector3 enemyPosition = gameObject.transform.position;
		Vector3 PreyPosition = Prey.transform.position;

		Vector3 VelocityRelative = Prey.GetComponent<Rigidbody>().velocity - enemyRigidbody.velocity;
		Vector3 Distance = targetPosition - enemyPosition;

		float timeToClose = Distance.magnitude / VelocityRelative.magnitude;

		Vector3 PredictedInterceptionPoint = targetPosition + (timeToClose * Prey.GetComponent<Rigidbody>().velocity);

		// move to calculated point
		Vector3 direction = PredictedInterceptionPoint - enemyPosition; direction.Normalize();

		enemyRigidbody.velocity = new Vector3(direction.x * ChaseSpeed, enemyRigidbody.velocity.y, direction.z * ChaseSpeed);
	}

	private void PatternMovement() {
		//Move towards the current waypoint.
		ChaseLineOfSight(wayPoints[currentWayPoint].transform.position, NormalSpeed);
		//Check if we are close to the next waypoint and incerement to the next waypoint.
		if (Vector3.Distance(gameObject.transform.position, wayPoints[currentWayPoint].transform.position) < distanceThreshold) {
			currentWayPoint = (currentWayPoint + 1) % wayPoints.Count; //modulo to restart at the beginning.
		}
	}

	private bool PlayerVisible(Vector3 targetPosition) {
		Vector3 directionToTarget = targetPosition - gameObject.transform.position; directionToTarget.Normalize();

		RaycastHit hit; Physics.Raycast(gameObject.transform.position, directionToTarget, out hit);


		return hit.collider.gameObject.tag.Equals("Player");
	}
}
