using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	GameManager _gameManager;

	// Managers
	BallManager _ballManager;
	BlockManager _blockManager;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.Find("GameCtrl").GetComponent<GameManager>();
		_ballManager = _gameManager._ballManager;
		_blockManager = _gameManager._blockManager;
	}

	// ボール情報を受け取る
	public void hit(Ball pBall) {
		// よしなに処理
		addBlock(pBall);
	}

	void addBlock(Ball pBall) {
		_ballManager.addBall(pBall, this.gameObject.GetComponent<Block>());
	}
}
