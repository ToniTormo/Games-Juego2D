using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase Vida que controla la salud de un objeto (posiblemente un enemigo).
public class Vida : MonoBehaviour
{
    // Puntos de vida del objeto.
    [SerializeField] private int puntos = 2;

    // Recompensa otorgada al jugador cuando este objeto muere.
    [SerializeField] private int reward = 50;

    // Indica si el objeto está muerto para evitar múltiples ejecuciones del proceso de muerte.
    private bool dead = false;

    // Método para recibir daño. Disminuye los puntos de vida y, si llegan a cero o menos, maneja la muerte.
    public void recibir_daño(int dmg){
        puntos -= dmg;

        // Si los puntos de vida son 0 o menos y el objeto aún no está marcado como muerto:
        if (puntos <= 0 && !dead){
            // Invoca un evento de muerte.
            spawner.OnEnemigoMuerto.Invoke();

            // Otorga la recompensa al jugador usando el método cobrar de GameController.
            GameController.main.cobrar(reward);

            // Marca el objeto como muerto para evitar duplicación de efectos.
            dead = true;

            // Destruye el objeto en el juego.
            Destroy(gameObject);
            if(spawner.main.oleada >= 5){
                GameController.main.Win();
            }
        }
    }

}
