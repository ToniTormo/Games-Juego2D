using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;

    private bool hovering=false;

    void Awake()
    {
       main=this; 
    }

    public void SetHovering(bool state){
        hovering=state;
    }

    public bool IsHoveringUI(){
        return hovering;
    }
}
