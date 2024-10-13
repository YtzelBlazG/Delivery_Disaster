using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npcs : MonoBehaviour
{
    public GameObject[] points; // Puntos para NPCs con tag Point
    public float speed = 2f;
    public float timeToWait = 10f;
    private Transform target;
    private float timeRemaining;
    private SpawnPoint currentPoint;
    private bool isWaiting = false;
    private bool goingToExit = false;
    private bool goingToPoint2 = false; // Para saber si va al Point2

    void Start()
    {
        points = GameObject.FindGameObjectsWithTag("Point");
        AssignNearestPoint();
        timeRemaining = timeToWait;
    }

    void Update()
    {
        if (target != null)
        {
            MoveToPoint();

            if (isWaiting)
            {
                timeRemaining -= Time.deltaTime;

                if (timeRemaining <= 0f)
                {
                    if (goingToExit)
                    {
                        AssignToExit(); // Asignar al punto "Exit"
                    }
                    else
                    {
                        AssignToMiddle(); // Asignar al punto "Middle"
                    }
                }
            }
        }
        else
        {
            AssignNearestPoint();
        }
    }

    void AssignNearestPoint()
    {
        float closestDistance = float.MaxValue;
        bool foundFreePoint = false; // Variable para verificar si encontramos un punto libre

        // Si el NPC tiene el tag NPC_Point
        if (gameObject.CompareTag("NPC_Point"))
        {
            foreach (GameObject point in points)
            {
                SpawnPoint spawnPoint = point.GetComponent<SpawnPoint>();

                if (spawnPoint != null && !spawnPoint.IsOccupied()) // Verifica si el spawnPoint existe y no está ocupado
                {
                    float distance = Vector3.Distance(transform.position, point.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        target = point.transform;
                        currentPoint = spawnPoint;
                        foundFreePoint = true; // Marcar que se encontró un punto libre
                    }
                }
            }
        }
        // Si el NPC tiene el tag NPC_Point2
        else if (gameObject.CompareTag("NPC_Point2"))
        {
            GameObject middlePoint = GameObject.FindWithTag("Middle");
            if (middlePoint != null)
            {
                target = middlePoint.transform;
                foundFreePoint = true; // Marcar que se encontró un punto libre
            }
        }
    }

    void AssignToMiddle()
    {
        GameObject middlePoint = GameObject.FindWithTag("Middle");
        if (middlePoint != null)
        {
            target = middlePoint.transform;
            currentPoint?.SetOccupied(false);
            currentPoint = null;
            isWaiting = false;
        }
        else
        {
            Debug.LogError("No se encontró un punto con el tag 'Middle'. Asegúrate de que el objeto exista en la escena.");
        }
    }

    void AssignToExit()
    {
        GameObject exitPoint = GameObject.FindWithTag("Exit");
        if (exitPoint != null)
        {
            target = exitPoint.transform;
            currentPoint?.SetOccupied(false);
            currentPoint = null;
            isWaiting = false;
        }
        else
        {
            Debug.LogError("No se encontró un punto con el tag 'Exit'. Asegúrate de que el objeto exista en la escena.");
        }
    }

    void MoveToPoint()
    {
        if (target != null)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, step);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                isWaiting = true;

                if (target.CompareTag("Middle"))
                {
                    if (goingToPoint2)
                    {
                        goingToExit = true; // Asignar a Exit después de Point2
                    }
                    else
                    {
                        goingToPoint2 = true; // Después de Middle, va a Point2 si es un NPC de ese grupo
                    }
                }
                else if (target.CompareTag("Exit")) // Lógica para destruir el NPC al llegar al Exit
                {
                    Destroy(gameObject); // Destruye el NPC
                }
            }
        }
    }

    private void OnDestroy()
    {
        currentPoint?.SetOccupied(false); // Liberar el punto ocupado cuando el NPC se destruye
    }
}
