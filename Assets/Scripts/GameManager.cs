using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject _flaskSet1;
    [SerializeField] GameObject _flaskSet2;

    public static GameManager instance;
    public void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ChangeFlaksSet()
    {
        if (_flaskSet1.activeInHierarchy == true)
        {
            _flaskSet1.SetActive(false);
            _flaskSet2.SetActive(true);
        }
        else
        {
            _flaskSet1.SetActive(true);
            _flaskSet2.SetActive(false);
        }
    }
}