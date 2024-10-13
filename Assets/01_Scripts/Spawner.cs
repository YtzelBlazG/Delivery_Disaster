using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject npcPrefab;
    public float spawnInterval = 2f; 
    private float spawnTimer = 0f; 
    public GameObject[] points; 

    void Start()
    {
        points = GameObject.FindGameObjectsWithTag("Point"); 
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval && IsThereFreePoint())
        {
            GameObject newNpc = Instantiate(npcPrefab, transform.position, Quaternion.identity); 
            AssignTagToNpc(newNpc); 
            spawnTimer = 0f; 
        }
    }

    bool IsThereFreePoint()
    {
        foreach (GameObject point in points)
        {
            SpawnPoint pointScript = point.GetComponent<SpawnPoint>();
            if (!pointScript.IsOccupied())
            {
                return true; 
            }
        }

        return false; 
    }

    void AssignTagToNpc(GameObject npc)
    {
        foreach (GameObject point in points)
        {
            SpawnPoint pointScript = point.GetComponent<SpawnPoint>();
            if (!pointScript.IsOccupied())
            {
                npc.tag = "NPC_Point"; 
                pointScript.SetOccupied(true); 
                return; 
            }
        }
    }
}
