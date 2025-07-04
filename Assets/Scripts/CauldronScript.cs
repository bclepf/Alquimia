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

    [SerializeField] PauseManager pauseManager;

    private string _raridadeResultado;
    private string _nomeResultado;

    private ProgressoDoJogador progressoAtual;

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
    [Header("Mixes")]
    [Header("Prefabs")]
    [SerializeField] private GameObject _prefabCesio;
    [SerializeField] private GameObject _prefabGolem;
    [SerializeField] private GameObject _prefabFalho;
    [SerializeField] private GameObject _prefabZumbi;
    [SerializeField] private GameObject _prefabDragao;
    [SerializeField] private GameObject _prefabPedra;
    [SerializeField] private GameObject _prefabCabelo;
    [SerializeField] private GameObject _prefabQuimera;
    [SerializeField] private GameObject _prefabFilosofal;
    [SerializeField] private GameObject _prefabArvore;
    [SerializeField] private GameObject _prefabHollow;
    [SerializeField] private GameObject _prefabDeus;
    [SerializeField] private GameObject _prefabCapivara;
    [SerializeField] private GameObject _prefabEstrela;
    [SerializeField] private GameObject _prefabDinheiro;
    [SerializeField] private GameObject _prefabEspada;
    [SerializeField] private GameObject _prefabCoroa;
    [SerializeField] private GameObject _prefabCalice;
    [SerializeField] private GameObject _prefabMedalhao;
    [Header("Lettering Elements")]
    [SerializeField] private Text _letreiroText;
    [Header("Codex")]
    [SerializeField] private CodexScript codexScript;

    private Dictionary<string, GameObject> mapaDePrefabs = new Dictionary<string, GameObject>();
    private Dictionary<string, (string, string)> receitas = new Dictionary<string, (string, string)>
        {
            { "Ácido+Energia+Tritio", ("Césio", "Lendária") },
            { "Energia+Solução de Ferro+Vida Líquida", ("Golem", "Épica") },
            { "Elixir Puro+Sangue+Vida Líquida",("Zumbi Pequeno", "Épica") },
            { "Lava Fria+Osso de Dragão+Pedra Misteriosa", ("Dragão Filhote","Épica") },
            { "Água+Lava Fria", ("Pedra","Rara") },
            { "Elixir Puro+Pena de Fenix+Runa", ("Cabelo","Lendária") },
            { "Baba de Yeti+Osso de Dragão+Sangue de Basilisco", ("Quimera","Épica") },
            { "Pedra Misteriosa+Perola+Runa", ("Pedra Filosofal","Rara") },
            { "Água+Ar+Folha", ("Árvore","Rara") },
            { "Elixir+Folha+Lava Fria", ("Hollow","Épica") },
            { "Energia+Fanta Uva+Sangue", ("Deus do Medo","Épica") },
            { "Água+Ar+Vida Líquida", ("Capivara","Rara") },
            { "Energia+Tritio+Tubo de Raios", ("Estrela","Épica") },
            { "Petroleo", ("Dinheiro","Épica") },
            { "Extrato de Sakura+Sangue de Basilisco+Solução de Ferro", ("Espada","Épica") },
            { "Ar+Baba de Yeti+Tubo de Raios", ("Coroa","Épica") },
            { "Energia+Lava Fria+Solução de Ferro", ("Cálice","Épica") },
            { "Elixir Puro+Gota de Sangue+Pedra Misteriosa", ("Medalhão","Épica") },
        };
    #endregion

    #region Cesio
    [Header("Césio")]
    [SerializeField] private Transform _cesioPosition;
    [SerializeField] private AudioClip _cesioSound;
    [SerializeField] private Color _corInicialCesio;
    [SerializeField] private Color _corFinalCesio;
    #endregion

    #region Cabelo
    [Header("Cabelo")]
    [SerializeField] private Transform _pedrinhoPosition;
    [SerializeField] private GameObject _pedrinhoObj;
    #endregion
    private void Start()
    {
        if (_restartLayer != null)
            _restartLayer.SetActive(false);  // Sempre desativa o restartLayer ao iniciar a cena

        if (_flasks != null)
            _flasks.SetActive(true);         // Sempre ativa os frascos ao iniciar

        progressoAtual = SaveManager.CarregarProgresso();
        codexScript.AtualizarCodex(new HashSet<string>(progressoAtual.misturasDescobertas));

        mapaDePrefabs["Mistura Falha"] = _prefabFalho;
        mapaDePrefabs["Césio"] = _prefabCesio;
        mapaDePrefabs["Golem"] = _prefabGolem;
        mapaDePrefabs["Zumbi Pequeno"] = _prefabZumbi;
        mapaDePrefabs["Dragão Filhote"] = _prefabDragao;
        mapaDePrefabs["Pedra"] = _prefabPedra;
        mapaDePrefabs["Cabelo"] = _prefabCabelo;
        mapaDePrefabs["Árvore"] = _prefabArvore;
        mapaDePrefabs["Quimera"] = _prefabQuimera;
        mapaDePrefabs["Pedra Filosofal"] = _prefabFilosofal;
        mapaDePrefabs["Hollow"] = _prefabHollow;
        mapaDePrefabs["Deus do Medo"] = _prefabDeus;
        mapaDePrefabs["Capivara"] = _prefabCapivara;
        mapaDePrefabs["Estrela"] = _prefabEstrela;
        mapaDePrefabs["Dinheiro"] = _prefabDinheiro;
        mapaDePrefabs["Espada"] = _prefabEspada;
        mapaDePrefabs["Coroa"] = _prefabCoroa;
        mapaDePrefabs["Cálice"] = _prefabCalice;
        mapaDePrefabs["Medalhão"] = _prefabMedalhao;
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

    #region Save
    private void RegistrarMistura(string nomeMistura)
    {
        if (!progressoAtual.misturasDescobertas.Contains(nomeMistura))
        {
            progressoAtual.misturasDescobertas.Add(nomeMistura);
            SaveManager.SalvarProgresso(progressoAtual);
        }

        codexScript.AtualizarCodex(new HashSet<string>(progressoAtual.misturasDescobertas));
    }
    #endregion
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
                    AtualizarLetreiroUltimoIngrediente(nomePocao);
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

    public void StartMix()
    {
        StartCoroutine(ProcessarMistura());
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

            RegistrarMistura(_nomeResultado);


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

            case "Cabelo":
                _pedrinhoObj.SetActive(false);
                if (mapaDePrefabs.TryGetValue("Cabelo", out GameObject prefabCabelo))
                {
                    Instantiate(prefabCabelo, _pedrinhoPosition.position, Quaternion.identity);
                }
                Debug.Log("EasterEgg Cabelo");
                break;

            default:
                Debug.LogWarning($"Easter egg desconhecido: {easteregg}. Invocando normalmente...");
                SummonResult();
                break;
        }

        StartCoroutine(ResetGame());
    }

    #endregion

    #region Letreiro
    private void AtualizarLetreiroUltimoIngrediente(string nomeIngrediente)
    {
        if (_letreiroText == null) return;

        _letreiroText.text = $"{nomeIngrediente}";
    }

    #endregion
    private IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(3f);
        ingredientesNoCaldeirao.Clear();
        _restartLayer.SetActive(true);
        _flasks.SetActive(false);
        pauseManager.ChangeUiStatus();
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

    #region Codex
    public void OnClickAbrirCodex()
    {
        codexScript.AbrirCodex();
    }
    #endregion
}
