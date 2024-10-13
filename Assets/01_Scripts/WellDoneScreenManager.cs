using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellDoneScreenManager : MonoBehaviour
{
    [SerializeField] StartSC[] Stars;

    [SerializeField] float EnlargeScale = 1.5f;
    [SerializeField] float ShrinkScale = 1f;
    [SerializeField] float EnlargeDuration = 0.25f;
    [SerializeField] float ShrinkDuration = 0.25f;

    private int currentStars; // Controla cuántas estrellas están activas

    void Start()
    {
        currentStars = 5; // Iniciamos con 5 estrellas activas (puedes ajustarlo)
        ShowStars(currentStars);
    }

    public void ShowStars(int numberOfStars)
    {
        StartCoroutine(ShowStarsRoutine(numberOfStars));
    }

    private IEnumerator ShowStarsRoutine(int numberOfStars)
    {
        foreach (StartSC star in Stars)
        {
            star.YellowStar.transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < numberOfStars; i++)
        {
            yield return StartCoroutine(EnlargeAndShrinkStar(Stars[i]));
        }
    }

    private IEnumerator EnlargeAndShrinkStar(StartSC star)
    {
        yield return StartCoroutine(ChangeStarScale(star, EnlargeScale, EnlargeDuration));
        yield return StartCoroutine(ChangeStarScale(star, ShrinkScale, ShrinkDuration));
    }

    private IEnumerator ChangeStarScale(StartSC star, float targetScale, float duration)
    {
        Vector3 initialScale = star.YellowStar.transform.localScale;
        Vector3 finalScale = new Vector3(targetScale, targetScale, targetScale);

        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            star.YellowStar.transform.localScale = Vector3.Lerp(initialScale, finalScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        star.YellowStar.transform.localScale = finalScale;
    }

    // Método para restar una estrella
    public void RemoveStar()
    {
        if (currentStars > 0)
        {
            currentStars--; // Reducir el número de estrellas activas
            Stars[currentStars].YellowStar.gameObject.SetActive(false); // Desactivar el GameObject de YellowStar
            Stars[currentStars].GreyStar.gameObject.SetActive(true);    // Activar el GameObject de GreyStar
        }
    }

    // Método para añadir una estrella
    public void AddStar()
    {
        if (currentStars < Stars.Length)
        {
            Stars[currentStars].GreyStar.gameObject.SetActive(false);    // Desactivar el GameObject de GreyStar
            Stars[currentStars].YellowStar.gameObject.SetActive(true);   // Activar el GameObject de YellowStar
            currentStars++; // Aumentar el número de estrellas activas
        }
    }
}

