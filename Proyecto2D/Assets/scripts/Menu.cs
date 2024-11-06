using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Menu : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI capitalUI;
    [SerializeField] Animator anim;
    private bool menu_abierto= true;
    void OnGUI()
    {
        capitalUI.text= GameController.main.capital.ToString();
    }

    public void ToggleMenu(){
        menu_abierto = !menu_abierto;
        anim.SetBool("Menu_open", menu_abierto);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
