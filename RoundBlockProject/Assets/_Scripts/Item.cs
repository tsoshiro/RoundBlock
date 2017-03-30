using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	GameManager _gameManager;

	// Managers
	BallManager _ballManager;
	BlockManager _blockManager;

	enum ItemType {
		ADD_BALL,
		HARD,
		WIDER_RACKET,
		SHOT,
		SUPER_SHOT
	}

	ItemType myType;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.Find("GameCtrl").GetComponent<GameManager>();
		_ballManager = _gameManager._ballManager;
		_blockManager = _gameManager._blockManager;
	}

	// ボール情報を受け取る
	public void hit(Ball pBall) {
		// よしなに処理

		switch (myType) {
			case ItemType.ADD_BALL:
				addBall(pBall);
				break;
			case ItemType.HARD:
				setBallHard (pBall);
				break;
			case ItemType.WIDER_RACKET:
				break;
			case ItemType.SHOT:
				break;
			case ItemType.SUPER_SHOT:
				break;
		}
	}

	void setBallHard(Ball pBall) {
		pBall.setState ((int)Ball.BallState.HARD);
	}

	void addBall(Ball pBall) {
		_ballManager.addBall(pBall, this.gameObject.GetComponent<Block>());
	}

	void setRacketWide() {
		
	}

	void setShot() {
		
	}
}
