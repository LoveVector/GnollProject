using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public bool wantToSpawn;

    Spawner[] spawnPoints;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            spawnPoints = GetComponentsInChildren<Spawner>();

            foreach (var spawnPoint in spawnPoints)
            {
                spawnPoint.stopSpawning = wantToSpawn;
            }
        }
    }
}
