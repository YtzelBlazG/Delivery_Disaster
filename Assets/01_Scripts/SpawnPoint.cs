using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private bool isOccupied = false; // Estado de ocupación del punto

    public bool IsOccupied()
    {
        return isOccupied; // Devuelve el estado de ocupación
    }

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied; // Establece el estado de ocupación
    }
}
