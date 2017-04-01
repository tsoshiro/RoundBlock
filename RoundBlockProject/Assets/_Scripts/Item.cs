﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
	GameManager _gameManager;

	// Managers
	BallManager _ballManager;
	BlockManager _blockManager;

	Const.ItemType myType;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.Find("GameCtrl").GetComponent<GameManager>();
		_ballManager = _gameManager._ballManager;
		_blockManager = _gameManager._blockManager;
		myType = Const.ItemType.NONE;
	}

	// ボール情報を受け取る
	public void hit(Ball pBall) {
		// よしなに処理
		switch (myType) {
			case Const.ItemType.ADD_BALL:
				addBall(pBall);
				break;
			case Const.ItemType.HARD:
				setBallHard (pBall);
				break;
			case Const.ItemType.WIDER_RACKET:
				break;
			case Const.ItemType.SHOT:
				break;
			case Const.ItemType.SUPER_SHOT:
				break;
		}
	}

	public void setItemType(Const.ItemType pItemType) {
		myType = pItemType;
	}

	public Const.ItemType getItemType () {
		return myType;
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
