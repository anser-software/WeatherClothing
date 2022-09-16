using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    public static LevelGenerator instance { get; private set; }

    [SerializeField]
    private LevelGenerationSettings levelGenerationSettings;

    [SerializeField]
    private Transform startPlatform, endPlatform, runwaysParent;

    [SerializeField]
    private GameObject clothingTriggerPrefab;

    [SerializeField]
    private bool minimizeRepetitions;

    private List<ClothingType> spawnedClothingTypeTriggers;

    private void Awake()
    {
        instance = this;
    }

    public void GenerateLevel()
    {
        var lvlGen = levelGenerationSettings;

        spawnedClothingTypeTriggers = new List<ClothingType>();

        var runwayLength = Random.Range(lvlGen.minRunwayLength, lvlGen.maxRunwayLength);

        List<float> clothingTriggersPositions = new List<float>();

        for (float i = lvlGen.safeRunwayDistanceFront; i < runwayLength - lvlGen.safeRunwayDistanceBack; i += Random.Range(lvlGen.minDistanceBetweenClothingTriggers, lvlGen.maxDistanceBetweenClothingTriggers))
        {
            clothingTriggersPositions.Add(i);
        }

        endPlatform.position = startPlatform.position + Vector3.forward * runwayLength;

        runwaysParent.position = new Vector3(runwaysParent.position.x, runwaysParent.position.y, (startPlatform.position.z + endPlatform.position.z) / 2F);

        runwaysParent.localScale = new Vector3(1F, 1F, runwayLength);

        foreach (var clothingTriggerPos in clothingTriggersPositions)
        {
            var clothingTriggerInstance = Instantiate(clothingTriggerPrefab);

            clothingTriggerInstance.transform.position = startPlatform.position + (endPlatform.position - startPlatform.position).normalized * clothingTriggerPos;

            var clothingTypes = ClothingType.GetValues(typeof(ClothingType));

            var clothingType = (ClothingType)clothingTypes.GetValue(Random.Range(0, clothingTypes.Length));

            if (minimizeRepetitions)
            {
                if (spawnedClothingTypeTriggers.Count > 0 && spawnedClothingTypeTriggers.Count < clothingTypes.Length)
                {
                    while (spawnedClothingTypeTriggers.Contains(clothingType))
                    {
                        clothingType = (ClothingType)clothingTypes.GetValue(Random.Range(0, clothingTypes.Length));
                    }

                    if (spawnedClothingTypeTriggers.Count == clothingTypes.Length)
                        spawnedClothingTypeTriggers = new List<ClothingType>();
                }

                spawnedClothingTypeTriggers.Add(clothingType);
            }

            clothingTriggerInstance.transform.GetComponentInChildren<ClothingTrigger>().Initialize(clothingType);
        }
    }

}
