using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase BuildManager, que hereda de MonoBehaviour, para administrar la construcción de objetos (posiblemente torretas) en el juego.
public class BuildManager : MonoBehaviour
{
    // Instancia estática para que haya un único BuildManager accesible globalmente desde otras clases.
    public static BuildManager main;

    // Array privado de tipo Defensa que contendrá diferentes tipos de torretas que el jugador puede construir.
    [SerializeField] private Defensa[] torretas;

    // Variable que representa el índice de la torreta seleccionada actualmente en el array.
    private int torreta_actual = 0;

    // Método Awake para inicializar la instancia única de BuildManager.
    private void Awake(){
        main = this;
    }

    // Método para obtener la torreta actualmente seleccionada en el array de torretas.
    public Defensa GetTorreta(){
        return torretas[torreta_actual];
    }

    // Método para establecer el índice de la torreta actual a uno específico, permitiendo cambiar la selección.
    public void Establecer(int _torreta){
        torreta_actual = _torreta;
    }

    // Método Start, que se llama una vez al iniciar el script. Actualmente no contiene funcionalidad.
    void Start()
    {
        
    }

    // Método Update, que se llama una vez por cuadro. Actualmente no contiene funcionalidad.
    void Update()
    {
        
    }
}
