﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour {
	public GameManager _gameManager;


	List<Block> _blockList;
	int blockCount;
	RacketCtrl _racketCtrl;
	BlockDestroyAnimationManager _blockDestroyAnimationManager;

	float edgeTop, edgeRight, edgeBottom, edgeLeft;
	float MARGINE = 5f;
	int ITEM_NUM = 5;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.Find("GameCtrl").GetComponent<GameManager>();

		setEdges();

		setAnimation();

		_blockList = new List<Block>();
		blockCount = this.transform.childCount;
		if (blockCount > 0) {
			for (int i = 0; i < blockCount; i++)
			{
				Block block = transform.GetChild(i).GetComponent<Block>();
				_blockList.Add(block);
			}
		}
	}

	void setEdges() {
		_racketCtrl = GameObject.Find("Racket").GetComponent<RacketCtrl>();
		edgeTop	   	= _racketCtrl.otherRackets[0].transform.position.z - MARGINE;
		edgeLeft	= _racketCtrl.otherRackets[1].transform.position.x + MARGINE;
		edgeRight 	= _racketCtrl.otherRackets[2].transform.position.x - MARGINE;
		edgeBottom 	= _racketCtrl.gameObject.transform.position.z + MARGINE;
	}

	void setAnimation() {
		_blockDestroyAnimationManager = this.GetComponent<BlockDestroyAnimationManager>();
		_blockDestroyAnimationManager.init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void removeBlock(Block pBlock) {
		// FX
		GameObject fxObj = _blockDestroyAnimationManager.getFxObj();
		int counter = _blockDestroyAnimationManager.getCounter();

		StartCoroutine(pBlock.animationCoroutine(fxObj, counter));

		_gameManager.removeBlock();


		// RESPAWN
		Vector3 v = new Vector3((float)Random.Range((int)edgeLeft, (int)edgeRight), 0, (float)Random.Range((int)edgeBottom, (int)edgeTop));
		pBlock.transform.position = v;

		addItem(pBlock);
		
		//blockCount--;
		//if (blockCount == 0) {
		//	reset();
		//	blockCount = _blockList.Count;
		//}
	}

	bool checkIsItem() {
		// 場に5個以上あるなら、アイテムを出さない。
		int itemCounter = 0;
		for (int i = 0; i < _blockList.Count; i++) {
			itemCounter++;
			if (itemCounter >= ITEM_NUM)
				return false;
		}	     

		int n = Random.Range(0, 100);
		if (n <= 20)
			return true;
		return false;
	}

	void addItem(Block pBlock) {
		if (!checkIsItem())
		{ // Itemでない
			if (pBlock.gameObject.GetComponent<Item>())
			{ // Itemがすでに付いているなら削除
				Destroy(pBlock.gameObject.GetComponent<Item>());
			}
		}
		else {
			pBlock.gameObject.AddComponent<Item>();		
		}

		pBlock.setItem();
	
	}

	public void fxReset(int pCounter) {
		_blockDestroyAnimationManager.returnToPool(pCounter);
	}


	public void reset() {
		for (int i = 0; i < _blockList.Count; i++) {
			_blockList[i].setDefaultPosition();
		}
	}

	#region DEBUG
	public void addMargineValue(float pFloat) {
		float abs = Mathf.Abs(MARGINE);
		abs += pFloat;
		MARGINE = abs;
		setEdges();
	}

	public float getMargineValue() {
		return MARGINE;
	}
	#endregion
}
