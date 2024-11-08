using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Clase Upgrade que implementa los interfaces IPointerEnterHandler e IPointerExitHandler.
// Estos interfaces permiten detectar cuando el cursor entra o sale de la UI.
public class Upgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Variable para indicar si el cursor está actualmente sobre el elemento.
    public bool mouse_over = false;

    // Método que se llama automáticamente cuando el cursor entra en el área del elemento.
    public void OnPointerEnter(PointerEventData eventData){
        mouse_over = true;

        // Cambia el estado de hovering en UIManager a true.
        UIManager.main.SetHovering(true);
    }

    // Método que se llama automáticamente cuando el cursor sale del área del elemento.
    public void OnPointerExit(PointerEventData eventData){
        mouse_over = false;

        // Cambia el estado de hovering en UIManager a false y desactiva el objeto.
        UIManager.main.SetHovering(false);
        gameObject.SetActive(false);
    }
}
