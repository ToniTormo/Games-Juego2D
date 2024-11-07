using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hovercolor;
    //public Renderer render;


    private GameObject torreobj;
    private Torreta torre;
    private Color color_inicial;
    
    
    void Start()
    {
        sr.enabled= false;
        color_inicial=sr.color;
    }
    
    void OnMouseEnter()
    {
        sr.color=hovercolor;
        sr.enabled= true;
    }
    
    void OnMouseExit()
    {
        sr.enabled= false;
        sr.color=color_inicial;
    }

    void OnMouseDown()
    {
        if (UIManager.main.IsHoveringUI()) return;

        if (torreobj != null){
            torre.OpenUpgrade();

            return;

        }
       Defensa torre_nueva= BuildManager.main.GetTorreta();

       if (torre_nueva.coste > GameController.main.capital){
        //No puedes comprar
        return;
       }

       GameController.main.gastar(torre_nueva.coste);
       torreobj=Instantiate(torre_nueva.prefab,transform.position,Quaternion.identity);
       torre=torreobj.GetComponent<Torreta>();
    
       
    }


    void Update()
    {
        
    }
}
