using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab del NPC
    public float spawnInterval = 2f; // Intervalo de tiempo entre apariciones
    private float spawnTimer = 0f; // Temporizador interno

    void Update()
    {
        // Incrementa el temporizador en función del tiempo transcurrido
        spawnTimer += Time.deltaTime;

        // Si el temporizador supera el intervalo de aparición, genera un NPC
        if (spawnTimer >= spawnInterval)
        {
            Instantiate(npcPrefab, transform.position, Quaternion.identity); // Instancia el NPC
            spawnTimer = 0f; // Reinicia el temporizador
        }
    }
}
