using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StartSC : MonoBehaviour
{
    public Image YellowStar; // Estrella amarilla
    public Image GreyStar;   // Estrella gris

    private void Awake()
    {
        if (YellowStar != null)
        {
            YellowStar.gameObject.SetActive(true); // Aseg�rate de que la estrella amarilla est� activa
        }
        else
        {
            Debug.LogError("YellowStar no est� asignada en StartSC.");
        }

        if (GreyStar != null)
        {
            GreyStar.gameObject.SetActive(false);  // La estrella gris debe estar desactivada al inicio
        }
        else
        {
            Debug.LogError("GreyStar no est� asignada en StartSC.");
        }
    }
}
