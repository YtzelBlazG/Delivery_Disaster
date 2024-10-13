using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndLookAtMouse : MonoBehaviour
{
    public float rotationSpeed = 30f; 

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, transform.position);

        if (plane.Raycast(ray, out float distance))
        {
            Vector3 mousePositionInWorld = ray.GetPoint(distance);
            Vector3 directionToMouse = (mousePositionInWorld - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(directionToMouse);

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            Quaternion currentRotation = transform.rotation;
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}