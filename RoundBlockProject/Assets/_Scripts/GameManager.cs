using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public enum GameState
	{
		READY,
		PLAYING,
		PAUSE,
		RESULT,
	}

	public Ball _ball;
	public RacketCtrl _racket;

	public GameState state;
	ScoreManager _scoreManager;
	TimeManager _timeManager;
	public BlockManager _blockManager;
	public BallManager _ballManager;

	int SCORE_PER_BLOCK = 100;
	float ADD_TIME_PER_BLOCK = 1f;
	float SUB_TIME_PER_MISS = 5f;

	// Use this for initialization
	void Start()
	{
		_scoreManager = this.gameObject.GetComponent<ScoreManager>();
		_timeManager = this.gameObject.GetComponent<TimeManager>();
	}

	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
			case GameState.READY:
				UpdateReady();
				break;
			case GameState.PLAYING:
				UpdatePlaying();
				break;
			case GameState.PAUSE:
				UpdatePause();
				break;
			case GameState.RESULT:
				UpdateResult();
				break;
			default:
				UpdateReady();
				break;
		}
	}

	void UpdateReady()
	{

	}

	void UpdatePlaying()
	{
		// 制限時間が0秒以下なら
		if (_timeManager.timer <= 0) {
			finishGame();
		}
	}

	void UpdatePause()
	{

	}

	void UpdateResult()
	{

	}



	public void setState(GameState pState)
	{
		state = pState;
	}

	public void startGame()
	{
		_timeManager.resetTime();
		_scoreManager.resetScore();

		_timeManager.startTimer();
		setState(GameState.PLAYING);
	}

	public void resumeGame() {
		_timeManager.startTimer();
		setState(GameState.PLAYING);
	}

	public void pauseGame() {
		setState(GameState.PAUSE);
		_timeManager.stopTimer();
	}

	public void finishGame() {
		setState(GameState.RESULT);

		_ball.resetPosition();
	}

	public void resetGame() {
		setState(GameState.READY);
	}

	// ブロック削除
	public void removeBlock() {
		// スコア追加
		_scoreManager.addScore(SCORE_PER_BLOCK);

		// 時間追加
		_timeManager.addTime(ADD_TIME_PER_BLOCK);
	}

	// ボール落とした=ミスした
	public void dropBall() {
		// 制限時間を減算
		_timeManager.addTime(- SUB_TIME_PER_MISS);
	}
}
