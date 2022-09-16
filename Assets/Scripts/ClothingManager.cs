using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ClothingManager : MonoBehaviour
{

    public ClothingPiece[] allClothingPieces => ClothesTotal;

    private ClothingPiece[] ClothesTotal;

    private void Start()
    {
        ClothesTotal = transform.GetComponentsInChildren<ClothingPiece>();
    }

    public ClothingPiece GetAppropriateClothing(ClothingType clothingType)
    {
        var rnd = new System.Random();

        var appropriateClothing = ClothesTotal.OrderBy(x => rnd.Next())
            .FirstOrDefault(c => c.ClothingType == clothingType && c.WeatherTypes.Contains(GameplayManager.instance.CurrentWeatherType));

        if(appropriateClothing == null)
        {
            Debug.LogError("No appropriate clothing present");
        }

        return appropriateClothing;
    }

    public void ActivateClothing(ClothingPiece clothingPiece)
    {
        foreach (var cloth in ClothesTotal)
        {
            if(clothingPiece.Equals(cloth))
            {
                cloth.Activate();
            } else if(clothingPiece.ClothingType == cloth.ClothingType)
            {
                cloth.Deactivate();
            }
        }
    }

    public ClothingPiece[] GetOneCorrectTwoWrongClothes(ClothingType clothingType)
    {
        var outputSet = new List<ClothingPiece>();

        outputSet.Add(GetAppropriateClothing(clothingType));

        var rnd = new System.Random();

        var wrongClothes = ClothesTotal.Where(c => c.ClothingType == clothingType && !c.WeatherTypes.Contains(GameplayManager.instance.CurrentWeatherType) && !c.isDefault).OrderBy(x => rnd.Next()).ToList();

        if (wrongClothes.Count > 2)
        {
            wrongClothes.RemoveRange(2, wrongClothes.Count - 2);
        }

        return outputSet.Concat(wrongClothes).ToArray();
    }

    public ClothingPiece[] GetClothesWithAtLeastOneCorrect(ClothingType clothingType)
    {
        var clothesOfCorrectType = ClothesTotal.Where(c => c.ClothingType == clothingType && !c.isDefault).ToList();

        var clothesOfCorrectTypeAndCorrectWeather = clothesOfCorrectType.Where(c => c.WeatherTypes.Contains(GameplayManager.instance.CurrentWeatherType)).ToList();

        var oneCorrectClothingIndex = Random.Range(0, clothesOfCorrectTypeAndCorrectWeather.Count);


        var correctClothing = clothesOfCorrectTypeAndCorrectWeather[oneCorrectClothingIndex];

        var outputSet = new List<ClothingPiece>();

        outputSet.Add(correctClothing);

        clothesOfCorrectType.Remove(correctClothing);

        Debug.Log(string.Format("PRIOR clothesOfCorrectType.Count = {0}", clothesOfCorrectType.Count));

        while (clothesOfCorrectType.Count > 2)
        {
            clothesOfCorrectType.RemoveAt(Random.Range(0, clothesOfCorrectType.Count));
        }


        Debug.Log(string.Format("AFTER clothesOfCorrectType.Count = {0}", clothesOfCorrectType.Count));

        return outputSet.Concat(clothesOfCorrectType).ToArray();

        /*
        var outputSet = new List<ClothingPiece>();

        outputSet.Add(GetAppropriateClothing(clothingType));

        var rnd = new System.Random();

        var wrongClothes = ClothesTotal.Where(c => c.ClothingType == clothingType && !c.WeatherTypes.Contains(GameplayManager.instance.CurrentWeatherType) && !c.isDefault).OrderBy(x => rnd.Next()).ToList();

        if (wrongClothes.Count > 2)
        {
            wrongClothes.RemoveRange(2, wrongClothes.Count - 2);
        }

        return outputSet.Concat(wrongClothes).ToArray();*/
    }

}
