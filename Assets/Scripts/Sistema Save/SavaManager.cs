using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static string caminhoArquivo => Application.persistentDataPath + "/save.json";

    public static void SalvarProgresso(ProgressoDoJogador progresso)
    {
        string json = JsonUtility.ToJson(progresso, true);
        File.WriteAllText(caminhoArquivo, json);
        Debug.Log("Progresso salvo em: " + caminhoArquivo);
    }

    public static ProgressoDoJogador CarregarProgresso()
    {
        if (File.Exists(caminhoArquivo))
        {
            string json = File.ReadAllText(caminhoArquivo);
            return JsonUtility.FromJson<ProgressoDoJogador>(json);
        }
        else
        {
            Debug.Log("Nenhum progresso salvo encontrado, criando novo progresso.");
            return new ProgressoDoJogador();
        }
    }

    public static HashSet<string> CarregarMisturasDescobertas()
    {
        ProgressoDoJogador progresso = CarregarProgresso();
        return new HashSet<string>(progresso.misturasDescobertas);
    }

    public static void DeletarProgresso()
    {
        if (File.Exists(caminhoArquivo))
        {
            File.Delete(caminhoArquivo);
            Debug.Log("Progresso deletado.");
        }
    }
}