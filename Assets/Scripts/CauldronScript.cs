using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CauldronScript : MonoBehaviour
{
    private List<string> ingredientesNoCaldeirao = new List<string>();
    private const int limiteIngredientes = 3;
    private bool estaMisturando = false;

    private string _raridadeResultado;
    private string _nomeResultado;

    [Header("General Items")]
    [SerializeField] private GameObject _restartLayer;
    [SerializeField] private GameObject _flasks;
    [SerializeField] private Image _filtroGradiente;
    [SerializeField] private Transform _bolhasPosition;
    [SerializeField] private Transform _pontoDeSpawn;
    [Header("Audio Sources")]
    [SerializeField] private AudioClip _blopMix;
    [SerializeField] private AudioClip _mixingSound;
    [Header("Particles")]
    [SerializeField] private ParticleSystem efeitoMistura;
    #region Misturas
    [Header("Misturas")]
    [Header("Prefabs")]
    [SerializeField] private GameObject _prefabCesio;
    [SerializeField] private GameObject _prefabGolem;
    [SerializeField] private GameObject _prefabFalho;
    [SerializeField] private GameObject _prefabZumbi;
    [SerializeField] private GameObject _prefabDragao;
    [SerializeField] private GameObject _prefabObsidian;

    private Dictionary<string, GameObject> mapaDePrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, (string, string)> receitas = new Dictionary<string, (string, string)>
    {
        { "Ácido+Energia+Tritio", ("Césio", "Lendária") },
        { "Energia+Solução de Ferro+Vida Líquida", ("Golem", "Épica") },
        { "Elixir Puro+Sangue+Vida Líquida",("Zumbi Pequeno", "Épica") },
        { "Lava Fria+Osso de Dragão+Pedra Misteriosa", ("Dragão Filhote","Épica") },
        { "Água+Lava Fria", ("Obsidiana","Rara") },

    };
    #endregion

    #region Cesio
    [Header("Césio")]
    [SerializeField] private Transform _cesioPosition;
    [SerializeField] private AudioClip _cesioSound;
    [SerializeField] private Color _corInicialCesio;
    [SerializeField] private Color _corFinalCesio;

    #endregion

    private void Start()
    {
        mapaDePrefabs["Mistura Falha"] = _prefabFalho;
        mapaDePrefabs["Césio"] = _prefabCesio;
        mapaDePrefabs["Golem"] = _prefabGolem;
        mapaDePrefabs["Zumbi Pequeno"] = _prefabZumbi;
        mapaDePrefabs["Dragão Filhote"] = _prefabDragao;
        mapaDePrefabs["Obsidiana"] = _prefabObsidian;
        
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
                    AudioSource.PlayClipAtPoint(_blopMix, transform.position);
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

        if (efeitoMistura != null) Instantiate(efeitoMistura, _bolhasPosition.position, Quaternion.identity);
        if (_mixingSound != null) AudioSource.PlayClipAtPoint(_mixingSound, transform.position);

        Debug.Log("Misturando...");

        yield return new WaitForSeconds(4f);

        Misturar();
        estaMisturando = false;

    }

    private void Misturar()
    {
        ingredientesNoCaldeirao.Sort();
        string combinacao = string.Join("+", ingredientesNoCaldeirao);

        if (receitas.ContainsKey(combinacao))
        {
            _nomeResultado = receitas[combinacao].Item1;
            _raridadeResultado = receitas[combinacao].Item2;

            if (_raridadeResultado != "Lendária")
            {
                Debug.Log($"Mistura feita! Você criou uma {_nomeResultado} ({_raridadeResultado})");
                SummonResult();
            }
            else
            {
                SummonEasterEgg(_nomeResultado);
            }
        }
        else
        {
            _nomeResultado = "Mistura Falha";
            Debug.Log("Sua mistura resultou e um montuado de poeira");
            SummonResult();
        }
    }

    private void SummonResult()
    {
        // Spawna o resultado
        if (mapaDePrefabs.TryGetValue(_nomeResultado, out GameObject prefab))
        {
            Instantiate(prefab, _pontoDeSpawn.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Prefab não encontrado para: " + _nomeResultado);
        }

        StartCoroutine(ResetGame());
    }



    #region EasterEggs

    private void SummonEasterEgg(string easteregg)
    {
        switch (easteregg)
        {
            case "Césio":
                if (_cesioSound != null)
                    AudioSource.PlayClipAtPoint(_cesioSound, transform.position);
                StartCoroutine(TrocarCorFundo(_corInicialCesio, _corFinalCesio));

                if (mapaDePrefabs.TryGetValue("Césio", out GameObject prefabCesio))
                {
                    Instantiate(prefabCesio, _cesioPosition.position, Quaternion.identity);
                }

                Debug.Log("EasterEgg Césio");
                break;

            default:
                Debug.LogWarning($"Easter egg desconhecido: {easteregg}. Invocando normalmente...");
                SummonResult();
                break;
        }

        StartCoroutine(ResetGame());
    }

    #endregion

    private IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(3f);


        ingredientesNoCaldeirao.Clear();
        _restartLayer.SetActive(true);
        _flasks.SetActive(false);
    }

    private IEnumerator TrocarCorFundo(Color corInicial, Color corFinal)
    {
        Debug.LogWarning("Iniciando Troca de cor de fundo");
        float duracao = 8f;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.deltaTime;
            float t = tempo / duracao;
            _filtroGradiente.color = Color.Lerp(corInicial, corFinal, t);
            yield return null;
        }

        _filtroGradiente.color = corFinal;
    }
    #endregion
}
