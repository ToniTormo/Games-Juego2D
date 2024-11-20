using System.Collections;
using UnityEngine;

// Clase Move que controla el movimiento de un objeto a través de puntos en un camino.
public class Move : MonoBehaviour
{
    // Velocidad de movimiento.
    [SerializeField] private float speed = 2f;

    // Componente Rigidbody2D para aplicar la física.
    [SerializeField] private Rigidbody2D rb;

    // El próximo punto objetivo en el camino.
    private Transform target;

    // Índice del punto actual en el camino.
    private int pathIndex = 0;

    // Velocidad base del objeto (sin modificaciones).
    private float baseSpeed;

    [SerializeField] private int ataque=5;
    


    // Método Start, llamado al iniciar el script.
    private void Start()
    {
        // Guarda la velocidad base y establece el primer objetivo en el camino.
        baseSpeed = speed;
        target = GameController.main.path[pathIndex];
    }

    // Método Update, llamado una vez por cuadro.
    private void Update()
    {
        if(Base.main.game_over) return;
        if (GameController.main.paused) return;
        // Verifica si el objeto está cerca del punto objetivo actual.
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            // Si el camino se ha completado, se invoca el evento de muerte del enemigo y se destruye el objeto.
            if (pathIndex >= GameController.main.path.Length)
            {
                spawner.OnEnemigoMuerto.Invoke();
                Base.main.dmgbase(ataque);
                Destroy(gameObject);
                return;
            }
            else
            {
                // Si no se ha completado el camino, establece el próximo punto objetivo.
                target = GameController.main.path[pathIndex];
            }
        }
    }

    // Método FixedUpdate, llamado a intervalos regulares para manejar la física.
    private void FixedUpdate()
    {
        // Calcula la dirección de movimiento hacia el objetivo y aplica la velocidad al Rigidbody.
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    // Método para reducir temporalmente la velocidad.
    public void Cambio_speed(float _speed){
        speed -= _speed;
    }

    // Método para restablecer la velocidad a la velocidad base.
    public void Reset_speed(){
        speed = baseSpeed;
    }
}
