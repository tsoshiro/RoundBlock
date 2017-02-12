using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
	/// <summary>
	/// Input処理を集約するクラス
	/// </summary>

	public float Z_OFFSET;

	Vector3 screenPosition;
	Vector3 worldPosition;
	bool isTouching;

	void Update() {
		if (Input.GetMouseButton(0)) {
			isTouching = true;
			screenPosition = Input.mousePosition;

			screenPosition.z = Z_OFFSET; // オブジェクトのy座標固定位置をここで指定

			worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
			return;
		}
		isTouching = false;
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
}