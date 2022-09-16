using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{

    [SerializeField]
    private Material buildingsMat;

    [SerializeField]
    private ColorScheme baseColorScheme;

    [SerializeField]
    private int colorSchemeCount;

    [SerializeField]
    private Color snowWeatherAdder, rainWeatherAdder;

    private void Start()
    {
        var colorSchemes = new ColorScheme[colorSchemeCount];

        colorSchemes[0] = baseColorScheme;

        var bgColor = baseColorScheme.backgroundColor;

        Color.RGBToHSV(bgColor, out float Hbg, out float Sbg, out float Vbg);

        var buildColor = baseColorScheme.backgroundColor;

        Color.RGBToHSV(buildColor, out float Hbuild, out float Sbuild, out float Vbuild);

        for (int i = 1; i < colorSchemeCount; i++)
        {
            Hbg += 1F / colorSchemeCount;

            Hbg -= Mathf.Floor(Hbg);

            Hbuild += 1F / colorSchemeCount;

            Hbuild -= Mathf.Floor(Hbg);

            colorSchemes[i] = new ColorScheme(Color.HSVToRGB(Hbg, Sbg, Vbg),
                Color.HSVToRGB(Hbuild, Sbuild, Vbuild));
        }

        var currentColorScheme = colorSchemes[Random.Range(0, colorSchemes.Length)];

        var adder = Color.clear;

        switch (GameplayManager.instance.CurrentWeatherType)
        {
            case WeatherType.Snow:
                adder = snowWeatherAdder;
                break;
            case WeatherType.Rain:
                adder = new Color(-rainWeatherAdder.r, -rainWeatherAdder.g, -rainWeatherAdder.b, -rainWeatherAdder.a);
                break;
        }

        buildingsMat.color = currentColorScheme.buildingsColor + adder;

        RenderSettings.fogColor = currentColorScheme.backgroundColor + adder;

        Camera.main.backgroundColor = currentColorScheme.backgroundColor + adder;
    }

}

[System.Serializable]
public struct ColorScheme
{
    public Color backgroundColor, buildingsColor;

    public ColorScheme(Color backgroundColor, Color buildingsColor)
    {
        this.backgroundColor = backgroundColor;
        this.buildingsColor = buildingsColor;
    }
}