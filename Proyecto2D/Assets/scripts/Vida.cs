using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{
    [SerializeField] private int puntos=2;
    [SerializeField] private int reward=50;

    private bool dead=false;

    public void recibir_daño(int dmg){
        puntos-= dmg;
        if (puntos <=0 && !dead){
            spawner.OnEnemigoMuerto.Invoke();
            GameController.main.cobrar(reward);
            dead=true;
            Destroy(gameObject);
        }
    }
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
