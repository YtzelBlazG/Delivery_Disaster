using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
    public PizzaSpawner.PizzaType tipo;

    private void Start()
    {
        Destroy(gameObject, 10f);
    }
}

