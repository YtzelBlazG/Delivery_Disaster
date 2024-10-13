using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaSpawner : MonoBehaviour
{
    [Header("Pizza Prefabs")]
    public GameObject[] pizzaPrefabs; // Prefabs de pizzas

    [Header("Spawn Points")]
    public Transform[] spawnPoints; // Puntos de spawn

    public float timeBtwSpawn = 5f;
    public float restTime = 10f; 
    private float timer = 0f;
    private int pizzaCount = 0; 
    private bool isResting = false; // Para controlar el descanso

    void Start()
    {
        timer = timeBtwSpawn;
    }

    void Update()
    {
        if (isResting)
        {
            timer += Time.deltaTime;
            if (timer >= restTime)
            {
                isResting = false;
                timer = 0f; 
            }
            return; 
        }

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
        for (int i = 0; i < pizzaPrefabs.Length; i++)
        {
            Vector3 spawnPos = spawnPoints[i].position;
            Instantiate(pizzaPrefabs[i], spawnPos, Quaternion.identity);
            pizzaCount++; 
        }

        if (pizzaCount >= 10)
        {
            isResting = true; 
            pizzaCount = 0; 
        }
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
