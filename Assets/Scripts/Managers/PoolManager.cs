using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    public int MaxObject;
    public Transform SpawnArea;
    public GameObject MonsterObject;
    public Queue<MonsterAi> gameObjects;
    //순서대로 슬라임,데비트리오,로얄슬라임,로얄데비트리오,비배드,고스틀라임,라바바,베이비듀,바인샤먼
    /*public int[ , ] Chances = new int[9, 9] {
        {100, 0, 0, 0, 0, 0, 0, 0, 0},
        {50, 50, 0, 0, 0, 0, 0, 0, 0},
        {40, 40, 0, 0, 0, 0, 20, 0, 0},
        {33, 33, 0, 0, 0, 0, 34, 0, 0 },
        {28, 28, 15, 15, 14, 0, 0, 0, 0},
        {24, 24, 13, 13, 13, 13, 0, 0, 0},
        {22, 22, 12, 11, 11, 11, 11, 0, 0},
        {21, 21, 11, 11, 11, 10, 10, 5, 0},
        {20, 20, 10, 10, 10, 10, 10, 5, 5}
    };*/
    public int[,] Chances = new int[9, 9] {
        {100, 0, 0, 0, 0, 0, 0, 0, 0},
        {50, 50, 0, 0, 0, 0, 0, 0, 0},
        {40, 40, 0, 0, 0, 0, 20, 0, 0},
        {40, 40, 0, 0, 10, 0, 10, 0, 0},
        {35, 35, 0, 0, 15, 0, 15, 0, 0},
        {30, 30, 0, 0, 20, 0, 20, 0, 0},
        {30, 30, 0, 0, 20, 0, 20, 0, 0},
        {30, 30, 0, 0, 20, 0, 20, 0, 0},
        {30, 30, 0, 0, 20, 0, 20, 0, 0},
    };
    public string[] MonsterList = new string[9] { "Slime", "DevyTrio", "RoyalSlime", "RoyalDevyTrio", "BeeBad", "Ghostlime", "HappyLarvava", "BabyDew", "VineSharman"};
    void Start()
    {
        gameObjects = new Queue<MonsterAi>();
        Instance = this;
        for (int i = 0; i < MaxObject; i++)
        { 
            gameObjects.Enqueue(CreateNewMonster());
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("소환");
            GetMonster();
        }
    }


    private MonsterAi CreateNewMonster()
    {
        var newObj = Instantiate(MonsterObject).GetComponent<MonsterAi>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform.GetChild(4));
        return newObj;
    }

    public void GetMonster()
    {
        if (Instance.gameObjects.Count > 0)
        {
            int direction = Random.Range(0, 4);
            SpawnArea = transform.GetChild(direction);
            Vector3 range;
            if (direction == 1 || direction == 2)
            {
                range = new Vector3((Random.Range(0, SpawnArea.transform.localScale.x) - (SpawnArea.transform.localScale.x / 2)), 0 ,0);
            }
            else
            {
                range = new Vector3(0, (Random.Range(0, SpawnArea.transform.localScale.y) - (SpawnArea.transform.localScale.y / 2)),0);
            }
            var obj = Instance.gameObjects.Dequeue();
            obj.transform.SetParent(null);
            obj.transform.position = SpawnArea.position + range;
            int monsterTypeSetting = Random.Range(1,101);
            int monsterChance = 0;
            int wave = GameManager.waveManager.CurrentWave - 1;
            if(wave > 8)
            {
                wave = 8;
            }
            for (int i = 0; i < 9; i++)
            {
                monsterChance += Chances[wave, i];
                if(monsterTypeSetting <= monsterChance)
                {
                    Debug.Log(MonsterList[i]);
                    obj.monsterOriginData = Resources.Load<MonsterData>("ScriptableScripts/Monster/"+MonsterList[i]);
                    break;
                }
            }
            obj.gameObject.SetActive(true);
        }
    }
    public void  ReturnMonster(MonsterAi obj)
    {
        Debug.Log("몬스터 집으로 돌아가기");
        obj.gameObject.SetActive(false);
        obj.monsterOriginData = null;
        obj.transform.SetParent(Instance.transform.GetChild(4));
        Instance.gameObjects.Enqueue(obj);

    }
}
