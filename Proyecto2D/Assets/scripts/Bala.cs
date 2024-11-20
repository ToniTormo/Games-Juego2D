using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase Bala, que representa el comportamiento de una bala en el juego.
public class Bala : MonoBehaviour
{
    // Componente Rigidbody2D que se usa para manejar la física de la bala.
    [SerializeField] private Rigidbody2D rb;

    // Velocidad de la bala.
    [SerializeField] private float velocidad = 3f;

    // Daño que la bala inflige al impactar.
    [SerializeField] private int dmg = 1;

    // Variable que guarda el objetivo al que la bala se dirige.
    private Transform target;

    // Tiempo de vida de la bala.
    private float tiempoVida = 5f;

    // Método público para establecer el objetivo de la bala.
    public void fijar_objetivo(Transform objetivo){
        target = objetivo;
    }
    
    // Método FixedUpdate, llamado a intervalos de tiempo constantes, para mover la bala.
    void FixedUpdate()
    {
        // Si no hay objetivo, la bala no se mueve.
        if (!target) return;

        // Calcula la dirección hacia el objetivo y mueve la bala hacia él.
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * velocidad;
    }
    
    // Evento que se activa al colisionar con otro objeto.
    void OnCollisionEnter2D(Collision2D other)
    {
        // Si el objeto colisionado tiene el componente Vida, le inflige daño.
        other.gameObject.GetComponent<Vida>().recibir_daño(dmg);

        // Destruye la bala tras impactar.
        Destroy(gameObject);
    }

    // Método Start, llamado al iniciar el script. Actualmente no contiene funcionalidad.
    void Start()
    {
        
    }

    // Método Update, llamado una vez por cuadro. Actualmente no contiene funcionalidad.
    void Update()
    {
       // Reduce el tiempo de vida con el paso del tiempo.
        tiempoVida -= Time.deltaTime;

        // Si el tiempo de vida llega a 0, destruye la bala.
        if (tiempoVida <= 0)
        {
            Destroy(gameObject);
        } 
    }
}
