using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bussiness : MonoBehaviour {
	public string weatherApiKey;
	public WeatherInfo weatherInfo;
	public WeatherForcast forecastInfo;
	private WeatherService weatherService;
	private LocationDetect locationService;

	private void Start () {
		locationService = new LocationDetect ();
		weatherService = new WeatherService (weatherApiKey, "metric");
		StartCoroutine (locationService.Detect (OnLocationDetected));
	}
	private void OnLocationDetected (LocationInfo info, bool success) {
		if (success) {
			StartCoroutine (weatherService.GetWeatherByLocation (info.latitude, info.longitude, OnWeatherCallback));
			StartCoroutine (weatherService.Get5DaysForcastByLocation (info.latitude, info.longitude, OnForcastCallback));
		} else {
			StartCoroutine (weatherService.GetWeatherByLocation (35, 139, OnWeatherCallback));
			StartCoroutine (weatherService.Get5DaysForcastByLocation (35, 139, OnForcastCallback));
		}
	}
	private void OnWeatherCallback (WeatherInfo info) {
		weatherInfo = info;
		UIController.Instance.UpdateTextCityName (info.name + ", " + info.sys.country);
		UIController.Instance.UpdateTextCurrentTemperature (Mathf.FloorToInt (info.main.temp) + "˚<size=70>C</size>");
		UIController.Instance.UpdateTextMainDescription (info.weather[0].main);
		Debug.Log ("info icon: " + info.weather[0].icon);
		UIController.Instance.UpdateImageIcon (info.weather[0].icon);
	}
	private void OnForcastCallback (WeatherForcast forcast) {
		forecastInfo = forcast;
		UIController.Instance.UpdateForcastContent (forcast);
	}
}