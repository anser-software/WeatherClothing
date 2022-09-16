using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIClothingButton : MonoBehaviour
{

    private int clothIndex;

    public void Initialize(int clothingIndex, ClothingPiece clothingPiece)
    {
        clothIndex = clothingIndex;
        Debug.Log(clothingPiece.name);
        GetComponent<Image>().sprite = clothingPiece.sprite;
    }

    public void Press()
    {
        GameplayManager.instance.SelectClothing(clothIndex);
    }

}
