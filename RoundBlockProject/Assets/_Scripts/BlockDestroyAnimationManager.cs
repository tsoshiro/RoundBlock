using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestroyAnimationManager : MonoBehaviour {
	public GameObject _fx; // 演出用Cubeのプレハブ
	public GameObject _fxPoolContainer; // CubeFxPoolのGameObjectを入れておく

	int fxCount = 15;
	GameObject[] fxs; // Pool
	int counter = 0; // 選出用カウンタ
	Vector3 fxPos;

	// 演出用Cubeを30個生成し、リストに保管
	public void init () {
		fxPos = _fxPoolContainer.transform.position;

		fxs = new GameObject[fxCount];
		for (int i = 0; i < fxCount; i++) {
			GameObject go = (GameObject)Instantiate (_fx, fxPos, _fx.transform.rotation);
			go.transform.parent = _fxPoolContainer.transform;
			go.name = "FX_" + i.ToString ("D2");
			go.SetActive(false);
			fxs [i] = go;
		}
	}

	public GameObject getFxObj() {
		// poolから演出用Cubeをひとつ取り出す
		GameObject fxObj = fxs[counter];
		return fxObj;
	}

	public int getCounter() {
		// 返す用
		int value = counter;

		// カウンタを増やす
		counter++;

		// カウンタがcubeFxPoolの最後の要素以上になったらリセット
		if (counter >= fxs.Length) {
			counter = 0;
		};

		return value;
	}

	// 消えるCubeの色と場所を引数に
	void playAnimation (Vector3 pPosition) {
		// poolから演出用Cubeをひとつ取り出す
		GameObject fxObj = fxs [counter];

		Vector3 pos = pPosition;
		pos.y -= 0.5f;
		fxObj.transform.position = pos;

		// 演出再生
		fxObj.SetActive(true);

		// カウンタを増やす
		counter++;

		// カウンタがcubeFxPoolの最後の要素以上になったらリセット
		if (counter >= fxs.Length) { 
			counter = 0;
		};
	}

	public IEnumerator animationCoroutine(Vector3 pPosition) {
		playAnimation(pPosition);

		yield return new WaitForSeconds(3f);

		int lastCounter = (counter > 0) ? counter - 1 : 0;
		returnToPool(lastCounter);
	}

	public void returnToPool (int pCounter) {
		GameObject fxObj = fxs [pCounter];

		// 位置を戻す
		fxObj.transform.position = fxPos;

		// falseにしておく
		fxObj.SetActive(false);
	}
}