using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Block group :
/// 複数のブロックからなる「ページ」を構成・管理するクラス
/// 機能として、
/// - グループのブロックがなくなったことをGameManagerに通知する
/// - ページ情報のインプットに応じて、ブロックを生成・配置し、指定された位置に配置する
/// </summary>
public class BlockGroup {
	int block_max; 	// ページ生成時のブロック数
	int block_left; // ページ内の残りブロック数
	float progress_rate; // block_left/block_max;

	List<Block> blocks = new List<Block>(); // 生成されたブロックリスト

	// 配置データ格納

	GameCtrl _gameCtrl; // 通知用

	/// <summary>
	/// ブロックグループを生成する
	/// </summary>
	void init() {
		
		blocks = new List<Block> ();
		instantiateBlocks ();

	}

	/// <summary>
	/// 入力された配置情報を元に、ブロックを生成・配置する
	/// </summary>
	/// <returns>The blocks.</returns>
	void instantiateBlocks() {
		// BlockプールからBlockを取得
		Block aBlock = new Block();

		// アイテムブロック設定
		aBlock.setItemType(Const.ItemType.NONE);
		aBlock.setBlockColor();

		// Blockの配置
		Vector3 pos = Vector3.zero;
		aBlock.transform.position = pos;

		// 追加
		blocks.Add(aBlock);
	}

	Block getBlock() {
		return new Block ();
	}

	/// <summary>
	/// 最後のブロックか確認
	/// </summary>
	/// <returns>The left.</returns>
	void checkIfIsLast() {
		if (blocks.Count <= 0) {
			// GameCtrlに通知
		}
	}
}
