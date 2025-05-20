using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    private bool isPaused = false;
    public static bool jogoPausado = false;

    public GameObject pausePanel;
    public string cena;

    [SerializeField] private GameObject _codexButton;
    [SerializeField] private GameObject _mixButton;
    [SerializeField] private GameObject _progressButton;
    [SerializeField] private GameObject _pauseButton;
    [SerializeField] private GameObject _flaskButton;

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
            ChangeUiStatus();
        }
        else
        {
            isPaused = true;
            jogoPausado = true;
            Time.timeScale = 0f;
            pausePanel.SetActive(true);
            ChangeUiStatus();
        }
    }

    private void ChangeUiStatus()
    {
        if (_codexButton.activeInHierarchy == true && _mixButton.activeInHierarchy == true && _progressButton.activeInHierarchy == true && _pauseButton.activeInHierarchy == true && _flaskButton.activeInHierarchy == true)
        {
            _codexButton.SetActive(false);
            _progressButton.SetActive(false);
            _mixButton.SetActive(false);
            _pauseButton.SetActive(false);
            _flaskButton.SetActive(false);
        }
        else
        {
            _codexButton.SetActive(true);
            _progressButton.SetActive(true);
            _mixButton.SetActive(true);
            _pauseButton.SetActive(true);
            _flaskButton.SetActive(true);
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