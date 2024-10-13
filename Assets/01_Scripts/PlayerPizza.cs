using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPizza : MonoBehaviour
{
    public Image[] stars; // Asigna tus imágenes de estrellas en el inspector
    private int currentStars = 5; // Comienza con 5 estrellas activas
    private bool pizzaRecogida = false; // Inicializa la variable pizzaRecogida

    private void Start()
    {
        ShowStars(currentStars); // Muestra las estrellas al inicio
        StartCoroutine(PizzaTimer()); // Inicia el temporizador
    }

    private IEnumerator PizzaTimer()
    {
        while (currentStars > 0) // Solo continúa si hay estrellas
        {
            yield return new WaitForSeconds(2f); // Espera 2 segundos

            // Verifica si la pizza ha sido recogida
            if (!pizzaRecogida) // Verifica si no se ha recogido la pizza
            {
                RemoveStar(); // Si no se ha recogido, desactiva una estrella
            }
            else
            {
                pizzaRecogida = false; // Restablece la variable para permitir el siguiente ciclo
            }
        }
    }

    public void CollectPizza()
    {
        AddStar(); // Añade una estrella
        pizzaRecogida = true; // Marca que se ha recogido la pizza
    }

    private void RemoveStar()
    {
        if (currentStars > 0)
        {
            currentStars--; // Reducir el número de estrellas activas
            if (currentStars >= 0)
            {
                stars[currentStars].gameObject.SetActive(false); // Desactivar la estrella
            }
        }
    }

    private void AddStar()
    {
        if (currentStars < stars.Length) // Solo activa si hay menos estrellas activas que el total
        {
            // Activa la siguiente estrella
            if (stars[currentStars] != null)
            {
                stars[currentStars].gameObject.SetActive(true); // Activar la estrella
                currentStars++; // Aumentar el número de estrellas activas
            }
        }
    }

    private void ShowStars(int numberOfStars)
    {
        for (int i = 0; i < numberOfStars; i++)
        {
            if (stars[i] != null)
            {
                stars[i].gameObject.SetActive(true); // Activa la estrella
            }
        }
    }
}











