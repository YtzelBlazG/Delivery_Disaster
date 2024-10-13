using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npcs : MonoBehaviour
{
    private bool hasObject = false; // Para verificar si el NPC ha agarrado una pizza
    public GameObject[] points; // Puntos para NPCs con tag Point
    public float speed = 2f;
    public float timeToWait = 10f; // Tiempo de espera por defecto
    private Transform target;
    private float timeRemaining;
    private SpawnPoint currentPoint;
    private bool isWaiting = false;
    private bool goingToExit = false;
    private bool goingToPoint2 = false; // Para saber si va al Point2

    public Transform pizzaParent; // Asigna un transform en el que se "agarra" la pizza
    private GameObject currentPizza;

    // Referencia al Animator para manejar las animaciones
    private Animator animator;

    void Start()
    {
        points = GameObject.FindGameObjectsWithTag("Point");
        AssignNearestPoint();
        timeRemaining = timeToWait;

        // Obtener el componente Animator del NPC
        animator = GetComponent<Animator>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pizza")) // Verifica si el objeto tiene el tag "Pizza"
        {
            Debug.Log("Pizza agarrada: " + collision.gameObject.name); // Mensaje de depuración
            GrabPizza(collision.gameObject); // Llama a la función para agarrar la pizza
            // Cambia a la animación de estar feliz si recibe la pizza
            animator.SetTrigger("Happy");
        }
    }

    void GrabPizza(GameObject pizza)
    {
        currentPizza = pizza; // Guarda la referencia a la pizza
        pizza.transform.SetParent(pizzaParent); // Cambia el padre del objeto pizza al NPC
        pizza.transform.localPosition = Vector3.zero; // Ajusta la posición de la pizza a la posición local del NPC
        pizza.GetComponent<Collider>().enabled = false; // Desactiva el collider de la pizza si no deseas que afecte la física
        pizza.GetComponent<Rigidbody>().isKinematic = true; // Hace que la pizza no responda a la física
        hasObject = true; // Marca que el NPC ha agarrado un objeto
    }

    void Update()
    {
        if (target != null)
        {
            MoveToPoint();

            if (isWaiting)
            {
                // Si el NPC ha agarrado una pizza, espera solo 2 segundos en lugar de 10
                if (hasObject)
                {
                    timeRemaining = Mathf.Min(timeRemaining, 2f); // Establece el tiempo de espera a 2 segundos si tiene pizza
                }

                Debug.Log("Tiempo de espera restante: " + timeRemaining + " segundos"); // Mostrar el tiempo de espera en consola
                timeRemaining -= Time.deltaTime;

                if (timeRemaining <= 0f)
                {
                    animator.SetTrigger("Sad");
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

        // Si el NPC está en movimiento, activamos la animación de caminar
        animator.SetBool("Walking", target != null && !isWaiting);
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
            transform.LookAt(target);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                isWaiting = true;
                // Cambiar a la animación de pedir orden
                animator.SetTrigger("Waiting");

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
