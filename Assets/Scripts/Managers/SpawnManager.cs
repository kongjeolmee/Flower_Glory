using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    Coroutine spawnCoroutine;

    public float spawnDelay;

    void Start()
    {
        StartSpawn();
    }

    void StartSpawn()
    {
        spawnCoroutine = StartCoroutine(Spawning());
    }

    void StopSpawn()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator Spawning()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnDelay);
            GameManager.poolManager.GetMonster();
            if(spawnCoroutine == null)
            {
                break;
            }
        }
    }
}
