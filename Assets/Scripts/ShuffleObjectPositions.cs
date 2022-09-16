using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ShuffleObjectPositions : MonoBehaviour
{

    private List<Transform> objectsToShuffle;

    private void Start()
    {
        objectsToShuffle = GetComponentsInChildren<Transform>().ToList();

        objectsToShuffle.Remove(transform);

        var rnd = new System.Random();

        var positions = objectsToShuffle.Select(o => o.position).OrderBy(x => rnd.Next()).ToArray();

        for (int i = 0; i < objectsToShuffle.Count; i++)
        {
            objectsToShuffle[i].transform.position = positions[i];
        }
    }

}
