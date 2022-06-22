using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject welcome;
    public GameObject questions;
    public int state = 0;
    [SerializeField] private TMPro.TMP_InputField field;
    public TextMeshProUGUI textQuestion;
    public GameObject timerPanel;
    public TextMeshProUGUI timer;
    public float timerCount = 0;
    public float maxTimer = 5;


    private void Start()
    {
        if(PlayerPrefs.GetFloat("Timer", 0) > 0)
        {
            error();
        }
    }

    public void showQuestions()
    {
        welcome.SetActive(false);
        questions.SetActive(true);
    }

    public void checkQuestion()
    {
        string text = field.text;

        if (state == 0 && text.Length > 2)
        {
            check0(text);
        }
        else if (state == 1 && text.Length > 2)
        {
            check1(text);
        }
        else if (state == 2)
        {
            check2(text);
        }
    }

    private void check0(string _text)
    {
        if(_text.Substring(0, 1) == _text.Substring(0, 1).ToUpper())
        {
            field.text = "";
            state++;
            textQuestion.text = "Quelle est votre couleur préférée ?";
        }
        else
        {
            error();
        }
    }

    private void check1(string _text)
    {
        _text = _text.ToUpper();

        if (_text.Contains("BLEU") || _text.Contains("ROUGE") || _text.Contains("JAUNE") || _text.Contains("VERT") || _text.Contains("VIOLET") || _text.Contains("ROSE") || _text.Contains("ORANGE") || _text.Contains("BRUN") || _text.Contains("MAUVE")
            || _text.Contains("CYAN"))
        {
            field.text = "";
            state++;
            textQuestion.text = "Quelle est la vélocité médiane d'une hirondelle à vide ?";
        }
        else
        {
            error();
        }
    }

    private void check2(string _text)
    {
        _text = _text.ToUpper();

        if ((_text.Contains("AFRI") || _text.Contains("EUROP")) && _text.Contains("?"))
        {
            SceneManager.LoadScene("Jeu");
        }
        else
        {
            error();
        }
    }

    private void error()
    {
        state = 0;
        field.text = "";
        textQuestion.text = "Quel est votre nom ?";

        timerPanel.SetActive(true);
        welcome.SetActive(false);
        questions.SetActive(false);

        if (PlayerPrefs.GetFloat("Timer", 0) == 0)
        {
            timerCount = maxTimer;
            PlayerPrefs.SetFloat("Timer", maxTimer);
        }
        else
        {
            timerCount = PlayerPrefs.GetFloat("Timer", 0);
        }

        StartCoroutine(IBeginTimer());
    }

    private IEnumerator IBeginTimer()
    {
        PlayerPrefs.SetFloat("Timer", timerCount);
        timer.text = timerCount.ToString();

        yield return new WaitForSeconds(1f);

        timerCount--;
        PlayerPrefs.SetFloat("Timer", timerCount);
        timer.text = timerCount.ToString();

        if (timerCount == 0)
        {
            timerPanel.SetActive(false);
            welcome.SetActive(true);
            questions.SetActive(false);
        }
        else
        {
            StartCoroutine(IBeginTimer());
        }
    }
}
