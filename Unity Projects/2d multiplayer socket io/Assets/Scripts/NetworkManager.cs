using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviour {
    public static NetworkManager Instance { get; private set; }
    public GameObject panel_Main;
    private SocketIOComponent socket;

    private void Awake () {
        if (Instance == null) {
            Instance = this;
        } else if (Instance != this) {
            Destroy (gameObject);
        }
        socket = GetComponent<SocketIOComponent> ();
        DontDestroyOnLoad (gameObject);
    }
    private void Start () {
        // TODO subcription
        socket.On ("other player connected", OnPlayerConnect);
        socket.On ("play", OnPlay);
        socket.On ("player disconnected", OnPlayerDisconnected);
        socket.On ("player move", OnPlayerMove);
        socket.On ("player turn", OnPlayerTurn);
    }

    public void ButtonJoinGame () {
        StartCoroutine (ConnectToServer ());
    }

    #region Command
    private IEnumerator ConnectToServer () {
        yield return new WaitForSeconds (.5f);
        socket.Emit ("player connect");
        yield return new WaitForSeconds (1f);
        string playerName = GameObject.Find ("if_name").GetComponent<InputField> ().text;
        Vector3 position = MapInfo.Instance.GetRandomPosition ();
        Quaternion rotation = MapInfo.Instance.GetRandomRotation ();
        PlayerJSON playerJSON = new PlayerJSON (playerName, position, rotation);
        string data = JsonUtility.ToJson (playerJSON);
        socket.Emit ("join", new JSONObject (data));
    }
    public void CmdMove (Vector3 vector3) {
        string data = JsonUtility.ToJson (new Vector3JSON (vector3));
        socket.Emit ("player move", new JSONObject (data));
    }
    public void CmdTurn (Quaternion quaternion) {
        string data = JsonUtility.ToJson (new QuaternionJSON (quaternion));
        socket.Emit ("player turn", new JSONObject (data));
    }
    #endregion

    #region Listen
    private void OnPlayerConnect (SocketIOEvent socketIOEvent) {
        Debug.Log ("Some one joined");
        string data = socketIOEvent.data.ToString ();
        PlayerJSON playerJSON = PlayerJSON.CreateFromJSON (data);
        // check if he already join
        if (GameObject.Find (playerJSON.name) != null) {
            return;
        }
        GameObject player = SpawnManager.Instance.SpawnPlayer (playerJSON.name, playerJSON.position.ToVector3 (), playerJSON.rotation.Euler ());
        PlayerController pc = player.GetComponent<PlayerController> ();
        pc.isLocalPlayer = false;

    }
    private void OnPlay (SocketIOEvent socketIOEvent) {

        string data = socketIOEvent.data.ToString ();
        Debug.Log ("You joined: " + data);
        PlayerJSON playerJSON = PlayerJSON.CreateFromJSON (data);
        GameObject player = GameObject.Find (playerJSON.name);
        if (player != null) {
            Destroy (player);
        }
        player = SpawnManager.Instance.SpawnPlayer (playerJSON.name, playerJSON.position.ToVector3 (), playerJSON.rotation.Euler ());
        PlayerController pc = player.GetComponent<PlayerController> ();
        pc.isLocalPlayer = true;
        CameraFollow.Instance.player = player;
        panel_Main.SetActive (false);
    }
    private void OnPlayerDisconnected (SocketIOEvent socketIOEvent) {
        Debug.Log ("player disconnected");
        string data = socketIOEvent.data.ToString ();
        PlayerJSON playerJSON = PlayerJSON.CreateFromJSON (data);
        Destroy (GameObject.Find (playerJSON.name));
    }
    private void OnPlayerMove (SocketIOEvent socketIOEvent) {
        string data = socketIOEvent.data.ToString ();
        PlayerJSON playerJSON = PlayerJSON.CreateFromJSON (data);
        Debug.Log (playerJSON.name + " is moving " + data);
        GameObject player = GameObject.Find (playerJSON.name);
        if (player != null) {
            player.transform.position = playerJSON.position.ToVector3 ();
        }
    }
    private void OnPlayerTurn (SocketIOEvent socketIOEvent) {
        string data = socketIOEvent.data.ToString ();
        PlayerJSON playerJSON = PlayerJSON.CreateFromJSON (data);
        Debug.Log (playerJSON.name + " is rotating " + playerJSON.rotation.ToQuaternion ());
        GameObject player = GameObject.Find (playerJSON.name);
        if (player != null) {
            player.transform.rotation = playerJSON.rotation.ToQuaternion ();
        }
    }
    #endregion

    #region JSON Package
    [Serializable]
    public class PlayerJSON {
        public string name;
        public Vector3JSON position;
        public QuaternionJSON rotation;

        public PlayerJSON (string name, Vector3 position, Quaternion rotation) {
            this.name = name;
            this.position = new Vector3JSON (position);
            this.rotation = new QuaternionJSON (rotation);
        }
        public static PlayerJSON CreateFromJSON (string data) {
            return JsonUtility.FromJson<PlayerJSON> (data);
        }
    }

    [Serializable]
    public class Vector3JSON {
        public float x;
        public float y;
        public float z;
        public Vector3JSON (float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public Vector3JSON (Vector3 vector3) {
            this.x = vector3.x;
            this.y = vector3.y;
            this.z = vector3.z;
        }
        public Vector3 ToVector3 () {
            return new Vector3 (x, y, z);
        }
    }

    [Serializable]
    public class QuaternionJSON {
        public float x;
        public float y;
        public float z;
        public float w;
        public QuaternionJSON (float x, float y, float z, float w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public QuaternionJSON (Quaternion quaternion) {
            this.x = quaternion.x;
            this.y = quaternion.y;
            this.z = quaternion.z;
            this.w = quaternion.w;
        }
        public Quaternion ToQuaternion () {
            return new Quaternion (x, y, z, w);
        }
        public Quaternion Euler () {
            return Quaternion.Euler (x, y, z);
        }
    }
    #endregion
}