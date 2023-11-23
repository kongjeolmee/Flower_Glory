using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    public float limitTime;
    public float currentTime;

    int currentWave = 1;
    public int CurrentWave
    {
        get { return currentWave; }
    }
    public bool waveCheck = false;
    int tileAmount = 200;
    public List<Transform> flowerTileList = new List<Transform>();

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

    IEnumerator TimeManager()
    {
        while (true) //!gameover
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                yield return null;
            }
            else
            {
                StartCoroutine(WaveCheck());
                //waveCheck
                yield return null;

            }
        }

    }

    IEnumerator WaveCheck()
    {
        waveCheck = true;
        Time.timeScale = 0;


        yield return null;
    }

}
