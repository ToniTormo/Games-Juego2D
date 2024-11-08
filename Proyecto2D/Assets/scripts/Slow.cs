using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Slow : MonoBehaviour
{
    // Variables públicas para ser asignadas en el inspector de Unity
    [SerializeField] private LayerMask enemyMask;  // Máscara de capa para identificar los enemigos
    [SerializeField] private float rango = 5f;  // Rango de acción para detectar enemigos
    [SerializeField] private float veldis = 4f;  // Velocidad de disparo, o frecuencia con la que se activa la congelación
    [SerializeField] private float freezeTime = 1f;  // Tiempo durante el cual se congela la velocidad del enemigo
    private float tiempo_disparo;  // Temporizador para controlar la frecuencia de disparo

    // Método para congelar enemigos dentro del rango
    private void Congelar()
    {
        // Realiza un CircleCast para detectar los enemigos dentro del rango
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position, 0f, enemyMask);

        // Si se detectaron enemigos
        if (hits.Length > 0)
        {
            // Recorre todos los enemigos detectados
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];  // Toma el enemigo actual

                // Obtiene el componente Move del enemigo para modificar su velocidad
                Move em = hit.transform.GetComponent<Move>();
                if (em != null)  // Verifica que el enemigo tenga un componente Move
                {
                    // Cambia la velocidad del enemigo a la mitad
                    em.Cambio_speed(0.5f);

                    // Inicia la coroutine para restablecer la velocidad del enemigo después de un tiempo
                    StartCoroutine(RestablecerVelocidad(em));
                }
            }
        }
    }

    // Coroutine que restablece la velocidad del enemigo después de un cierto tiempo
    private IEnumerator RestablecerVelocidad(Move em)
    {
        // Espera el tiempo especificado por freezeTime
        yield return new WaitForSeconds(freezeTime);

        // Restaura la velocidad del enemigo al valor original
        em.Reset_speed();
    }

    // El método Start se ejecuta al inicio, no está siendo utilizado en este caso
    void Start()
    {
    }

    // El método Update se ejecuta una vez por frame
    void Update()
    {
        // Incrementa el temporizador de disparo con el tiempo que ha pasado desde el último frame
        tiempo_disparo += Time.deltaTime;

        // Si ha pasado suficiente tiempo (en función de veldis), ejecuta la función Congelar
        if (tiempo_disparo >= 1f / veldis)
        {
            Congelar();
            // Reinicia el temporizador para esperar el siguiente disparo
            tiempo_disparo = 0f;
        }
    }

    // Método para dibujar una visualización del rango de acción en el editor de Unity (cuando el objeto está seleccionado)
    private void OnDrawGizmosSelected()
    {
        // Establece el color de los Gizmos a cian
        Handles.color = Color.cyan;

        // Dibuja un círculo que representa el rango de acción del Slow
        Handles.DrawWireDisc(transform.position, transform.forward, rango);
    }
}
