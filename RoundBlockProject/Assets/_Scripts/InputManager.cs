using System.Collections;
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
		setTouchDown();
		setIsTouching();
		setIsReleased();
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
}