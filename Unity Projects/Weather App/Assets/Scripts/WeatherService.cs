using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherService {
	public string apiKey;
	public string units;
	public WeatherService (string apiKey, string units) {
		this.apiKey = apiKey;
		this.units = units;
	}
	public IEnumerator GetWeatherByCityName (string cityName, Action<WeatherInfo> callback) {
		string uriReq = string.Format ("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units={2}", cityName, apiKey, units);
		using (UnityWebRequest req = UnityWebRequest.Get (uriReq)) {
			yield return req.SendWebRequest ();
			while (!req.isDone) {
				yield return null;
			}
			byte[] result = req.downloadHandler.data;
			string weatherJason = System.Text.Encoding.Default.GetString (result);
			WeatherInfo info = JsonUtility.FromJson<WeatherInfo> (weatherJason);
			callback (info);
		}
	}
	public IEnumerator GetWeatherByCityName (string cityName, string countryCode, Action<WeatherInfo> callback) {
		string uriReq = string.Format ("http://api.openweathermap.org/data/2.5/weather?q={0},&appid={1}&units={2}", cityName, countryCode, apiKey, units);
		using (UnityWebRequest req = UnityWebRequest.Get (uriReq)) {
			yield return req.SendWebRequest ();
			while (!req.isDone) {
				yield return null;
			}
			byte[] result = req.downloadHandler.data;
			string weatherJason = System.Text.Encoding.Default.GetString (result);
			WeatherInfo info = JsonUtility.FromJson<WeatherInfo> (weatherJason);
			callback (info);
		}
	}
	public IEnumerator GetWeatherByLocation (float lat, float lon, Action<WeatherInfo> callback) {
		string uriReq = string.Format ("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units={2}&appid={3}", lat, lon, units, apiKey);
		using (UnityWebRequest req = UnityWebRequest.Get (uriReq)) {
			yield return req.SendWebRequest ();
			while (!req.isDone) {
				yield return null;
			}
			byte[] result = req.downloadHandler.data;
			string weatherJason = System.Text.Encoding.Default.GetString (result);
			Debug.Log ("my response: " + req.isNetworkError + " " + weatherJason);
			WeatherInfo info = JsonUtility.FromJson<WeatherInfo> (weatherJason);
			callback (info);
		}
	}
	public IEnumerator Get5DaysForcastByLocation (float lat, float lon, Action<WeatherForcast> callback) {
		string uriReq = string.Format ("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&units={2}&appid={3}", lat, lon, units, apiKey);
		using (UnityWebRequest req = UnityWebRequest.Get (uriReq)) {
			yield return req.SendWebRequest ();
			while (!req.isDone) {
				yield return null;
			}
			byte[] result = req.downloadHandler.data;
			string weatherJason = System.Text.Encoding.Default.GetString (result);
			Debug.Log ("my response: " + req.isNetworkError + " " + weatherJason);
			WeatherForcast forcast = JsonUtility.FromJson<WeatherForcast> (weatherJason);
			callback (forcast);
		}
	}
}