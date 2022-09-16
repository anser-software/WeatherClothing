using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float walkSpeed, accelerationDuration;

    [SerializeField]
    private AnimationCurve accelerationCurve;

    [SerializeField]
    private GameObject hotVFX, coldVFX, rainVFX;

    private bool walking;

    private float targetSpeed = 0F;

    private void Start()
    {
        GameplayManager.instance.OnGameStateChanged += () => 
        {
            if (GameplayManager.instance.GameState == GameState.Walking)
                StartWalking();
            else if (GameplayManager.instance.GameState == GameState.Finish)
                SetFinishPose();
        };

        GameplayManager.instance.OnClothingPicked += ClothingChange;
    }

    private void StartWalking()
    {
        animator.SetTrigger("Walk");
        walking = true;
    }

    public void ClothingChange(ClothingPiece clothingPicked)
    {
        if (!clothingPicked.correctWeather)
        {
            switch (GameplayManager.instance.CurrentWeatherType)
            {
                case WeatherType.Hot:
                    animator.SetTrigger("Hot");
                    hotVFX.SetActive(true);
                    break;
                case WeatherType.Hurricane:
                    animator.SetTrigger("Storm");
                    break;
                case WeatherType.Rain:
                    animator.SetTrigger("Rain");
                    rainVFX.SetActive(true);
                    break;
                case WeatherType.Snow:
                    animator.SetTrigger("Snow");
                    coldVFX.SetActive(true);
                    break;
            }
        } else
        {
            animator.SetTrigger("Walk");
            hotVFX.SetActive(false);
            coldVFX.SetActive(false);
            rainVFX.SetActive(false);
        }
    }

    private void SetFinishPose()
    {
        animator.SetTrigger("Finish");
        walking = false;
        hotVFX.SetActive(false);
        coldVFX.SetActive(false);
        rainVFX.SetActive(false);
    }

    private void Update()
    {
        if(walking)
        {
            targetSpeed = Mathf.Clamp01(targetSpeed + Time.deltaTime / accelerationDuration);
            transform.Translate(Vector3.forward * accelerationCurve.Evaluate(targetSpeed) * walkSpeed * Time.deltaTime);
        }
    }


}
