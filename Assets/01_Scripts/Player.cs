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

    [Header("Cámaras")]
    public Camera mainCamera;
    public Transform isometricView;
    public Transform firstPersonView;
    public Transform thirdPersonView;
    public Transform frontView;
    public Transform topView;

    private GameObject nearbyPizza = null;
    private GameObject grabbedPizza = null;

    private bool invertedControls = false;
    private bool reversedClicks = false;

    void Start()
    {
        // Vista Inicial Isométrica
        SetCameraView(isometricView);

        // Inicia Ataque Fantasma
        StartCoroutine(GhostIntervention());
    }

    void Update()
    {
        MoverPlayer();

        if (!reversedClicks && Input.GetMouseButtonDown(0))  // Click Izq agarre
        {
            if (grabbedPizza == null && nearbyPizza != null)
            {
                GrabPizza();
            }
        }
        else if (!reversedClicks && Input.GetMouseButtonDown(1))  // Click Der soltar
        {
            if (grabbedPizza != null)
            {
                DropPizza();
            }
        }
        else if (reversedClicks && Input.GetMouseButtonDown(1))  // Click Der agarre (invertido)
        {
            if (grabbedPizza == null && nearbyPizza != null)
            {
                GrabPizza();
            }
        }
        else if (reversedClicks && Input.GetMouseButtonDown(0))  // Click Izq soltar (invertido)
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
    }

    void GrabPizza()
    {
        if (nearbyPizza != null)
        {
            grabbedPizza = nearbyPizza;
            grabbedPizza.GetComponent<Rigidbody>().isKinematic = true;
            grabbedPizza.transform.position = firePoint.position;
            grabbedPizza.transform.SetParent(firePoint);
        }
    }

    void DropPizza()
    {
        if (grabbedPizza != null)
        {
            grabbedPizza.GetComponent<Rigidbody>().isKinematic = false;
            grabbedPizza.transform.SetParent(null);
            grabbedPizza = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pizza"))
        {
            nearbyPizza = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pizza"))
        {
            nearbyPizza = null;
        }
    }

    IEnumerator GhostIntervention()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5f, 10f));

            // Invertir controles o clicks
            invertedControls = Random.value > 0.5f;
            reversedClicks = Random.value > 0.5f;

            // Cambiar cámara a una vista aleatoria
            ChangeRandomCameraView();

            Debug.Log("Ghost Attack");

            // Duración de la intervención del fantasma
            yield return new WaitForSeconds(10f);

            // Restaurar los controles y vista isométrica
            invertedControls = false;
            reversedClicks = false;

            SetCameraView(isometricView);
        }
    }

    void SetCameraView(Transform newView)
    {
        if (newView == firstPersonView)
        {
            // Hacer que la cámara siga al jugador en primera persona
            mainCamera.transform.SetParent(transform);
            mainCamera.transform.localPosition = firstPersonView.localPosition; // Posición Relativa Player
            mainCamera.transform.localRotation = firstPersonView.localRotation;
        }
        else
        {
            // Desanidar la cámara del Player cuando no esté en primera persona
            mainCamera.transform.SetParent(null);
            mainCamera.transform.position = newView.position;
            mainCamera.transform.rotation = newView.rotation;
        }
    }


    void ChangeRandomCameraView()
    {
        int randomView = Random.Range(0, 3);
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
