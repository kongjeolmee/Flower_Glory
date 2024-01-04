using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{

    public float limitTime;
    public float currentTime;

    int currentWave = 1;
    public int CurrentWave
    {
        get { return currentWave; }
    }
    public int goalFlowerTile = 10;

    public bool waveCheck = false;
    int tileAmount = 200;
    public List<Transform> flowerTileList = new List<Transform>();

    public Image flowerCheckFill;
    

    private void Awake()
    {
        currentTime = limitTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeManager());
    }

    // Update is called once per frame
    void Update()
    {
        float tileCount = flowerTileList.Count;
        flowerCheckFill.DOFillAmount(tileCount / goalFlowerTile, 0.3f);
        //float tileCount = flowerTileList.Count;
        //flowerCheckFill.fillAmount = tileCount / goalFlowerTile;

        if (Input.GetKeyDown(KeyCode.O))
        {
            if(flowerTileList.Count > 0)
            {
                foreach (var item in flowerTileList)
                {
                    Debug.Log(item.name);
                }
            }
        }
    }

    public void UpdateFlowerTile()
    {
        float tileCount = flowerTileList.Count;
        flowerCheckFill.DOFillAmount(tileCount / goalFlowerTile, 0.3f);
    }

    IEnumerator TimeManager()
    {
        while (!GameManager.Instance.GameOver) //!gameover
        {
            while(currentTime > 0 && !waveCheck)
            {

                currentTime -= Time.deltaTime;
                yield return null;
            }

            waveCheck = true;
            StartCoroutine(WaveCheck());
            //waveCheck
            yield return new WaitUntil(() => waveCheck == false);

        }

    }

    IEnumerator WaveCheck()
    {
        waveCheck = true;

        if(flowerTileList.Count >= goalFlowerTile)
        {
            StartCoroutine(GameManager.uiManager.WaveCheckLoading(true));
        }
        else
        {
            StartCoroutine(GameManager.uiManager.WaveCheckLoading(false));

            //gameOver
        }
        yield return null;
    }

    public IEnumerator NextWave()
    {
        if(currentWave <= 9)
        {
            limitTime += 20f;
            goalFlowerTile += 2;
        }
        else
        {
            limitTime = 200f;
            goalFlowerTile = 20;
        }
        currentTime = limitTime;
        Time.timeScale = 1.0f;
        GameManager.waveManager.waveCheck = false;
        currentWave++;
        
        yield return null;
    }
}
