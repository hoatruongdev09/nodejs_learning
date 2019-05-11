using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public static CameraFollow Instance { get; private set; }
    public bool useSmooth;

    public GameObject player;
    public float smoothSpeed = 0.125f;
    public Vector3 offSet = new Vector3 (0, 0, -10);

    private Camera mainCam;

    private Vector3 randomPos;
    private float resetRandomTime = 5;
    private float timing;

    private void Start () {
        Instance = this;
        mainCam = Camera.main;
    }
    public void Update () {
        if (player) {
            if (!useSmooth) {
                transform.position = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);
            } else {
                Vector3 desiredPosition = player.transform.position + offSet;
                transform.position = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed * Time.smoothDeltaTime);
            }
        }
    }
    public void ShakeIt (float time = 0.1f, float magnitude = 0.2f) {

        StopCoroutine (Shake (0, 0));
        StartCoroutine (Shake (time, magnitude));
    }
    private IEnumerator Shake (float time, float magnitude) {
        float timeRemain = 0.0f;
        while (timeRemain < time) {
            float x = Random.Range (-1f, 1f) * magnitude;
            float y = Random.Range (-1f, 1f) * magnitude;

            mainCam.transform.localPosition = new Vector3 (x, y, mainCam.transform.localPosition.z);
            timeRemain += Time.deltaTime;
            yield return null;
        }
        mainCam.transform.localPosition = Vector3.zero;
    }
}