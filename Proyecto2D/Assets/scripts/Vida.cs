using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vida : MonoBehaviour
{
    [SerializeField] private int puntos=2;

    public void recibir_daño(int dmg){
        puntos-= dmg;
        if (puntos <=0){
            spawner.OnEnemigoMuerto.Invoke();
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
