using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : MonoBehaviour
{
    [SerializeField] public int vida=100;
    public static Base main;
    public bool game_over=false;

    public void dmgbase(int _daño){
        vida -= _daño;
    }

    private void Awake(){
        main = this;
    
    }

    void Update()
    {
        if(vida <= 0){
            Debug.Log("Game Over");
            game_over=true;
        }
    }
}
