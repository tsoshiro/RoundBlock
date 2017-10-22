using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverCtrl : MonoBehaviour {
	GameManager _gameManager;

	public bool isFeverMode = false;
	float FEVER_TIME = 10f;
	float time = 0f;

	// Use this for initialization
	void Start () {
		_gameManager = this.gameObject.GetComponent<GameManager> ();
	}

	// Update is called once per frame
	void Update () {
		if (!isFeverMode)
			return;

		time -= Time.deltaTime;
		if (time <= 0) {
			finishFever ();
		}
	}

	public void startFever() {
		if (isFeverMode)
			return;

		isFeverMode = true;
		time += FEVER_TIME;

		_gameManager._ballManager.setAllBallsMagnitudeMax();

		for (int i = 0; i < _gameManager._wallManager._walls.Count; i++) {
			_gameManager._wallManager._walls [i].GetComponent<Collider> ().enabled = true;
			_gameManager._wallManager._walls [i].GetComponent<MeshRenderer>().material.color = Color.red;
		}
	}

	public void finishFever() {
		time = 0;
		isFeverMode = false;

		_gameManager._ballManager.setAllBallsMagnitudeDefault();

		for (int i = 0; i < _gameManager._wallManager._walls.Count; i++) {
			_gameManager._wallManager._walls [i].GetComponent<Collider> ().enabled = false;
			_gameManager._wallManager._walls [i].GetComponent<MeshRenderer>().material.color = Color.gray;
		}
	}	
}
