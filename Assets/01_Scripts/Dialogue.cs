using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public string[] lines;
    public float textSpeed = 0.1f ;
    int index;

    void Start()
    {
        dialogueText.text = string.Empty;
        StartDialog();
    }
     void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }

        }
    }

    public void StartDialog()
    {
        index = 0;
        StartCoroutine(Writeline());
    }

    IEnumerator Writeline()
    {
        foreach (var letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(Writeline());
        }
        else
        {
            gameObject.SetActive(false);
            // Aquí se carga la escena "Game" una vez que el diálogo termina
            SceneManager.LoadScene("Game");
        }
        
    
    }
}
