using System.Collections;
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

	#region ITEM RATE
	public List<int> RATE_LIST = new List<int> ();

	// 自前のRATE設定クラス
	[System.Serializable]
	class RateSettings {
		public int RATE_ADD_BALL 	= 10;
		public int RATE_HARD 		= 10;
		public int RATE_WIDER 		= 30;
		public int RATE_SHOT 		= 0;
		public int RATE_SUPER_SHOT 	= 0;
		public int RATE_FEVER	 	= 0;
	};

	#endregion

	// HARD BALL MODE
	float HARD_TIME = 10f;
	float hardTimer = 0f;
	float MARGINE_ADJUSTER = 2;

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
		_racketCtrl = GameObject.Find("Rackets").GetComponent<RacketCtrl>();
		edgeBottom 	= _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.BOTTOM].transform.position.z + MARGINE;
		edgeTop	   	= _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.TOP].transform.position.z - MARGINE;
		edgeLeft	= _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.LEFT].transform.position.x + MARGINE;
		edgeRight 	= _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.RIGHT].transform.position.x - MARGINE;
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

		setItemType(pBlock);
		
		//blockCount--;
		//if (blockCount == 0) {
		//	reset();
		//	blockCount = _blockList.Count;
		//}
	}

	/// <summary>
	/// ItemTypeを決めるアルゴリズム
	/// </summary>
	/// <returns>The item type.</returns>
	Const.ItemType decideItemType() {
		int itemCounter = 0;
		Const.ItemType itemType = Const.ItemType.NONE;

		// 場に5個以上あるなら、アイテムを出さない。
		for (int i = 0; i < _blockList.Count; i++) {
			if (_blockList [i].getItemType () != Const.ItemType.NONE)
				itemCounter++;
			if (itemCounter >= ITEM_NUM)
				return itemType;
		}

		// 確率でアイテムを出す
		int n = Random.Range(0, 100);
		int rate_value = 0;
		for (int i = 0; i < RATE_LIST.Count; i++) {
			rate_value += RATE_LIST [i];
			if (n <= rate_value) {
				itemType = (Const.ItemType)(i+1); // 0 : NONEなのでひとつずらす
				break;
			}
		}
		return itemType;
	}
		
	void setItemType(Block pBlock) {
		// ItemTypeを決定する
		Const.ItemType itemType = decideItemType();

		// ItemクラスにItemTypeを設定する
		pBlock.setItemType(itemType);

		// 色を変える
		pBlock.setBlockColor ();
	}

	public void fxReset(int pCounter) {
		_blockDestroyAnimationManager.returnToPool(pCounter);
	}


	public void reset() {
		for (int i = 0; i < _blockList.Count; i++) {
			_blockList[i].setDefaultPosition();
		}
	}

	#region HARD BALL METHOD
	/// <summary>
	/// ボール
	/// 位置が、ブロックが出現する位置より外にある(=ラケットに近い)場合、trueを返す
	/// </summary>
	/// <returns><c>true</c>, if is near racket was checked, <c>false</c> otherwise.</returns>
	/// <param name="pBall">P ball.</param>
	public bool checkIsNearRacket(Ball pBall) {
		Vector3 pos = pBall.gameObject.transform.position;
		if (pos.z > edgeTop 	+ MARGINE_ADJUSTER||
			pos.z < edgeBottom 	- MARGINE_ADJUSTER ||
			pos.x > edgeRight	+ MARGINE_ADJUSTER||
			pos.x < edgeLeft	- MARGINE_ADJUSTER) {
			return true;
		}
		return false;
	
	}
	#endregion

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
