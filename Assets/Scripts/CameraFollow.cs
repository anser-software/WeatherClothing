using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform target;

    [SerializeField]
    private float followSpeed;

    private Vector3 offset;

    private bool follow = true;

    private void Start()
    {
        target = FindObjectOfType<PlayerController>().transform;

        offset = transform.position - target.position;

        GameplayManager.instance.OnGameStateChanged += () => { if (GameplayManager.instance.GameState == GameState.Finish) follow = false; };
    }

    private void Update()
    {
        if (!follow)
            return;

        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * followSpeed);
    }

}
