using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npcs : MonoBehaviour
{
    public GameObject[] points; // Array de puntos donde pueden ir los NPCs
    public float speed = 2f; // Velocidad de movimiento
    public float stoppingDistance = 1f; // Distancia para detenerse
    public float minDistanceToPoint = 1.5f; // Distancia m�nima que se debe mantener del punto
    public float separationDistance = 1.5f; // Distancia m�nima para separarse de otros NPCs
    public int capacidadMaxima = 5; // Capacidad m�xima de NPCs en el �rea
    public int controlVariable = 1; // Variable de control para modificar desde el Inspector

    private bool isWaiting = false; // Indica si el NPC est� esperando
    private Transform target; // Almacena el objetivo (punto)
    private float timeRemaining = 10f; // Temporizador de 10 segundos

    void Start()
    {
        // Encuentra todos los objetos de tipo "Point" y los almacena en el array
        points = GameObject.FindGameObjectsWithTag("Point");
        AssignNearestPoint();
    }

    void Update()
    {
        // Verifica si hay un objetivo asignado
        if (target != null)
        {
            if (controlVariable == 1)
            {
                // Si controlVariable es 1, se mueve hacia el punto
                MoveToPoint();
                MaintainDistance(); // Mantiene la distancia de otros NPCs
            }
            else if (controlVariable == 0)
            {
                // Si controlVariable es 0, aleja del punto
                LeavePoint();
            }

            // Actualiza el temporizador solo si el NPC est� esperando
            if (isWaiting)
            {
                timeRemaining -= Time.deltaTime;

                // Imprime el tiempo restante en la consola
                Debug.Log($"Tiempo restante: {timeRemaining:F2} segundos");

                // Si el temporizador llega a 0, se resetea controlVariable
                if (timeRemaining <= 0f)
                {
                    controlVariable = 0; // Resetea controlVariable
                    Debug.Log("controlVariable se ha puesto en 0 despu�s de 10 segundos.");
                }
            }
        }
        else
        {
            // Si no hay objetivo, busca un nuevo punto
            AssignNearestPoint();
        }
    }

    void AssignNearestPoint()
    {
        // Asigna el punto m�s cercano que est� libre
        float closestDistance = float.MaxValue;
        foreach (GameObject point in points)
        {
            if (IsPointFree(point))
            {
                float distance = Vector3.Distance(transform.position, point.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = point.transform;
                }
            }
        }
    }

    bool IsPointFree(GameObject point)
    {
        // Verifica si el punto est� ocupado
        int npcCount = 0;
        Collider[] hitColliders = Physics.OverlapSphere(point.transform.position, stoppingDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("NPC")) // Aseg�rate de que los NPCs tengan el tag "NPC"
            {
                npcCount++;
            }
        }
        return npcCount < capacidadMaxima; // Devuelve verdadero si el punto est� libre
    }

    void MoveToPoint()
    {
        // Mueve el NPC hacia el punto solo si est� m�s lejos de la distancia m�nima
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget > minDistanceToPoint)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);
            isWaiting = false; // Mientras se mueve, no est� esperando
            timeRemaining = 10f; // Reinicia el temporizador a 10 al llegar
        }
        else
        {
            // Si ha llegado al punto de destino, se queda esperando
            if (!isWaiting) // Solo reinicia el temporizador si no est� esperando
            {
                timeRemaining = 10f; // Reinicia el temporizador al llegar
                Debug.Log("NPC ha llegado al punto."); // Confirmaci�n visual en consola
            }

            isWaiting = true; // Establece que ahora est� esperando
        }
    }

    void LeavePoint()
    {
        // L�gica para alejarse del punto
        if (target != null)
        {
            // Aleja al NPC del punto
            Vector3 awayDirection = (transform.position - target.position).normalized;
            transform.position += awayDirection * speed * Time.deltaTime;
            isWaiting = false; // Se aleja y no est� esperando
            Debug.Log("NPC se ha alejado del punto."); // Confirmaci�n visual en consola
        }
        else
        {
            AssignNearestPoint(); // Si no hay target, busca un nuevo punto
        }
    }

    void MaintainDistance()
    {
        // Verifica si hay otros NPCs cerca y ajusta la posici�n si es necesario
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, separationDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != this.gameObject) // Verifica que no sea el mismo NPC
            {
                // Separa si hay otros NPCs
                Vector3 direction = (transform.position - hitCollider.transform.position).normalized;
                transform.position += direction * 0.2f; // Ajusta la cantidad para la separaci�n
                Debug.Log("Separ�ndose de otro NPC."); // Verificaci�n visual
            }
        }
    }

    bool KitchenIsFull()
    {
        // L�gica para verificar si el �rea est� llena
        int npcCount = 0;
        Collider[] hitColliders = Physics.OverlapSphere(target.position, stoppingDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("NPC")) // Aseg�rate de que los NPCs tengan el tag "NPC"
            {
                npcCount++;
            }
        }
        return npcCount >= capacidadMaxima; // Devuelve verdadero si el �rea est� llena
    }

    // M�todo para volver a permitir el movimiento si el �rea no est� llena
    public void AllowMovement()
    {
        isWaiting = false;
        AssignNearestPoint(); // Intenta reasignar un punto cuando se permite el movimiento
    }
}
