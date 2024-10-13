using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private bool isOccupied = false; // Estado de ocupaci�n del punto

    public bool IsOccupied()
    {
        return isOccupied; // Devuelve el estado de ocupaci�n
    }

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied; // Establece el estado de ocupaci�n
    }
}
