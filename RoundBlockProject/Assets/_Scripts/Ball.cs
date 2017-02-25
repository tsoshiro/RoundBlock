﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public GameManager _gameManager;

	public float VEC_X = 5f;
	public float VEC_Z = 5f;

	public float magnitude = 20f;
	public float angleAdjustValue = 100f;

	Rigidbody rigid;
	Vector3 defaultPosition;
	InputManager _inputManager;

	public enum BallState {
		DEFAULT,
		HARD
	}

	public BallState myBallState;


	// リセット位置
	public List<GameObject> _walls;
	List<Vector3> _wallPositions;
	public RacketCtrl _racketCtrl;
	bool isMoving = false;

	// メイン OR サブ
	bool isMain = true;

	// Use this for initialization
	void Start () {
		GameObject _gameCtrl = GameObject.Find("GameCtrl");
		_inputManager = _gameCtrl.GetComponent<InputManager>();
		_gameManager = _gameCtrl.GetComponent<GameManager>();

		myBallState = BallState.DEFAULT;

		defaultPosition = this.transform.position;
		setWallPositions();

		rigid = this.GetComponent<Rigidbody>();
	}

	// 追加ボールの時、ドロップ時の戻り先をセットしておく
	public void InitAddBall(GameObject pParent) {
		defaultPosition = pParent.transform.position;
		isMain = false;
	}
	
	// Update is called once per frame
	void Update () {
		// 共通
		if (isMoving) {
			UpdateMoving();
			return;
		}

		// メイン玉のみ操作が入る
		if (isMain) {
			UpdateStaying();
		}
	}

	void UpdateStaying() {
		if (_inputManager.getIsTouching()) {
			setStartPosition();
			return;
		}
		if (_inputManager.getIsReleased()) {
			if (_gameManager.state == GameManager.GameState.PAUSE) {
				return;
			}
			if (_gameManager.state == GameManager.GameState.RESULT) {
				_gameManager.resetGame();
				return;
			}
			if (_gameManager.state == GameManager.GameState.READY) {
				// ゲーム開始
				_gameManager.startGame();
			}

			Shot();
			isMoving = true;
		}
	}

	void UpdateMoving() {
		resetPositionWhenOut();
	}

	#region OUT_JUDGE
	void setStartPosition() {
		Vector3 pos = defaultPosition;
		pos.x = _racketCtrl.gameObject.transform.position.x;
		this.transform.position = pos;
	}

	void setWallPositions() {
		_wallPositions = new List<Vector3>();
		for (int i = 0; i < _walls.Count; i++) {
			_wallPositions.Add(_walls[i].transform.position);
		}
	}

	void resetPositionWhenOut() {
		if (checkIsOut()) {
			resetBall();
		}
	}

	void resetBall() {
		resetPosition();
		setState((int)BallState.DEFAULT);

		// Main時のみペナルティ
		if (isMain) {
			_gameManager.dropBall();
		}
	}

	public void resetPosition() {
		setStartPosition();
		rigid.velocity = Vector3.zero;

		isMoving = false;
	}

	bool checkIsOut() {
		bool flg;
		for (int i = 0; i < _wallPositions.Count; i++) {
			flg = checkIsOver(i);
			if (flg) {
				return flg;
			}
		}
		return false;
	}

	bool checkIsOver(int pWallPosition) {
		if (pWallPosition == (int)Const.WallPosition.TOP) {
			return checkIsUpThanZ(_wallPositions[pWallPosition].z);
		} else if (pWallPosition == (int)Const.WallPosition.RIGHT) {
			return checkIsRightThanX(_wallPositions[pWallPosition].x);
		} else if (pWallPosition == (int)Const.WallPosition.BOTTOM) {
			return checkIsDownThanZ(_wallPositions[pWallPosition].x);
		} else if (pWallPosition == (int)Const.WallPosition.LEFT) {
			return checkIsLeftThanX(_wallPositions[pWallPosition].x);
		}
		return false;
	}

	bool checkIsRightThanX(float pX) {
		if (this.transform.position.x >= pX) {
			return true;
		}
		return false;
	}

	bool checkIsLeftThanX(float pX) {
		if (this.transform.position.x <= pX)
		{
			return true;
		}
		return false;
	}

	bool checkIsUpThanZ(float pZ) {
		if (this.transform.position.z >= pZ) {
			return true;
		}
		return false;
	}

	bool checkIsDownThanZ(float pZ) {
		if (this.transform.position.z <= pZ) {
			return true;
		}
		return false;	
	}
	#endregion

	void Shot() {
		Vector3 v = transform.forward * 2 + transform.right;
		setMagnitude(v);

		return;

		//rigid.AddForce(
		//	(transform.forward + transform.right) * magnitude,
		//	ForceMode.VelocityChange);
	}

	void FixedUpdate() {
		//checkVelocity();
	}

	void setMagnitude(Vector3 v) {
		v.Normalize();

		rigid.velocity = v * magnitude;
	}

	void checkVelocity() {
		Vector3 v = rigid.velocity;

		if (- VEC_X < v.x && v.x <= 0)	{
			v.x = - VEC_X;
		}
		else if (VEC_X > v.x && v.x >= 0) {
			v.x = VEC_X;
		}

		if (- VEC_Z < v.z && v.z <= 0){
			v.z = - VEC_Z;
		}
		else if (VEC_Z > v.z && v.z >= 0) {
			v.z = VEC_Z;
		}

		// 平均化
		setMagnitude(v);
	}

	void HitRacket(GameObject pGameObject) {
		float distanceFromCenter = 0;
		float velocityValue = 0;

		Vector3 v;
		float width = pGameObject.transform.localScale.x;

		if (pGameObject.name == "Racket" || pGameObject.name == "Racket_Top")
		{
			// HORIZONTAL
			distanceFromCenter = this.transform.position.x - pGameObject.transform.position.x;

			velocityValue = distanceFromCenter * width;

			v = new Vector3(velocityValue, rigid.velocity.y, rigid.velocity.z);
		} else {
			// VERTICAL
			distanceFromCenter = this.transform.position.z - pGameObject.transform.position.z;

			velocityValue = distanceFromCenter * width;

			v = new Vector3(rigid.velocity.x, rigid.velocity.y, velocityValue);
		}

		// 平均化
		setMagnitude(v);
	}

	public void setState(int pInt) {
		myBallState = (BallState)pInt;
	}

	void OnCollisionEnter(Collision pCollision) {
		if (pCollision.gameObject.tag == "Racket") {
			HitRacket(pCollision.gameObject);
		}
	}

	#region MAIN/SUB SWITCHER
	public void setIsMain(bool pFlg) {
		isMain = pFlg;
	}
	#endregion

	public bool getIsMoving() {
		return isMoving;
	}

	public bool getIsMain() {
		return isMain;
	}

	#region DEBUG MODE
	public void addMagnitudeValue(float pFloat) {
		magnitude += pFloat;
	}

	#endregion
}