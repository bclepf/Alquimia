using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskScript : MonoBehaviour
{
    private bool _isDragging = false;
    private Vector3 _offset;
    private Rigidbody2D _rb;
    private float _minX, _maxX, _minY, _maxY;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        // Pegamos os limites da tela com base na câmera principal
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Definimos os limites de movimento
        _minX = bottomLeft.x;
        _maxX = topRight.x;
        _minY = bottomLeft.y;
        _maxY = topRight.y;
    }

    void OnMouseDown()
    {
        _isDragging = true;
        _rb.gravityScale = 0; // Desativa a gravidade enquanto arrasta
        _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        if (_isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
            newPosition.z = 0; // Mantém no mesmo plano

            // Mantém o objeto dentro dos limites da câmera
            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, _minY, _maxY);

            transform.position = newPosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            _rb.gravityScale = 1; // Ativa a gravidade ao soltar
        }
    }
}
