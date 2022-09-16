using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : MonoBehaviour
{

    public static UIManager instance { get; private set; }

    [SerializeField]
    private GameObject clothingSelectionUI, startScreen, finishScreen;

    [SerializeField]
    private UIClothingButton clothingButtonTemplate;

    [SerializeField]
    private Image weatherIcon;

    [SerializeField]
    private Sprite hot, snow, rain, hurricane;

    [SerializeField]
    private float finishScreenShowDelay;

    [SerializeField]
    private Text levelCounter;

    private List<GameObject> buttonInstances;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameplayManager.instance.OnGameStateChanged += () => 
        {
            if (GameplayManager.instance.GameState == GameState.SelectClothing) 
                OpenClothingSelection(); 
            else if(GameplayManager.instance.GameState == GameState.Completed)
            {
                DOTween.Sequence().SetDelay(finishScreenShowDelay).OnComplete(() => finishScreen.SetActive(true));
            }
            else
            {
                if (buttonInstances != null)
                {
                    for (int i = 0; i < buttonInstances.Count; i++)
                    {
                        Destroy(buttonInstances[i]);
                    }
                    buttonInstances.Clear();
                }

                clothingSelectionUI.SetActive(false);
            }
        };

        switch(GameplayManager.instance.CurrentWeatherType)
        {
            case WeatherType.Snow:
                weatherIcon.sprite = snow;
                break;
            case WeatherType.Hot:
                weatherIcon.sprite = hot;
                break;
            case WeatherType.Rain:
                weatherIcon.sprite = rain;
                break;
            case WeatherType.Hurricane:
                weatherIcon.sprite = hurricane;
                break;
        }

        levelCounter.text = "LEVEL " + LevelManager.instance.currentLevel.ToString();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startScreen.SetActive(false);
        }
    }

    public void NextLevel()
    {
        LevelManager.instance.StartNextLevel();
    }

    private void OpenClothingSelection()
    {
        clothingSelectionUI.SetActive(true);

        buttonInstances = new List<GameObject>();

        var clothes = GameplayManager.instance.currentClothingSelection;

        for (int i = 0; i < clothes.Length; i++)
        {
            var buttonInstance = Instantiate(clothingButtonTemplate, clothingButtonTemplate.transform.parent);

            buttonInstance.gameObject.SetActive(true);
            buttonInstance.Initialize(i, clothes[i]);

            buttonInstances.Add(buttonInstance.gameObject);
        }
    }

}
