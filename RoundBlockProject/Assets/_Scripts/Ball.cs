using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public GameManager _gameManager;

	public float VEC_X = 5f;
	public float VEC_Z = 5f;

	public float magnitude = 20f;
	public float angleAdjustValue = 100f;

	Rigidbody rigid;
	Vector3 lastVelocity;
	Vector3 defaultPosition;
	InputManager _inputManager;

	public enum BallState {
		DEFAULT,
		HARD
	}

	public BallState myBallState;
	public float MAX_HARD_TIME = 10f;
	public float hardTime = 0f;
	Collider _collider;

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
		_collider = gameObject.GetComponent<Collider> ();

		myBallState = BallState.DEFAULT;

		defaultPosition = this.transform.position;
		setWallPositions();

		rigid = this.GetComponent<Rigidbody>();
	}

	// 追加ボールの時、ドロップ時の戻り先をセットしておく
	public void InitAddBall(GameObject pParent) {
		defaultPosition = pParent.transform.position;
		setIsMain(false);
		setIsMoving (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (myBallState == BallState.HARD) {
			// count hard time
			setTriggerWhenHard ();
			countHardTimer();
		}

		if (isMain) {
			if (!isMoving) { // メイン玉のみ、動いていなければ操作が入る
				UpdateStaying();
				return;
			}
		}

		if (isMoving) {
			// Mainも非メインも
			UpdateMoving ();
		}

		// LastVelocityを更新
		lastVelocity = rigid.velocity;

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
			setIsMoving (true);
		}
	}

	void UpdateMoving() {
		resetPositionWhenOut();
	}

	#region OUT_JUDGE
	void setStartPosition() {
		Vector3 pos = defaultPosition;
		if (isMain) {
			pos.x = _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.BOTTOM].gameObject.transform.position.x;
		}
		this.transform.position = pos;
		if (!isMain) {
			this.gameObject.SetActive (false);
		}
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

		setIsMoving (false);
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

		if (pGameObject.name == "Racket_Bottom" || pGameObject.name == "Racket_Top")
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

	// Stateの設定
	public void setState(int pInt) {
		myBallState = (BallState)pInt;
		Color color;

		switch (myBallState) {
		case BallState.DEFAULT:
			// Mainかどうかでデフォルトの色が変わる
			color = (isMain) ? Color.yellow : Color.white;
			_collider.isTrigger = false;
			break;
		case BallState.HARD:
			color = Color.blue;
			hardTime = MAX_HARD_TIME;
			break;
		default:
			// Mainかどうかでデフォルトの色が変わる
			color = (isMain) ? Color.yellow : Color.white;
			_collider.isTrigger = false;
			break;
		}
		setColor (color);
	}

	void OnCollisionEnter(Collision pCollision) {
		if (pCollision.gameObject.tag == "Racket") {
			HitRacket(pCollision.gameObject);
		}
	}

	void OnTriggerEnter(Collider pCollider) {
		// HARD以外ならTrigger無効
		if (myBallState != BallState.HARD)
			return;
		if (pCollider.gameObject.tag == "Block") {
			pCollider.gameObject.GetComponent<Block> ().HitBy(this);
		}
	}

	#region MAIN/SUB SWITCHER
	public void setIsMain(bool pFlg) {
		isMain = pFlg;

		// Color
		Color color = (isMain) ? Color.yellow : Color.white;
		setColor(color);
	}

	public void setIsMoving(bool pFlg) {
		isMoving = pFlg;
	}

	void setColor(Color pColor) {
		this.GetComponent<MeshRenderer>().material.color = pColor;
	}
	#endregion

	public bool getIsMoving() {
		return isMoving;
	}

	public bool getIsMain() {
		return isMain;
	}

	public Vector3 getLastVelocity() {
		return lastVelocity;
	}

	#region HARD TYPE
	void countHardTimer() {
		hardTime -= Time.deltaTime;
		if (hardTime <= 0) {
			hardTime = 0;
			setState ((int)BallState.DEFAULT);
		}
	}
		
	/// <summary>
	/// HARD時、ボールがラケットに近い時はTriggerをOFFにする
	/// </summary>
	void setTriggerWhenHard() {
		if (_gameManager._blockManager.checkIsNearRacket(this)) {
			_collider.isTrigger = false;
		} else {
			_collider.isTrigger = true;
		}
	}
	#endregion

	#region DEBUG MODE
	public void addMagnitudeValue(float pFloat) {
		magnitude += pFloat;
	}

	#endregion
}