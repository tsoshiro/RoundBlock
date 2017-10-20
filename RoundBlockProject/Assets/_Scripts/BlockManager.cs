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
	DataCtrl dataCtrl;

	// 0:ADD_BALL 1:HARD 2:WIDER 3:SHOT 4:SUPER_SHOT 5:FEVER
	public List<float> RATE_LIST = new List<float> ();
	public float total = 100; // RATEの合計

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

		// RATE_SETTINGS
		RATE_LIST = new List<float> ();
		dataCtrl = new DataCtrl ();
		dataCtrl.InitData ();
		total = getTotalValue ();

		// SCORE
		InitScoreLevel();
	}

	/// <summary>
	/// ラケットを取得し、位置を設定する
	/// </summary>
	void setEdges() {
		_racketCtrl = GameObject.Find("Rackets").GetComponent<RacketCtrl>();
		edgeBottom 	= _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.BOTTOM].transform.position.z + MARGINE;
		edgeTop	   	= _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.TOP].transform.position.z - MARGINE;
		edgeLeft	= _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.LEFT].transform.position.x + MARGINE;
		edgeRight 	= _racketCtrl._rackets[(int)RacketCtrl.RacketPosition.RIGHT].transform.position.x - MARGINE;
	}

	/// <summary>
	/// 破壊アニメーションの設定
	/// </summary>
	void setAnimation() {
		_blockDestroyAnimationManager = this.GetComponent<BlockDestroyAnimationManager>();
		_blockDestroyAnimationManager.init();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateRates ();
	}

	float lastPeriod;
	/// <summary>
	/// 1秒ごとに確率更新
	/// </summary>
	void UpdateRates() {
		if (lastPeriod != Mathf.Floor (_gameManager._timeManager.timePassed)) {
			DebugLogger.Log ("UPDATE! lastPeriod:" + lastPeriod + " now:" + Mathf.Floor (_gameManager._timeManager.timePassed));
			lastPeriod = Mathf.Floor (_gameManager._timeManager.timePassed);
			total = getTotalValue ();
		}
	}

	/// <summary>
	/// ブロックの撤去処理
	/// </summary>
	/// <param name="pBlock">P block.</param>
	public void removeBlock(Block pBlock) {
		// 利用可能な演出オブジェクトを取得
		GameObject fxObj = _blockDestroyAnimationManager.getFxObj();
		int counter = _blockDestroyAnimationManager.getCounter();

		StartCoroutine(pBlock.animationCoroutine(fxObj, counter));

		_gameManager.removeBlock();


		// リスポーン
		Vector3 v = new Vector3((float)Random.Range((int)edgeLeft, (int)edgeRight), 0, (float)Random.Range((int)edgeBottom, (int)edgeTop));
		pBlock.transform.position = v;

		setItemType(pBlock);
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
		float n = Random.Range(0, total);
		float rate_value = 0;
		for (int i = 0; i < RATE_LIST.Count; i++) {
			rate_value += RATE_LIST [i];
			if (n <= rate_value) {
				itemType = (Const.ItemType)(i+1); // 0 : NONEなのでひとつずらす
				break;
			}
		}
		return itemType;
	}

	float getTotalValue() {
		RATE_LIST = dataCtrl.getRateList (_gameManager._timeManager.timePassed);

		float value = 100f;

//		for (int i = 0; i < RATE_LIST.Count; i++) {
//			value += RATE_LIST [i];
//		}
		return value;
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

	#region SCORE_TIME
	int scoreLevelNow = 0;
	int scoreLevelNext = 0;
	void InitScoreLevel() {
		ScoreLevelUpdate (0);
	}

	public void ScoreLevelUpdate(int pScore) {
		if (pScore == 0 ||
			pScore >= scoreLevelNext) {
			float addTimeNew = dataCtrl.getAddTime (pScore);
			_gameManager.setAddTimePerBlock (addTimeNew);

			scoreLevelNow = scoreLevelNext;
			scoreLevelNext = dataCtrl.getNextTargetScore (pScore);
		}
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
