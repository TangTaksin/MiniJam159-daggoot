using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public float spawnRate;
    public GameObject[] spawnPool;

    float timer;

    private void Start()
    {
        timer = spawnRate;
    }

    void Spawn()
    {
        if (spawnPool.Length > 0)
        {
            var r = Random.Range(0, spawnPool.Length);
            Instantiate(spawnPool[r], transform.position, transform.rotation);
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Spawn();
            timer = spawnRate;
        }
    }
}
