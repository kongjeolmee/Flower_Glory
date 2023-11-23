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

    float waterFullTime = 1.8f;

    int maxHeart = 6;

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

    
    #endregion

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
        }
        player = FindObjectOfType<Player>();
        waveManager = FindObjectOfType<WaveManager>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
