using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Atributos")]
    public float speed = 7f;

    [Header("Referencias")]
    public Rigidbody rb;
    public Transform firePoint;
    public Animator animator;

    [Header("Cámaras")]
    public Camera mainCamera;
    public Transform isometricView;
    public Transform firstPersonView;
    public Transform thirdPersonView;
    public Transform frontView;
    public Transform topView;

    [Header("Fantasma")]
    public GameObject ghostPrefab;
    public Transform[] ghostPositions;

    private GameObject nearbyPizza = null;
    private GameObject grabbedPizza = null;
    private GameObject ghostInstance = null;
    private bool invertedControls = false;
    private bool reversedClicks = false;

    [Header("Sonidos")]
    public AudioClip ghostSound;

    void Start()
    {
        SetCameraView(isometricView);
        StartCoroutine(GhostIntervention());
    }

    void Update()
    {
        MoverPlayer();
        CheckNearbyPizza();  // Verificar si hay pizza cerca

        if (!reversedClicks && Input.GetMouseButtonDown(0))
        {
            if (grabbedPizza == null && nearbyPizza != null)
            {
                GrabPizza();
            }
        }
        else if (!reversedClicks && Input.GetMouseButtonDown(1))
        {
            if (grabbedPizza != null)
            {
                DropPizza();
            }
        }
        else if (reversedClicks && Input.GetMouseButtonDown(1))
        {
            if (grabbedPizza == null && nearbyPizza != null)
            {
                GrabPizza();
            }
        }
        else if (reversedClicks && Input.GetMouseButtonDown(0))
        {
            if (grabbedPizza != null)
            {
                DropPizza();
            }
        }
    }

    void MoverPlayer()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (invertedControls)
        {
            x = -x;
            z = -z;
        }

        rb.velocity = Vector3.forward * speed * z + Vector3.right * speed * x;

        // Control de animaciones
        if (grabbedPizza == null)
        {
            if (x == 0 && z == 0)
            {
                animator.SetBool("Walking", false);
                animator.SetBool("Carrying", true);
            }
            else
            {
                animator.SetBool("Walking", true);
                animator.SetBool("Carrying", false);
            }
        }
        else
        {
            if (x == 0 && z == 0)
            {
                animator.SetBool("WalkingBox", false);
                animator.SetBool("CarryingBox", true);
            }
            else
            {
                animator.SetBool("WalkingBox", true);
                animator.SetBool("CarryingBox", false);
            }
        }
    }

    void CheckNearbyPizza()
    {
        float detectionRadius = 3f;  // Radio de Detección Pizza
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        nearbyPizza = null;  // Resetear la pizza cercana

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Pizza"))
            {
                nearbyPizza = collider.gameObject;
                break;  // Salir del bucle si encontramos una pizza
            }
        }
    }

    void GrabPizza()
    {
        if (nearbyPizza != null)
        {
            grabbedPizza = nearbyPizza;
            Rigidbody pizzaRb = grabbedPizza.GetComponent<Rigidbody>();

            if (pizzaRb != null)
            {
                pizzaRb.isKinematic = true; // Prevenir física
                pizzaRb.detectCollisions = false; // Desactivar colisiones
            }

            grabbedPizza.transform.position = firePoint.position;
            grabbedPizza.transform.SetParent(firePoint);
        }
    }


    void DropPizza()
    {
        if (grabbedPizza != null)
        {
            Rigidbody pizzaRb = grabbedPizza.GetComponent<Rigidbody>();

            if (pizzaRb != null)
            {
                pizzaRb.isKinematic = false; // Restaurar la física
                pizzaRb.detectCollisions = true; // Restaurar colisiones
            }

            grabbedPizza.transform.SetParent(null);
            grabbedPizza = null;
        }
    }


    IEnumerator GhostIntervention()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 30f));
            ghostInstance = Instantiate(ghostPrefab, GetRandomGhostPosition(), Quaternion.identity);
            invertedControls = Random.value > 0.5f;
            reversedClicks = Random.value > 0.5f;

            Debug.Log("Ghost Attack");
            ChangeRandomCameraView();
            StartCoroutine(MoveGhost());

            yield return new WaitForSeconds(10f);
            Destroy(ghostInstance);

            invertedControls = false;
            reversedClicks = false;

            SetCameraView(isometricView);
        }
    }

    IEnumerator MoveGhost()
    {
        while (ghostInstance != null)
        {
            Vector3 targetPosition = GetRandomGhostPosition();
            float moveDuration = Random.Range(2f, 4f);
            float elapsedTime = 0f;

            Vector3 startPosition = ghostInstance.transform.position;

            while (elapsedTime < moveDuration)
            {
                if (ghostInstance == null) yield break;
                ghostInstance.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }

    Vector3 GetRandomGhostPosition()
    {
        int randomIndex = Random.Range(0, ghostPositions.Length);
        return ghostPositions[randomIndex].position;
    }

    void SetCameraView(Transform newView)
    {
        if (newView == firstPersonView)
        {
            mainCamera.transform.SetParent(transform);
            mainCamera.transform.localPosition = firstPersonView.localPosition;
            mainCamera.transform.localRotation = firstPersonView.localRotation;
        }
        else
        {
            mainCamera.transform.SetParent(null);
            mainCamera.transform.position = newView.position;
            mainCamera.transform.rotation = newView.rotation;
        }
    }

    void ChangeRandomCameraView()
    {
        int randomView = Random.Range(0, 4);
        switch (randomView)
        {
            case 0:
                SetCameraView(firstPersonView);
                Debug.Log("Primera Persona");
                break;
            case 1:
                SetCameraView(thirdPersonView);
                Debug.Log("Tercera Persona");
                break;
            case 2:
                SetCameraView(frontView);
                Debug.Log("Vista Frente");
                break;
            case 3:
                SetCameraView(topView);
                Debug.Log("Vista Arriba");
                break;
            default:
                SetCameraView(isometricView);
                Debug.Log("Vista Isométrica");
                break;
        }
    }
}
