using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMarkerCtrl : MonoBehaviour {
	/// <summary>
	/// タッチ位置を可視化するためのマーカー
	/// </summary>
	InputManager _inputManager;

	// Use this for initialization
	void Start () {
		_inputManager = GameObject.Find("GameCtrl").GetComponent<InputManager>();
	}

	// Update is called once per frame
	void Update () {
		if (_inputManager.getIsTouching()) {
			this.transform.position = _inputManager.getWorldPosition();
		}
	}
}
