using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Item_Forecast : MonoBehaviour {
	public Text txt_DateTime;
	public Text txt_Status;
	public Text txt_Temperature;
	public Image img_Icon;

	public void SetData (string dt, string stt, string temp, string icon) {
		txt_DateTime.text = dt;
		txt_Status.text = stt;
		txt_Temperature.text = Mathf.FloorToInt (float.Parse (temp)) + "˚<size=20>C</size>";
		//img_Icon.sprite = icon;
		Sprite sp = Resources.Load<Sprite> ("Icons/" + icon);
		//Debug.Log (icon + " : " + sp);
		img_Icon.sprite = sp;
	}
	public void SetData (string dt, string stt, string temp) {
		txt_DateTime.text = dt;
		txt_Status.text = stt;
		txt_Temperature.text = Mathf.FloorToInt (float.Parse (temp)) + "˚<size=20>C</size>";
	}

}