using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameplayManager : MonoBehaviour
{
    
    public static GameplayManager instance { get; private set; }

    public GameState GameState { get; private set; }

    public ClothingManager playerClothingManager { get; private set; }

    public EnemyClothingManager enemyClothingManager { get; private set; }

    public ClothingPiece[] currentClothingSelection { get; private set; }

    public System.Action OnGameStateChanged;

    public System.Action<ClothingPiece> OnClothingPicked;

    public WeatherType CurrentWeatherType;

    [Header("Top,Bottom,Shoes,Accessory,Head")]
    [SerializeField]
    private Sprite[] iconPerClothingType;

    [SerializeField]
    private GameObject[] clothingSelectVFX;

    [SerializeField]
    private Vector3 clothingVFXOffset;

    [SerializeField]
    private float selectClothingDuration, selectClothingTimeScale;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerClothingManager = FindObjectOfType<ClothingManager>();

        enemyClothingManager = FindObjectOfType<EnemyClothingManager>();

        LevelGenerator.instance.GenerateLevel();
    }

    public void StartGameplay()
    {
        if (GameState != GameState.WaitOnStart)
            return;

        GameState = GameState.Walking;

        OnGameStateChanged?.Invoke();
    }

    public void EnterClothingSelectionTrigger(ClothingType clothingType)
    {
        if (GameState == GameState.SelectClothing)
            return;

        Time.timeScale = selectClothingTimeScale;

        currentClothingSelection = playerClothingManager.GetClothesWithAtLeastOneCorrect(clothingType);

        enemyClothingManager.currentClothingType = clothingType;

        //currentClothingSelection = playerClothingManager.allClothingPieces.Where(c => !c.isDefault).ToArray();

        GameState = GameState.SelectClothing;

        OnGameStateChanged?.Invoke();
    }

    public Sprite GetClothingTypeIcon(ClothingType clothingType)
    {
        return iconPerClothingType[(int)clothingType];
    }

    public void SelectClothing(int index)
    {
        Debug.Log("Selected " + index.ToString());

        playerClothingManager.ActivateClothing(currentClothingSelection[index]);

        enemyClothingManager.ActivateRandomClothing();

        var vfxInstance = Instantiate(clothingSelectVFX[Random.Range(0, clothingSelectVFX.Length)], playerClothingManager.transform.position + clothingVFXOffset, Quaternion.identity);

        Time.timeScale = 1F;

        GameState = GameState.Walking;

        OnClothingPicked?.Invoke(currentClothingSelection[index]);

        OnGameStateChanged?.Invoke();
    }

    public void Finish()
    {
        if (GameState == GameState.Finish)
            return;

        GameState = GameState.Finish;

        OnGameStateChanged?.Invoke();
    }

    public void FinishCompleted()
    {
        if (GameState == GameState.Completed)
            return;

        LevelManager.instance.WinLevel();

        GameState = GameState.Completed;

        OnGameStateChanged?.Invoke();
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartGameplay();
        }
    }

}

public enum GameState
{
    WaitOnStart,
    Walking,
    SelectClothing,
    Finish,
    Completed
}

public enum ClothingType
{
    Top,
    Bottom,
    Shoes,
    Accessory,
    Head
}