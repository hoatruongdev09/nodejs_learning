using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnManager : MonoBehaviour {
    public static SpawnManager Instance { get; private set; }
    public List<Vector3> spawnPoints;

    public GameObject playerPrefab;

    private GameObject PLAYER_HOLDER;

    private void Start () {
        Instance = this;
        PLAYER_HOLDER = new GameObject ("PLAYER_HOLDER");
    }
    public GameObject SpawnPlayer (string name, Vector3 position, Quaternion rotation) {
        GameObject playerGO = Instantiate (playerPrefab, position, rotation);
        playerGO.name = name;
        playerGO.transform.parent = PLAYER_HOLDER.transform;
        return playerGO;
    }
}