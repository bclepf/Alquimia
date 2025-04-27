using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodexScript : MonoBehaviour
{
    [SerializeField] private GameObject codexPanel;
    [SerializeField] private List<Text> misturaTexts;
    [SerializeField] private Button botaoProximaPagina;
    [SerializeField] private Button botaoPaginaAnterior;
    [SerializeField] private List<Sprite> spritesMisturas;
    [SerializeField] private List<string> nomesMisturas;

    private Dictionary<string, Sprite> mapaSprites = new Dictionary<string, Sprite>();

    private HashSet<string> misturasDescobertas = new HashSet<string>();
    private int paginaAtual = 0;
    private int itensPorPagina = 4;

    private void Awake()
    {
        for (int i = 0; i < nomesMisturas.Count; i++)
        {
            mapaSprites[nomesMisturas[i]] = spritesMisturas[i];
        }
    }

    private void Start()
    {
        AtualizarCodex(SaveManager.CarregarMisturasDescobertas());
    }

    [System.Serializable]
    public struct SlotMistura
    {
        public Image imagemMistura;
        public Text nomeMistura;
    }

    [SerializeField] private List<SlotMistura> slotsMisturas;
    public void AtualizarCodex(HashSet<string> misturas)
    {
        misturasDescobertas = new HashSet<string>(misturas);
        paginaAtual = 0;
        AtualizarPagina();
    }

    public void AbrirCodex()
    {
        codexPanel.SetActive(true);
        AtualizarPagina();
    }

    public void FecharCodex()
    {
        codexPanel.SetActive(false);
    }

    public void ProximaPagina()
    {
        if ((paginaAtual + 1) * itensPorPagina < misturasDescobertas.Count)
        {
            paginaAtual++;
            AtualizarPagina();
        }
    }

    public void PaginaAnterior()
    {
        if (paginaAtual > 0)
        {
            paginaAtual--;
            AtualizarPagina();
        }
    }

    private void AtualizarPagina()
    {
        List<string> listaMisturas = new List<string>(misturasDescobertas);
        int startIndex = paginaAtual * itensPorPagina;

        for (int i = 0; i < slotsMisturas.Count; i++)
        {
            int itemIndex = startIndex + i;
            if (itemIndex < listaMisturas.Count)
            {
                string nomeMistura = listaMisturas[itemIndex];
                slotsMisturas[i].nomeMistura.text = nomeMistura;

                if (mapaSprites.ContainsKey(nomeMistura))
                {
                    slotsMisturas[i].imagemMistura.sprite = mapaSprites[nomeMistura];
                    slotsMisturas[i].imagemMistura.color = Color.white; // Garante que a imagem fique visível
                }
                else
                {
                    slotsMisturas[i].imagemMistura.sprite = null;
                    slotsMisturas[i].imagemMistura.color = new Color(1, 1, 1, 0); // Esconde a imagem se não houver sprite
                }
            }
            else
            {
                slotsMisturas[i].nomeMistura.text = "";
                slotsMisturas[i].imagemMistura.sprite = null;
                slotsMisturas[i].imagemMistura.color = new Color(1, 1, 1, 0);
            }
        }

    }
    public void ApagarProgresso()
    {
        SaveManager.DeletarProgresso();
        Debug.Log("Progresso resetado com sucesso!");
    }
}