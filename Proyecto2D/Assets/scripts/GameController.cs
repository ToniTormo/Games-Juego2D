using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definición de la clase GameController que hereda de MonoBehaviour, 
// lo cual la convierte en un componente que puede añadirse a objetos en Unity.
public class GameController : MonoBehaviour
{
    // Variable estática para que haya una única instancia accesible desde otras clases.
    public static GameController main;

    // Array de Transform que representa un camino o ruta, posiblemente para un personaje o elemento en el juego.
    public Transform[] path;

    // Transform para el punto de inicio.
    public Transform inicio;

    // Variable para almacenar el capital actual del jugador.
    public int capital;

    // Método Awake, llamado cuando la instancia se inicializa. 
    // Aquí se asigna la instancia actual a la variable estática 'main' para hacerla accesible globalmente.
    private void Awake(){
        main = this;
    }
  
    // Método Start, llamado al inicio de la ejecución. 
    // Inicializa el capital en 100 al comenzar el juego.
    void Start()
    {
        capital = 100;      
    }

    // Método para incrementar el capital en una cantidad dada.
    public void cobrar(int cantidad){
        capital += cantidad;
    }

    // Método para intentar reducir el capital en una cantidad dada.
    // Si el capital actual es suficiente, se resta y devuelve 'true' indicando éxito.
    // Si no es suficiente, devuelve 'false' indicando que la transacción no es posible.
    public bool gastar(int cantidad){
        if(cantidad <= capital){
            // La cantidad es suficiente, por lo que se resta del capital.
            capital -= cantidad;
            return true;
        } else {
            // La cantidad no es suficiente.
            return false;
        }
    }
}
