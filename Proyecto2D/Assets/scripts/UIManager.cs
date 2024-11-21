using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Clase UIManager que administra el estado de la interfaz de usuario en el juego.
public class UIManager : MonoBehaviour
{
    // Instancia estática para que haya un único UIManager accesible globalmente desde otras clases.
    public static UIManager main;

    // Variable que indica si el cursor está sobre la interfaz de usuario.
    private bool hovering = false;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameover_screen;
    [SerializeField] private GameObject victory_screen;
    [SerializeField] TextMeshProUGUI vida_txt;


    public void ShowPauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }


    // Método Awake, que se llama al inicio para asignar la instancia única de UIManager.
    void Awake()
    {
        main = this;
    }


    void Start()
    {
        pauseMenu.SetActive(false);
        gameover_screen.SetActive(false);
        victory_screen.SetActive(false);

    }

    
    void Update()
    {
        if(Base.main.game_over){
            gameover_screen.SetActive(true);
        }
        if(GameController.main.win){
            victory_screen.SetActive(true);
        }
        vida_txt.text = Base.main.vida.ToString();
    }

    // Método para establecer el estado de "hovering" (si el cursor está sobre la interfaz).
    public void SetHovering(bool state){
        hovering = state;
    }

    // Método para verificar si el cursor está sobre la interfaz de usuario.
    public bool IsHoveringUI(){
        return hovering;
    }

}
