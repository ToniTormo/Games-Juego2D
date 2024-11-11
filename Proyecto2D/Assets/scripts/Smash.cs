using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Smash : MonoBehaviour
{

    // Variables públicas para ser asignadas en el inspector de Unity
    [SerializeField] private LayerMask enemyMask;  // Máscara de capa para identificar los enemigos
    [SerializeField] private float rango = 5f;  // Rango de acción para detectar enemigos
    [SerializeField] private float veldis = 4f; // Velocidad de disparo, o frecuencia con la que se activa la congelación
    [SerializeField] private int dmg = 1000;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite new_sr;
    private Sprite sr_copia;



    private float tiempo_disparo;  // Temporizador para controlar la frecuencia de disparo


    private void Aplastar(){
        // Realiza un CircleCast para detectar los enemigos dentro del rango
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position, 0f, enemyMask);

        // Si se detectaron enemigos
        if (hits.Length > 0)
        {
            // Recorre todos los enemigos detectados
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];  // Toma el enemigo actual

                hit.transform.GetComponent<Vida>().recibir_daño(dmg);
                
            }

            sr.sprite=new_sr;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        sr_copia=sr.sprite;
    }

    // Update is called once per frame
    void Update()
    {
       // Incrementa el temporizador de disparo con el tiempo que ha pasado desde el último frame
        tiempo_disparo += Time.deltaTime;

        if (tiempo_disparo >= veldis/2){
            sr.sprite=sr_copia;
        }

        // Si ha pasado suficiente tiempo (en función de veldis), ejecuta la función Congelar
        if (tiempo_disparo >= 1f / veldis)
        {
            Aplastar();
            // Reinicia el temporizador para esperar el siguiente disparo
            tiempo_disparo = 0f;
        } 
    }
    private void OnDrawGizmosSelected()
    {
        // Establece el color de los Gizmos a cian
        Handles.color = Color.cyan;

        // Dibuja un círculo que representa el rango de acción del Slow
        Handles.DrawWireDisc(transform.position, transform.forward, rango);
    }
}