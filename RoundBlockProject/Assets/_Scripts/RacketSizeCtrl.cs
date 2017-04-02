using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketSizeCtrl : MonoBehaviour {
	List<Vector3> defaultSize = new List<Vector3>(); // 1f
	List<GameObject> rackets = new List<GameObject>();

	float EACH_SIZE = 0.2f;
	float EACH_TIME = 5f;
	int SIZE_LIMIT = 5;

	float widthTimer = 0f;
	List<float> time_check_points = new List<float>();
	int size = 0;
	int lastSize = 0; // 記録用

	// Use this for initialization
	void Start () {
		// チェックする時間の初期化
		for (int i = 0; i < SIZE_LIMIT; i++) {
			time_check_points.Add ((float)i * EACH_TIME);
		}
	}

	/// <summary>
	/// 初期化。Racketsの初期サイズを格納
	/// </summary>
	/// <param name="pRackets">P rackets.</param>
	public void Init(List<GameObject> pRackets) {
		for (int i = 0; i < pRackets.Count; i++) {
			rackets.Add (pRackets [i]);
			defaultSize.Add (pRackets [i].transform.localScale);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (size > 0) {
			countTimer ();
		}
		lastSize = size;
	}

	void countTimer() {
		widthTimer -= Time.deltaTime;

		if (widthTimer <= 0) {
			widthTimer = 0;
			size = 0;
			setWidth (size);
			return;
		}

		int activeSize = 0;
		for (int i = 0; i < SIZE_LIMIT; i++) {
			if (widthTimer > time_check_points [i]) {
				activeSize++;
			} else {
				size = activeSize;
				setWidth (size);
				return;
			}
		}
	}

	public void addSize() {
		size++;
		if (size > SIZE_LIMIT) {
			size = SIZE_LIMIT;
		}
		widthTimer += EACH_TIME;
		setWidth (size);
	}

	void setWidth(int pSize) {
		if (size == lastSize) // 前のフレームと同じサイズならば処理をスキップ
			return;
		for (int i = 0; i < defaultSize.Count; i++) {
			Vector3 newSize = defaultSize[i];
			float addOnRate = (float)pSize * EACH_SIZE;
			newSize.x *= 1 + addOnRate;

			rackets[i].transform.localScale = newSize;
		}
	}

	public bool checkIsWiderRacketActive() {
		if (size > 0) 
			return true;
		return false;
	}
}
