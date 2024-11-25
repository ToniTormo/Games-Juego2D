using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Clase Menu que controla la interfaz del menú en el juego.
public class Menu : MonoBehaviour
{
    // Referencia al componente TextMeshProUGUI que muestra el capital del jugador.
    [SerializeField] TextMeshProUGUI capitalUI;

    // Referencia al Animator que controla las animaciones del menú.
    [SerializeField] Animator anim;

    // Variable que indica si el menú está abierto o cerrado.
    private bool menu_abierto = true;

    // Método OnGUI, que actualiza la UI del capital en cada cuadro.
    void OnGUI()
    {
        // Actualiza el texto de capitalUI con el capital actual del jugador obtenido desde GameController.
        capitalUI.text = GameController.main.capital.ToString();
    }

    // Método para alternar la visibilidad del menú.
    public void ToggleMenu(){
        // Cambia el estado de abierto a cerrado (o viceversa).
        menu_abierto = !menu_abierto;

        // Actualiza el parámetro "Menu_open" del Animator para mostrar u ocultar el menú.
        anim.SetBool("Menu_open", menu_abierto);
    }

    
    void Update()
    {
        capitalUI.text = GameController.main.capital.ToString();
    }
}
