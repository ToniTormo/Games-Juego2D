using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Upgrade : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler {
    
    public bool mouse_over=false;
    

    public void OnPointerEnter(PointerEventData eventData){
        mouse_over=true;
        UIManager.main.SetHovering(true);
    }
    public void OnPointerExit(PointerEventData eventData){
        mouse_over=false;
        UIManager.main.SetHovering(false);
        gameObject.SetActive(false);

    }

    
}
