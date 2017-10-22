using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCtrl {
	List<ItemMaster> _itemMasterList = new List<ItemMaster>();
	List<TimeMaster> _timeMasterList = new List<TimeMaster>();

	public void InitData() {
		InitItemMasterData ();
		InitTimeMasteData ();
	}

	public void InitItemMasterData() {
		var entityMasterTable = new ItemMasterTable ();
		entityMasterTable.Load ();
		foreach (var entityMaster in entityMasterTable.All) {
			_itemMasterList.Add (entityMaster);
		}
	}

	public void InitTimeMasteData() {
		var entityMasterTable = new TimeMasterTable ();
		entityMasterTable.Load ();
		foreach (var entitymaster in entityMasterTable.All) {
			_timeMasterList.Add (entitymaster);
		}
	}

	// TODO 目的の行を低負荷で探す // a) IDを振る。 b)探索アルゴリズムを組む
	public List<float> getRateList(float pTimePassed) {
		List<float> resultList = new List<float> ();

		float s = Mathf.Floor (pTimePassed);
		int n = 0;
		for (int i = _itemMasterList.Count - 1; i >= 0; i--) {
			if (_itemMasterList [i].TIME_PASSED_LESS_THAN <= s) {
				n = i;
				break;
			}
		}

		ItemMaster im = _itemMasterList[n];
		resultList.Add (im.RATE_ADD_BLOCK);
		resultList.Add (im.RATE_HARD);
		resultList.Add (im.RATE_WIDE);
		resultList.Add (im.RATE_SHOT);
		resultList.Add (im.RATE_SUPERSHOT);
		resultList.Add (im.RATE_FEVER);
		resultList.Add (im.RATE_TIME);

		return resultList;
	}

	public float getAddTime(int pScore) {
		int n = 0;
		for (int i = _timeMasterList.Count - 1; i >= 0; i--) {
			if (_timeMasterList [i].SCORE_IS_LOWER_THAN <= pScore) {
				n = i;
				break;
			}
		}
		return _timeMasterList [n].ADD_TIME;
	}

	public int getNextTargetScore(int pScore) {
		int n = 0;
		for (int i = _timeMasterList.Count - 1; i >= 0; i--) {
			if (_timeMasterList [i].SCORE_IS_LOWER_THAN <= pScore) {
				n = (i >= _timeMasterList.Count - 1) ? i : i + 1;
				break;
			}
		}
		return _timeMasterList[n].SCORE_IS_LOWER_THAN;
	}
}
