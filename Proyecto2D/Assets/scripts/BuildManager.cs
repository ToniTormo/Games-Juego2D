using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    
    //[SerializeField] private GameObject[] torretaspf;
    [SerializeField] private Defensa[] torretas;
    private int torreta_actual=0;
    
    private void Awake(){
        main=this;
    }

    public Defensa GetTorreta(){
        return torretas[torreta_actual];
    }

    public void Establecer(int _torreta){
        torreta_actual=_torreta;
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
