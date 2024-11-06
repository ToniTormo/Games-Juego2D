using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public static GameController main;
  public Transform[] path;
  public Transform inicio; 

  public int capital;

  private void Awake(){
    main=this;
  }
  
  void Start()
  {
    capital=100;      
  }
  public void cobrar(int cantidad){
    capital+= cantidad;
  }
  public bool gastar(int cantidad){
    if(cantidad <= capital){
      //Comprar
      capital -= cantidad;
      return true;
    }else{
      return false;
    }
  }
}
