using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskScript : MonoBehaviour
{
    [SerializeField] private string nomePocao;
    [SerializeField] private string raridade;
    private bool _isDragging = false;
    private Vector3 _offset;
    private Rigidbody2D _rb;
    private float _minX, _maxX, _minY, _maxY;

    public string GetNomePocao() => nomePocao;
    public string GetRaridade() => raridade;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_rb == null)
        {
            Debug.LogWarning("Rigidbody2D não encontrado no objeto " + gameObject.name + ", mas o código já tenta usar o existente.");
        }

        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));

        _minX = bottomLeft.x;
        _maxX = topRight.x;
        _minY = bottomLeft.y;
        _maxY = topRight.y;
    }

    void OnMouseDown()
    {
        _isDragging = true;
        if (_rb != null)
        {
            _rb.gravityScale = 0;
        }

        _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void Update()
    {
        if (_isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
            newPosition.z = 0;

            newPosition.x = Mathf.Clamp(newPosition.x, _minX, _maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, _minY, _maxY);

            transform.position = newPosition;
        }


        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            if (_rb != null)
            {
                _rb.gravityScale = 1;
            }
        }
    }
}
