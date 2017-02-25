﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour {
	public List<Ball> _balls = new List<Ball>();

	public GameObject _ballPrefab;
	int n = 5; // Ball Prefabの値
	int counter = 0;

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
			obj.transform.SetParent(this.transform);
			obj.transform.localPosition = new Vector3(0, 0, 0);
			_balls.Add(obj.GetComponent<Ball>());
			obj.SetActive(false);
		}
	}

	bool checkIsBallAvailable() {
		// 動いていないのがひとつでもあればtrue
		for (int i = 0; i < n; i++) {
			if (!_balls[i].getIsMoving()) {
				return true;
			}
		}
		return false;
	}

	public void addBall(Ball pBall, Block pBlock = null) {
		// 全て出払っている場合
		if (!checkIsBallAvailable()) {
			Debug.Log("NO BALL AVAILABLE");
			return;	
		}

		// BallのVelocityを取得
		Vector3 v = pBall.gameObject.GetComponent<Rigidbody>().velocity;
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
}