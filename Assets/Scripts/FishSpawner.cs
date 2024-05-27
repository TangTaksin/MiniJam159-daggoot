using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject[] spawnPool;

    float timer;
    bool gameInSession = false;

    private void OnEnable()
    {
        Clock.StartEvent += StartSpawn;
        Clock.OverEvent += StopSpawn;
    }

    private void OnDisable()
    {
        Clock.StartEvent -= StartSpawn;
        Clock.OverEvent -= StopSpawn;
    }

    void Spawn()
    {
        if (spawnPool.Length > 0)
        {
            var r = Random.Range(0, spawnPool.Length);
            var fishRotate = Random.Range(0f, 360f);
            Instantiate(spawnPool[r], transform.position, Quaternion.Euler(0, fishRotate, 0));

        }
    }

    void StartSpawn()
    {
        timer = spawnRate;
        gameInSession = true;
    }

    void StopSpawn()
    {
        gameInSession = false;
    }


    private void Update()
    {
        if (gameInSession)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                Spawn();
                timer = spawnRate;
            }
        }
    }
}
