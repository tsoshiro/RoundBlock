using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
	int score;
	public Text _scoreLabel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void addScore(int pScore) {
		score += pScore;
		setScoreText(score.ToString());
	}

	public void resetScore() {
		score = 0;
		setScoreText(score.ToString());
	}

	public void setScoreText(string pString) {
		_scoreLabel.text = "SCORE : " + pString;
	}

	public int getScore() {
		return score;
	}
}
