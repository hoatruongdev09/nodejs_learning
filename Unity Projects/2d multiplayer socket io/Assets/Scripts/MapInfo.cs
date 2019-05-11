using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour {
	public static MapInfo Instance { get; private set; }
	public Vector2 mapSize = new Vector2 (100, 100);

	private void Start () {
		Instance = this;
	}
	public Vector3 GetRandomPosition () {
		float randomdX = Random.Range (-mapSize.x / 2, mapSize.x / 2);
		float randomdY = Random.Range (-mapSize.y / 2, mapSize.y / 2);
		return new Vector3 (randomdX, randomdY);
	}
	public Quaternion GetRandomRotation () {
		float angle = Random.Range (0f, 360f);
		return Quaternion.Euler (0, 0, angle);
	}
	private void OnDrawGizmos () {
		Gizmos.DrawWireCube (Vector3.zero, mapSize);
	}
}