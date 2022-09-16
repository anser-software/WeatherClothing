using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ClothingPiece : MonoBehaviour
{

    public bool activated { get; private set; }

    public ClothingType ClothingType => clothingType;
    public WeatherType[] WeatherTypes => appropriateWeatherTypes;

    public bool correctWeather { get; private set; }

    public bool isDefault => defaultClothing;

    public Vector3 visualsPos => realVisualsPosition.position;

    public Sprite sprite;


    [SerializeField]
    private WeatherType[] appropriateWeatherTypes;

    [SerializeField]
    private ClothingType clothingType;

    [SerializeField]
    private GameObject[] visuals;

    [SerializeField]
    private bool defaultClothing;

    [SerializeField]
    private Transform realVisualsPosition;


    private void Start()
    {
        correctWeather = appropriateWeatherTypes.Contains(GameplayManager.instance.CurrentWeatherType);

        foreach (var visual in visuals)
        {
            visual.SetActive(defaultClothing);
        }
    }

    public void Activate()
    {
        foreach (var visual in visuals)
        {
            visual.SetActive(true );
        }

        activated = true;

        //correctWeather = appropriateWeather == GameplayManager.instance.CurrentWeatherType;

        Debug.Log(string.Format("Choice is {0}", (appropriateWeatherTypes.Contains(GameplayManager.instance.CurrentWeatherType)).ToString()));
    }

    public void Deactivate()
    {
        foreach (var visual in visuals)
        {
            visual.SetActive(false);
        }

        activated = false;
    }

}
