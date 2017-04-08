using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour {
	public List<Ball> _balls = new List<Ball>();

	public Ball _mainBall;
	public GameObject _ballPrefab;
	int n = 5; // Ball Prefabの値
	int counter = 0;

	float magnitudeDefault = 0;

	// Use this for initialization
	void Start () {
		Init();
	}

	void Init() {
		// Ballを複製してプールする
		for (int i = 0; i < n; i++) {
			GameObject obj = (GameObject)Instantiate(_ballPrefab,
			                                         _ballPrefab.transform.position,
			                                         _ballPrefab.transform.rotation);
			obj.name = _ballPrefab.name + "_" + i.ToString ("D2");
			obj.transform.SetParent(this.transform);
			obj.transform.localPosition = new Vector3(0, 0, 0);
			_balls.Add(obj.GetComponent<Ball>());
			obj.SetActive(false);
		}

		magnitudeDefault = _mainBall.magnitude;
	}

	/// <summary>
	/// 追加用のボールがあるか確認する。
	/// </summary>
	/// <returns>追加用のボールがあれば<c>true</c>,なければ <c>false</c></returns>
	bool checkIsBallAvailable() {
		// 動いていないボールがひとつでもあればtrue
		for (int i = 0; i < n; i++) {
			if (!_balls[i].getIsMoving()) {
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// ボールを追加する。
	/// </summary>
	/// <param name="pBall">ヒットしたボール</param>
	/// <param name="pBlock">ヒットしたブロック</param>

	public void addBall(Ball pBall, Block pBlock = null) {
		// 全て出払っている場合
		if (!checkIsBallAvailable()) {
			Debug.Log("NO BALL AVAILABLE");
			return;
		}

		// BallのlastVelocityを取得
		Vector3 v = pBall.getLastVelocity();
		Vector3 position = pBall.transform.position;

		// Blockならブロックの位置を代入
		if (pBlock != null) {
			position = pBlock.transform.position;
		}

		_balls[counter].gameObject.SetActive(true);
		_balls[counter].transform.position = position;
		_balls[counter].gameObject.GetComponent<Rigidbody>().velocity = v;
		_balls[counter].InitAddBall(this.gameObject);

		counter++;
		if (counter >= n) {
			counter = 0;
		}
	}

	public void setAllBallsMagnitudeDefault() {
		setAllBallsMagnitude (magnitudeDefault);	
	}

	public void setAllBallsMagnitudeMax() {
		setAllBallsMagnitude (magnitudeDefault * 1.5f);	
	}

	public void setAllBallsMagnitude(float pMagnitude) {
		// Main
		_mainBall.magnitude = pMagnitude;
		_mainBall.reloadVelocity ();

		// AddBalls
		for (int i = 0; i < _balls.Count; i++) {
			_balls [i].magnitude = pMagnitude;
			_balls [i].reloadVelocity ();
		}
	}
}
