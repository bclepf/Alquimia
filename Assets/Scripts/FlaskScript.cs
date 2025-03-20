using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskScript : MonoBehaviour
{
    private static GameObject selectedObject = null;
    private static Vector3 offset;
    private static Rigidbody2D selectedRb;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Quando o botão do mouse for pressionado
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null) // Verifica se clicamos em algum objeto
            {
                selectedObject = hit.collider.gameObject; // Define o objeto selecionado
                selectedRb = selectedObject.GetComponent<Rigidbody2D>();

                if (selectedRb != null)
                {
                    selectedRb.gravityScale = 0; // Desativa a gravidade ao pegar o objeto
                    offset = selectedObject.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
        }

        if (selectedObject != null && Input.GetMouseButton(0)) // Arrasta apenas o objeto selecionado
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            newPosition.z = 0; // Mantém no mesmo plano
            selectedObject.transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null) // Solta o objeto
        {
            if (selectedRb != null)
            {
                selectedRb.gravityScale = 1; // Ativa a gravidade ao soltar
            }

            selectedObject = null; // Reseta a seleção
            selectedRb = null;
        }
    }
}
