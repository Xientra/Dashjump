using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController instance;

	
	private GameController() {
		if (instance == null) {
			instance = this;
		}
	}

	public static GameController Instance {
		get {
			if (instance == null)
				instance = new GameController();
			return instance;
		}
	}
	

	void Start() {
		
	}

	void Update() {

	}
}
