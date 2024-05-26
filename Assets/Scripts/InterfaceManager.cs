using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaceManager : MonoBehaviour
{
    public GameObject GameplayInterface;
    public GameObject TitleInterface;
    public GameObject ResultInterface;

    enum screen { title, gameplay, result}
    screen curScreen = screen.title;

    private void OnEnable()
    {
        toTitle();
        Clock.OverEvent += toResult;
    }

    private void OnDisable()
    {
        Clock.OverEvent -= toResult;
    }

    public void toTitle()
    {
        curScreen = screen.title;

        TitleInterface?.SetActive(true);

        GameplayInterface?.SetActive(false);
        ResultInterface?.SetActive(false);
    }

    public void toGameplay()
    {
        curScreen = screen.gameplay;

        GameplayInterface?.SetActive(true);

        TitleInterface?.SetActive(false);
        ResultInterface?.SetActive(false);

        
        Clock.StartEvent?.Invoke();
    }

    public void toResult()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.timeOut);
        curScreen = screen.result;

        ResultInterface?.SetActive(true);

        GameplayInterface?.SetActive(false);
        TitleInterface?.SetActive(false);
    }
    
    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (curScreen == screen.title)
        {
            if (Input.GetMouseButtonDown(0))
            {
                toGameplay();
            }
        }
    }
}
