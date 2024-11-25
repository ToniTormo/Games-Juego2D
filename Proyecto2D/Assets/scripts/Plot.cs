using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Clase Plot, que representa un lugar en el que se puede construir una torreta.
public class Plot : MonoBehaviour
{
    // Componente SpriteRenderer para cambiar el color del plot cuando el cursor está sobre él.
    [SerializeField] private SpriteRenderer sr;

    // Color que se aplica al plot cuando el cursor pasa por encima.
    [SerializeField] private Color hovercolor;

    // Objeto y componentes Torreta, slow y smash construido en el plot.
    private GameObject torreobj;
    private Torreta torre;
    private Slow slow;
    private Smash smash;



    // Color inicial del SpriteRenderer.
    private Color color_inicial;
    
    // Inicialización del color inicial y desactivación del SpriteRenderer.
    void Start()
    {
        sr.enabled = false;
        color_inicial = sr.color;
    }
    
    // Método que se ejecuta cuando el cursor entra en el área del plot.
    void OnMouseEnter()
    {
        sr.color = hovercolor;
        sr.enabled = true;
    }
    
    // Método que se ejecuta cuando el cursor sale del área del plot.
    void OnMouseExit()
    {
        sr.enabled = false;
        sr.color = color_inicial;
    }

    // Método que se ejecuta al hacer clic sobre el plot.
    void OnMouseDown()
    {
        // Evita la interacción si el cursor está sobre la UI.
        if (UIManager.main.IsHoveringUI()) return;

        // Si ya hay una torreta en el plot, abre la interfaz de mejora de la torreta.
        if (torreobj != null){
            try{
            torre.OpenUpgrade();
            return;
        
            }catch{}
            try{
                slow.OpenUpgrade();
                return;
            }catch{}
            try{

                smash.OpenUpgrade();
                return;

            }catch{}
        }

        

        // Obtiene la torreta seleccionada actualmente en BuildManager.
        Defensa torre_nueva = BuildManager.main.GetTorreta();

        // Verifica si el jugador tiene suficiente capital para construir la torreta.
        if (torre_nueva.coste > GameController.main.capital){
            // No es posible construir debido a falta de capital.
            return;
        }

        // Descuenta el costo de la torreta del capital del jugador.
        GameController.main.gastar(torre_nueva.coste);

        // Instancia la torreta en la posición del plot.
        torreobj = Instantiate(torre_nueva.prefab, transform.position, Quaternion.identity);
        try{
            torre = torreobj.GetComponent<Torreta>();
        }catch{}
        try{
            slow = torreobj.GetComponent<Slow>();
        }catch{}
        try{
            smash = torreobj.GetComponent<Smash>();
        }catch{}
        
    }


}
