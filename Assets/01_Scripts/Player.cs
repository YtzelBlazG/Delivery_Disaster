using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 5.0f;
  

    [Header("Referencias")]
    public Rigidbody rb;
   

    void Update()
    {
        // Movimiento del jugador
        Movement();
        // Disparo del jugador
        
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical"); // Cambiar "y" por "z" para 3D
        rb.velocity = new Vector3(x, 0, z) * speed; // Usar Vector3 para 3D
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colisi�n detectada con: " + other.gameObject.name); // Esta l�nea es para depuraci�n

        if (other.CompareTag("Pizza")) // Verifica si colisiona con el objeto que tiene el tag "Pizza"
        {
            PlayerPizza playerPizza = GetComponent<PlayerPizza>();
            if (playerPizza != null)
            {
                playerPizza.CollectPizza(); // Llama al m�todo CollectPizza
            }

            other.gameObject.SetActive(false); // Desactiva la pizza despu�s de recogerla
        }
    }
}
