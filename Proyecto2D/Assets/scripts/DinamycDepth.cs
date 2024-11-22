using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DynamicDepth : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Ajusta el Sorting Order basado en la posición Y del objeto.
        // Cuanto menor sea el valor de Y, mayor será el orden, colocando el sprite más adelante.
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}
