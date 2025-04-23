using UnityEngine;
using UnityEngine.UI;

public class VolumeControllerComIcones : MonoBehaviour
{
    private int estadoVolume = 0;

    [SerializeField] private Image botaoImagem;
    [SerializeField] private Sprite iconeVolumeAlto;
    [SerializeField] private Sprite iconeVolumeMedio;
    [SerializeField] private Sprite iconeVolumeMudo;

    private AudioSource audioSource;

    void Start()
    {
        // Pega dinamicamente o AudioSource do objeto que tem o script Persistente
        var persistente = FindObjectOfType<Persistente>();
        if (persistente != null)
        {
            audioSource = persistente.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogWarning("Nenhum objeto com script Persistente encontrado.");
        }
    }

    public void AlterarVolume()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource não encontrado.");
            return;
        }

        estadoVolume++;
        if (estadoVolume > 2)
        {
            estadoVolume = 0;
        }

        switch (estadoVolume)
        {
            case 0:
                audioSource.volume = 1.0f;
                botaoImagem.sprite = iconeVolumeAlto;
                break;
            case 1:
                audioSource.volume = 0.5f;
                botaoImagem.sprite = iconeVolumeMedio;
                break;
            case 2:
                audioSource.volume = 0.0f;
                botaoImagem.sprite = iconeVolumeMudo;
                break;
        }
    }
}