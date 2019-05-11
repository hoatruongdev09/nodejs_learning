using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public bool isLocalPlayer = false;

	private Vector3 oldPosition;
	private Quaternion oldRotation;

	private void Start () {
		oldPosition = transform.position;
		oldRotation = transform.rotation;
	}

	private void Update () {
		if (!isLocalPlayer) {
			return;
		}
		transform.Rotate (0, 0, Input.GetAxis ("Horizontal") * Time.deltaTime * 150f);
		transform.Translate (Vector2.up * Input.GetAxis ("Vertical") * Time.deltaTime * 3f);

		if (transform.position != oldPosition) {
			NetworkManager.Instance.CmdMove (transform.position);
			oldPosition = transform.position;
		}
		if (transform.rotation != oldRotation) {
			NetworkManager.Instance.CmdTurn (transform.rotation);
			oldRotation = transform.rotation;
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			//Network stuff

		}
	}
}