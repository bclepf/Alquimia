using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public static bool jogoPausado = false;

    public GameObject pausePanel;
    public string cena; // Nome da cena do menu no build settings (ex: "MenuPrincipal")

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        jogoPausado = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseScreen();
        }
    }

    public void PauseScreen()
    {
        if (isPaused)
        {
            isPaused = false;
            jogoPausado = false;
            Time.timeScale = 1f;
            pausePanel.SetActive(false);
        }
        else
        {
            isPaused = true;
            jogoPausado = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
        }
    }

    public void BackToMenu()
    {
        // Antes de trocar de cena, garante que tudo volta ao normal
        Time.timeScale = 1f;
        jogoPausado = false;

        SceneManager.LoadScene(cena); // A cena deve estar adicionada no Build Settings
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}