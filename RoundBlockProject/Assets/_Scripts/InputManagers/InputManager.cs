﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	/// <summary>
	/// Input処理を集約するクラス
	/// </summary>

	public float Z_OFFSET;

	// タッチ位置を返す
	Vector3 screenPosition; // スクリーン座標
	Vector3 worldPosition;	// ワールド座標

	// タッチ情報を返す
	bool isTouching;
	bool isTapped;
	bool isReleased;

	void Update() {
		if (Input.GetMouseButtonDown (0) ||
		    Input.GetMouseButton (0) ||
		    Input.GetMouseButtonUp (0)) {
			setTouchDown ();
			setIsTouching ();
			setIsReleased ();
		} else {
			keyControl ();
		}
	}

	void setTouchDown() {
		if (Input.GetMouseButtonDown(0)) {
			isTapped = true;
			return;
		}
		isTapped = false;
	}

	void setIsTouching() {
		if (Input.GetMouseButton(0)) {
			isTouching = true;
			screenPosition = Input.mousePosition;

			screenPosition.z = Z_OFFSET; // オブジェクトのy座標固定位置をここで指定

			worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			return;
		}
		isTouching = false;
	}

	void setIsReleased() {
		if (Input.GetMouseButtonUp(0)) {
			isReleased = true;
			return;
		}
		isReleased = false;
	}

	public Vector3 getScreenPosition() {
		return screenPosition;
	}

	public Vector3 getWorldPosition() {
		return worldPosition;
	}

	public bool getIsTouching() {
		return isTouching;
	}

	public bool getIsTapped() {
		return isTapped;
	}

	public bool getIsReleased() {
		return isReleased;
	}

	#region Keyboard Control
	[SerializeField]
	float KEY_SPEED = .5f;

	void keyControl() {
		setKeyDown ();
		setIsKey ();
		setIsKeyUp ();
	}

	void setKeyDown() {
		if (Input.GetKeyDown ("right") ||
			Input.GetKeyDown ("left") ||
			Input.GetKeyDown ("space")) {
			isTapped = true;
			return;
		}
		isTapped = false;
	}

	void setIsKey() {
		isTouching = true;
		if (Input.GetKey ("right")) {
			worldPosition.x += KEY_SPEED;
		} else if (Input.GetKey ("left")) {
			worldPosition.x -= KEY_SPEED;
		} else {
			isTouching = false;
		}
	}

	void setIsKeyUp() {
		if (Input.GetKeyDown ("right") ||
			Input.GetKeyDown ("left") ||
			Input.GetKeyDown ("space")) {
			isReleased = true;
			return;
		}
		isReleased = false;
	}

	#endregion
}