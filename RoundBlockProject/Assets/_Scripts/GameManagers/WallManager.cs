using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour {
	public List<GameObject> _walls;
	public List<Vector3> _wallPositions;

	// Use this for initialization
	void Start () {
		setWallPositions ();
	}

	void setWallPositions() {
		_wallPositions = new List<Vector3>();
		for (int i = 0; i < _walls.Count; i++) {
			_wallPositions.Add(_walls[i].transform.position);
		}
	}
}
