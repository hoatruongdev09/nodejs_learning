using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class Main {
    public float temp;
    public float temp_min;
    public float temp_max;
    public float pressure;
    public float sea_level;
    public float grnd_level;
    public float humidity;
    public float temp_kf;
}

[System.Serializable]
public class WeatherInfo {
    public int id;
    public string name;
    public float cod;
    public Coord coord;
    public Main main;
    public float visibility;
    public Wind wind;
    public Clouds clouds;
    public Rain rain;
    public float dt;
    public Sys sys;
    public List<Weather> weather;
    public string dt_txt;
}

[System.Serializable]
public class Wind {
    public float speed;
    public float deg;
}

[System.Serializable]
public class Clouds {
    public float all;
}

[System.Serializable]
public class Rain {

}

[System.Serializable]
public class Sys {
    public float type;
    public int id;
    public float message;
    public string country;
    public float sunrise;
    public float sunset;
    public string pod;
}

[System.Serializable]
public class Coord {
    public float lon;
    public float lat;
}

[System.Serializable]
public class Weather {
    public int id;
    public string main;
    public string description;
    public string icon;
}

[System.Serializable]
public class WeatherForcast {
    public string cod;
    public float message;
    public int cnt;
    public List<WeatherInfo> list;
}