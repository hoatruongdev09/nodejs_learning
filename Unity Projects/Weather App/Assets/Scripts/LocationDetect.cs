using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LocationDetect {
	private string status;
	private LocationInfo lastLocationInfo;
	private bool detectSuccess;
	public bool GetDetectSuccess () {
		return detectSuccess;
	}
	public LocationInfo GetLastLocationInfo () {
		return lastLocationInfo;
	}
	public IEnumerator Detect (Action<LocationInfo, bool> onSuccess) {
		// First, check if user has location service enabled
		detectSuccess = false;
		if (!Input.location.isEnabledByUser) {
			Debug.Log ("location service not enabled");
			UpdateText ("location service not enabled");
			onSuccess (lastLocationInfo, detectSuccess);
			yield break;
		}

		// Start service before querying location
		Input.location.Start ();

		// Wait until service initializes
		int maxWait = 20;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			Debug.Log ("Waiting for 20 sec");
			UpdateText ("Waiting for 20 sec");
			yield return new WaitForSeconds (1);
			maxWait--;
		}

		// Service didn't initialize in 20 seconds
		if (maxWait < 1) {
			Debug.Log ("Timed out");
			UpdateText ("Time out");
			yield break;
		}

		// Connection has failed
		if (Input.location.status == LocationServiceStatus.Failed) {
			Debug.Log ("Unable to determine device location");
			UpdateText ("Unable to determine device location");
			yield break;
		} else {
			lastLocationInfo = new LocationInfo ();
			lastLocationInfo = Input.location.lastData;
			UpdateText ("lat: " + lastLocationInfo.latitude + "\nlong: " + lastLocationInfo.longitude);
			detectSuccess = true;
			// Access granted and location value could be retrieved
			//print ("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
		}
		onSuccess (lastLocationInfo, detectSuccess);
		// Stop service if there is no need to query location updates continuously
		Input.location.Stop ();
	}
	private void UpdateText (string stt) {
		if (status != null)
			status = stt;
	}
	public string GetStatus () {
		return status;
	}
}