using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelGenerationSettings", menuName = "Level Generation Settings", order = 1)]
public class LevelGenerationSettings : ScriptableObject
{

    public float minRunwayLength, maxRunwayLength;

    public float safeRunwayDistanceFront, safeRunwayDistanceBack;

    public float minDistanceBetweenClothingTriggers, maxDistanceBetweenClothingTriggers;

}
