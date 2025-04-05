using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class CauldronScript : MonoBehaviour
{
    private List<string> ingredientesNoCaldeirao = new List<string>();
    private const int limiteIngredientes = 3;  
    private bool estaMisturando = false;        

    [SerializeField] private ParticleSystem efeitoMistura;
    [SerializeField] private AudioSource somMistura;

    private Dictionary<string, (string, string)> receitas = new Dictionary<string, (string, string)>
    {
        { "pocao de cura+Poção de Fogo+pocao de mana", ("Poção de Cura Flamejante", "Comum") },
        { "Poção de Regeneração+Poção de Velocidade", ("Poção de Aceleração Regenerativa", "Rara") },
        { "Poção de Força+Poção de Imunidade", ("Poção de Invencibilidade", "Lendária") },
        { "Poção de Velocidade+Poção de Energia", ("Poção de Super Velocidade", "Rara") },
        { "Poção de Força+Poção de Cura+Poção de Imunidade", ("Poção Suprema de Resistência", "Lendária") }
    };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))  
        {
            ReiniciarCena();
        }
    }

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

        if (receitas.ContainsKey(combinacao))
        {
            string nomeResultado = receitas[combinacao].Item1;
            string raridadeResultado = receitas[combinacao].Item2;

            Debug.Log($"Mistura feita! Você criou uma {nomeResultado} ({raridadeResultado})");
        }
        else
        {
            Debug.Log("A mistura falhou! Você criou uma Poção Estragada!");
        }

        ingredientesNoCaldeirao.Clear();  
    }

    private void ReiniciarCena()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
