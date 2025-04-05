using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CauldronScript : MonoBehaviour
{
    private List<string> ingredientesNoCaldeirao = new List<string>();
    private const int limiteIngredientes = 3;  
    private bool estaMisturando = false;        

    [SerializeField] private ParticleSystem efeitoMistura;
    [SerializeField] private AudioSource somMistura;
    [SerializeField] private GameObject _restartLayer;
    [SerializeField] private GameObject _flasks;

    #region Misturas
    [SerializeField] private Transform pontoDeSpawn;
    [SerializeField] private GameObject prefabCesio;
    [SerializeField] private GameObject prefabGolem;
    [SerializeField] private GameObject prefabPocaoEstragada;

    private Dictionary<string, GameObject> mapaDePrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, (string, string)> receitas = new Dictionary<string, (string, string)>
    {
        { "Ácido+Energia+Tritio", ("Césio", "Lendária") },
        { "Energia+Solução de Ferro+Vida Líquida", ("Golem", "Rara") },
        
    };
    #endregion



    private void Start()
    {
        mapaDePrefabs["Césio"] = prefabCesio;
        mapaDePrefabs["Golem"] = prefabGolem;
        mapaDePrefabs["Poção Estragada"] = prefabPocaoEstragada;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))  
        {
            GameManager.instance.ReiniciarCena();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(ProcessarMistura());
        }
    }
    #region Misturas
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (estaMisturando) return;  

        FlaskScript pocao = other.GetComponent<FlaskScript>();

        if (pocao != null)
        {
            string nomePocao = pocao.GetNomePocao();
            if (!ingredientesNoCaldeirao.Contains(nomePocao))  
            {
                if (ingredientesNoCaldeirao.Count < limiteIngredientes)  
                {
                    ingredientesNoCaldeirao.Add(nomePocao);
                    Debug.Log($"Poção adicionada: {nomePocao}");  
                    Destroy(other.gameObject);  

                    if (ingredientesNoCaldeirao.Count == limiteIngredientes)  
                    {
                        StartCoroutine(ProcessarMistura());
                    }
                }
                else
                {
                    Debug.Log("O caldeirão já está cheio!");
                }
            }
            else
            {
                Debug.Log("Essa poção já está no caldeirão!");
            }
        }
    }

    private IEnumerator ProcessarMistura()
    {
        estaMisturando = true;  

        if (efeitoMistura != null) efeitoMistura.Play();
        if (somMistura != null) somMistura.Play();

        Debug.Log("Misturando...");

        yield return new WaitForSeconds(2f);  

        Misturar();  
        estaMisturando = false;

    }

    private void Misturar()
    {
        ingredientesNoCaldeirao.Sort();
        string combinacao = string.Join("+", ingredientesNoCaldeirao);

        string nomeResultado;

        if (receitas.ContainsKey(combinacao))
        {
            nomeResultado = receitas[combinacao].Item1;
            string raridadeResultado = receitas[combinacao].Item2;

            Debug.Log($"Mistura feita! Você criou uma {nomeResultado} ({raridadeResultado})");
        }
        else
        {
            nomeResultado = "Poção Estragada";
            Debug.Log("A mistura falhou! Você criou uma Poção Estragada!");
        }

        

        // Spawna o resultado
        if (mapaDePrefabs.TryGetValue(nomeResultado, out GameObject prefab))
        {
            Instantiate(prefab, pontoDeSpawn.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Prefab não encontrado para: " + nomeResultado);
        }

        StartCoroutine(ResetGame());
        
    }

    private IEnumerator ResetGame() 
    {
        yield return new WaitForSeconds(3f);


        ingredientesNoCaldeirao.Clear();
        _restartLayer.SetActive(true);
        _flasks.SetActive(false);
    }
    #endregion
}
