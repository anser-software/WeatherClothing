using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyClothingManager : MonoBehaviour
{

    public ClothingPiece[] allClothingPieces { get; private set; }

    private List<ClothingPiece> ClothesTotal;

    [HideInInspector]
    public ClothingType currentClothingType;

    private void Start()
    {
        allClothingPieces = transform.GetComponentsInChildren<ClothingPiece>();
        ClothesTotal = allClothingPieces.ToList();
    }

    public void ActivateRandomClothing()
    {
        if (ClothesTotal.Count < 1)
            return;

        var rnd = new System.Random();

        var appropriateClothing = ClothesTotal.OrderBy(x => rnd.Next())
    .FirstOrDefault(c => c.ClothingType == currentClothingType);

        foreach (var cloth in ClothesTotal)
        {
            if (appropriateClothing.Equals(cloth))
            {
                cloth.Activate();
            }
            else if (appropriateClothing.ClothingType == cloth.ClothingType)
            {
                cloth.Deactivate();
            }
        }

        GetComponent<EnemyController>().ClothingChange(appropriateClothing);

        //ClothesTotal.Remove(appropriateClothing);
    }

}
