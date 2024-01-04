using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance
    {
        get { 
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance; }
        set { instance = value; }
    }

    public static Player player;

    public static WaveManager waveManager;
    public static UIManager uiManager;
    public static GridManager gridManager;
    public static PoolManager poolManager;
    
    public int maxMonster;
    public int monsterKillCount = 0;
    public int flowerCount = 0;

    bool gameOver = false;

    float waterFullTime = 1.8f;

    int maxHeart = 7;

    int swordType = 1;
    float swordCool = 1f;
    #region property
        
     public float WaterFullTime
     {
        get { return waterFullTime; }
        set { waterFullTime = value; }
     }
    public int SwordType
    {
        get { return swordType; }
        set { swordType = value; }
    }
    public float SwordCool
    {
        get { return swordCool; }
        set { swordCool = value; }
    }

    public int MaxHeart
    {
        get { return maxHeart; }
        set { maxHeart = value; }
    }

    public bool GameOver
    {
        get { return gameOver; }
        set { gameOver = value; }
    }


    #endregion

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        player = FindObjectOfType<Player>();
        waveManager = FindObjectOfType<WaveManager>();
        uiManager = FindObjectOfType<UIManager>();
        gridManager = FindObjectOfType<GridManager>();
        poolManager = FindObjectOfType<PoolManager>();

        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
   
}
