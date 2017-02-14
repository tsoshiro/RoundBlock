using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketCtrl : MonoBehaviour {
	// Top, Left, Rightのラケット 
	public List<Rigidbody> otherRackets;

	enum OtherRacketPosition {
		TOP,
		LEFT,
		RIGHT
	}
	Rigidbody rig;

	#region INPUT
	public bool isVelocity = false;

	Vector3 inputVector;
	#endregion

	#region MOVE LOGIC
	public float speed = 100;

	// 移動可能範囲
	float range_x;
	float range_z;

	// ステージサイズ : 移動可能範囲算出用
	public GameObject _wallHorizontal;
	public GameObject _wallVertical;

	public float STAGE_WIDTH = 10f;
	public float STAGE_HEIGHT = 22f;
	float height_width_rate;

	// ラケットの幅 : localScale
	float verticalRacketWidth;
	float horizontalRacketWidth;

	// 基本ラケットと、その他ラケットの位置差分
	float x_diff_horizontal;
	float z_diff_horizontal;
	float z_diff_vertical;
	#endregion


	// Use this for initialization
	void Start () {
		_inputManager = GameObject.Find("GameCtrl").GetComponent<InputManager>();

		Init();
	}

	void Init() {
		rig = this.GetComponent<Rigidbody>();

		STAGE_WIDTH = _wallHorizontal.transform.localScale.x;
		STAGE_HEIGHT = _wallVertical.transform.localScale.x;
		height_width_rate = STAGE_HEIGHT / STAGE_WIDTH;

		horizontalRacketWidth = this.transform.localScale.x;
		verticalRacketWidth = otherRackets[(int)OtherRacketPosition.RIGHT].transform.localScale.x;

		// ステージ幅の半分 - ラケットの半分 = 移動可能範囲の絶対値
		//range_x = STAGE_WIDTH * 0.5f - horizontalRacketWidth * 0.5f;
		//range_z = STAGE_HEIGHT * 0.5f - verticalRacketWidth * 0.5f;

		range_x = STAGE_WIDTH * 0.5f;
		range_z = STAGE_HEIGHT * 0.5f;

		x_diff_horizontal = otherRackets[(int)OtherRacketPosition.RIGHT].transform.position.x - rig.transform.position.x;
		z_diff_horizontal = otherRackets[(int)OtherRacketPosition.RIGHT].transform.position.z - rig.transform.position.z;
		z_diff_vertical = otherRackets[(int)OtherRacketPosition.TOP].transform.position.z - rig.transform.position.z;
	}

	// Update is called once per frame
	void Update () {
		GetTouchPosition();
	}

	void FixedUpdate() {
		Move();
	}

	void Move() {
		rig.AddForce(inputVector, ForceMode.Impulse);
	}


	InputManager _inputManager;
	void GetTouchPosition() {
		if (_inputManager.getIsTouching()) {
			Vector3 worldPos = _inputManager.getWorldPosition();

			// MOVE VELOCITY
			if (isVelocity)
			{
				MoveWithVelocity(worldPos);
			}
			else {
				MoveWithTransform(worldPos);
			}
		}
	}

	// Transformの方が適切な動きする。
	void MoveWithTransform(Vector3 worldPos) {
		Vector3 target = new Vector3(worldPos.x, 0, this.transform.position.z);
		if (target.x <= -range_x) {
			target.x = -range_x;
		} else if (target.x >= range_x) {
			target.x = range_x;
		}

		this.transform.position = target;

		MoveOtherRackets(target);
	}

	void MoveWithVelocity(Vector3 worldPos) {
		Vector3 target = new Vector3(worldPos.x, 0, 2f); // 決め打ちにしてみる
		if (target.x <= -range_x) {
			target.x = -range_x;
		} else if (target.x >= range_x) {
			target.x = range_x;
		}

		Vector3 desiredVelocity = (target - transform.position).normalized * speed;

		float sqrMag = (target - transform.position).sqrMagnitude;
		this.GetComponent<Rigidbody>().velocity = desiredVelocity;

		MoveOtherRackets(desiredVelocity);
	}

	void MoveOtherRackets(Vector3 pVelocity) {
		for (int i = 0; i < otherRackets.Count; i++) {
			if (!otherRackets[i].gameObject.activeSelf) {
				return;
			}
			Vector3 v = exchangeVector(pVelocity, (OtherRacketPosition)i, isVelocity);
			if (isVelocity)	{
				otherRackets[i].velocity = v;
			} else {
				otherRackets[i].transform.position = v;
			}
		}
	}

	Vector3 exchangeVector(Vector3 pVelocity, OtherRacketPosition pRacketPos, bool pIsVelocity = true) {
		Vector3 v = pVelocity;

		if (pIsVelocity) {
			v = exchangeVelocityVector(v, pRacketPos);
		} else {
			v = exchangeTransformVector(v, pRacketPos);
		}

		return v;
	}

	// Transform
	Vector3 exchangeTransformVector(Vector3 pVelocity, OtherRacketPosition pRacketPos) {
		Vector3 v = pVelocity;

		// 左右反転
		if (pRacketPos == OtherRacketPosition.TOP)
		{
			v.x = - v.x;
			v.z += z_diff_vertical;
		} else if (pRacketPos == OtherRacketPosition.LEFT) {
			// XとZを交代
			float tmp = - (v.x - x_diff_horizontal) * STAGE_HEIGHT / STAGE_WIDTH;
			v.x = - x_diff_horizontal;
			v.z = v.z - z_diff_horizontal + tmp + STAGE_HEIGHT / 2;

		} else { // RIGHT
			float tmp = (v.x - x_diff_horizontal) * STAGE_HEIGHT / STAGE_WIDTH;
			v.x = x_diff_horizontal;
			v.z = z_diff_horizontal + tmp + STAGE_HEIGHT / 2;
		}

		return v;
	}

	// Velocity
	Vector3 exchangeVelocityVector(Vector3 pVelocity, OtherRacketPosition pRacketPos) {
		Vector3 v = pVelocity;

		if (pRacketPos == OtherRacketPosition.TOP) {
			v.x = -v.x;
		} else if (pRacketPos == OtherRacketPosition.LEFT) {
			float tmp = -v.x * STAGE_HEIGHT / STAGE_WIDTH;
			v.x = v.z;
			v.z = tmp;
		} else {
			float tmp = v.x * STAGE_HEIGHT / STAGE_WIDTH;
			v.x = v.z;
			v.z = tmp;
		}
		return v;
	}

	#region DEBUG
	public void setIsVelocity(bool pFlg) {
		isVelocity = pFlg;
	}
	#endregion
}