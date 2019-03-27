using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIController : MonoBehaviour {
	public static UIController Instance { get; private set; }

	public Text txt_currentTemperature;
	public Text txt_mainDescription;
	public Text txt_detailDesciption;
	public Image img_icon;
	public Text txt_cityName;
	public Item_Forecast childPrefab;
	public GameObject forecastContent;
	private void Start () {
		Instance = this;
	}

	public void UpdateForcastContent (WeatherForcast forcast) {
		foreach (WeatherInfo wi in forcast.list) {
			Item_Forecast tmp = Instantiate (childPrefab, forecastContent.transform);
			tmp.SetData (wi.dt_txt, wi.weather[0].description, wi.main.temp.ToString (), wi.weather[0].icon);
		}
	}
	public void UpdateImageIcon (string icon) {
		Sprite sp = Resources.Load<Sprite> ("Icons/" + icon);
		img_icon.sprite = sp;
	}
	public void UpdateTextCurrentTemperature (string temp) {
		txt_currentTemperature.text = temp;
	}
	public void UpdateTextMainDescription (string desc) {
		txt_mainDescription.text = desc;
	}
	public void UpdateTextDetailDescription (string desc) {
		txt_detailDesciption.text = desc;
	}
	public void UpdateTextCityName (string ctName) {
		txt_cityName.text = ctName;
	}
}