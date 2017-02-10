using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketCtrl : MonoBehaviour {
	public float accel = 1000.0f;

	// Top, Left, Rightのラケット 
	public List<Rigidbody> otherRackets;

	enum OtherRacketPosition {
		TOP,
		LEFT,
		RIGHT
	}
	Rigidbody rig;

	#region INPUT
	public GameObject _touchPositionMarker;
	public float Z_OFFSET = 7.5f;
	public bool isVelocity = false;

	Vector3 inputVector;
	#endregion

	#region MOVE LOGIC
	public float speed = 20;

	// 移動可能範囲
	float range_x;
	float range_z;
	float rangeRate_z;

	// ステージサイズ : 移動可能範囲算出用
	public GameObject _wallHorizontal;
	public GameObject _wallVertical;

	public float STAGE_WIDTH = 10f;
	public float STAGE_HEIGHT = 22f;

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
		Init();
	}

	void Init() {
		rig = this.GetComponent<Rigidbody>();

		STAGE_WIDTH = _wallHorizontal.transform.localScale.x;
		STAGE_HEIGHT = _wallVertical.transform.localScale.x;

		horizontalRacketWidth = this.transform.localScale.x;
		verticalRacketWidth = otherRackets[(int)OtherRacketPosition.RIGHT].transform.localScale.x;

		// ステージ幅の半分 - ラケットサイズの半分　= 移動可能範囲の絶対値
		range_x = STAGE_WIDTH * 0.5f - horizontalRacketWidth * 0.5f;
		range_z = STAGE_HEIGHT * 0.5f - verticalRacketWidth * 0.5f;
		rangeRate_z = range_z / range_x;

		x_diff_horizontal = otherRackets[(int)OtherRacketPosition.RIGHT].transform.position.x - rig.transform.position.x;
		z_diff_horizontal = otherRackets[(int)OtherRacketPosition.RIGHT].transform.position.z - rig.transform.position.z;
		z_diff_vertical = otherRackets[(int)OtherRacketPosition.TOP].transform.position.z - rig.transform.position.z;
	}

	// Update is called once per frame
	void Update () {
		GetInputVector();

		GetTouchPosition();
	}

	void FixedUpdate() {
		Move();
	}

	void GetInputVector() {
		inputVector = transform.right * Input.GetAxisRaw("Horizontal") * accel;
	}

	void Move() {
		rig.AddForce(inputVector, ForceMode.Impulse);
	}

	void GetTouchPosition() {
		if (Input.GetMouseButton(0)) {
			Vector3 screenPos = Input.mousePosition;
			screenPos.z = Z_OFFSET; // オブジェクトのy座標固定位置をここで指定
			Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
			_touchPositionMarker.transform.position = worldPos;

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
		Vector3 target = new Vector3(worldPos.x, 0, worldPos.z);
		if (target.x <= -range_x) {
			target.x = -range_x;
		} else if (target.x >= range_x) {
			target.x = range_x;
		}

		float lastSqrMgr = Mathf.Infinity;
		Vector3 desiredVelocity = (target - transform.position).normalized * speed;

		float sqrMag = 0f;
		sqrMag = (target - transform.position).sqrMagnitude;
		if (sqrMag > lastSqrMgr) {
			desiredVelocity = Vector3.zero;
		}
		lastSqrMgr = sqrMag;
		this.GetComponent<Rigidbody>().velocity = desiredVelocity;

		MoveOtherRackets(desiredVelocity);
	}

	void MoveOtherRackets(Vector3 pVelocity) {
		for (int i = 0; i < otherRackets.Count; i++) {
			Vector3 v = exchangeVector(pVelocity, (OtherRacketPosition)i);
			if (isVelocity)
			{
				otherRackets[i].velocity = v;
			} else {
				otherRackets[i].transform.position = v;
			}
		}
	}

	Vector3 exchangeVector(Vector3 pVelocity, OtherRacketPosition pRacketPos) {
		Vector3 v = pVelocity;

		// 左右反転
		if (pRacketPos == OtherRacketPosition.TOP)
		{
			v.x = - v.x;
			v.z += z_diff_vertical;
		} 

		else if (pRacketPos == OtherRacketPosition.LEFT) {
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
}