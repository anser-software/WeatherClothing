using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothingTrigger : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer clothingIcon;

    private ClothingType clothingType;

    private bool activated;

    public void Initialize(ClothingType clothingType)
    {
        this.clothingType = clothingType;

        clothingIcon.sprite = GameplayManager.instance.GetClothingTypeIcon(clothingType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated)
            return;

        var player = other.GetComponent<PlayerController>();

        if(player)
        {
            GameplayManager.instance.EnterClothingSelectionTrigger(clothingType);

            activated = true;
        }
    }

}
