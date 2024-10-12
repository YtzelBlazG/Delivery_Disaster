using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] pizzaPrefabs;  
    public Transform leftPoint;
    public Transform rightPoint;
    public float timeBtwSpawn = 5f; 

    private float timer = 0f;

    void Start()
    {
        timer = timeBtwSpawn;  
    }

    void Update()
    {
        if (timer < timeBtwSpawn)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            SpawnPizza();
        }
    }

    void SpawnPizza()
    {
        PizzaType randomPizza = (PizzaType)Random.Range(0, pizzaPrefabs.Length);

        float x = Random.Range(leftPoint.position.x, rightPoint.position.x);
        float z = Random.Range(leftPoint.position.z, rightPoint.position.z);
        Vector3 spawnPos = new Vector3(x, 0, z);

        Instantiate(pizzaPrefabs[(int)randomPizza], spawnPos, Quaternion.identity);
    }

    public enum PizzaType 
    { 
        Pepperoni, 
        Salame, 
        Clasica, 
        Hawaiana, 
        Napolitana, 
        Champinones 
    }

}
