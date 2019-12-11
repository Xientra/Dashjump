using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour {

	public Transform spawnPoint;
	private PlayerMovement playerMovement;

	[Header("Shoot:")]
	public GameObject shootFxPrefab;

	private void Awake() {
		playerMovement = GetComponent<PlayerMovement>();
	}

	void Start() {
		Spawn();
	}

	public void Spawn() {
		transform.position = spawnPoint.position;
		transform.rotation = Quaternion.identity;
	}

	public void OnDeath() {
		Spawn();
	}


	private void Update() {
		Shoot();
	}

	private void Shoot() {
		if (Input.GetMouseButtonDown(0)) {
			GameObject shootFx = Instantiate(shootFxPrefab, transform.position, playerMovement.cameraAnchor.rotation * Quaternion.Inverse(playerMovement.playerCamera.transform.localRotation));
			Destroy(shootFx, 5f);
		}
	}

	private void OnCollisionEnter(Collision collision) {

		if (collision.gameObject.tag == "Enemy") {
			Enemy enemy = collision.gameObject.GetComponent<Enemy>();
			Collider col = collision.gameObject.GetComponent<Collider>();
			Collider mycol = this.gameObject.GetComponent<Collider>();

			if (mycol.bounds.center.y - mycol.bounds.extents.y > col.bounds.center.y + 0.5f * col.bounds.extents.y) {

				// add enemy.bumpSpeed to player
				playerMovement.performJump(enemy.bumpSpeed);

				// kill enemy
				enemy.OnDeath();
			}
			else {
				OnDeath();
			}
		}
	}
}