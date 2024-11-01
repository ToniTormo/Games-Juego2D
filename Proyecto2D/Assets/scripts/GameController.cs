using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
  public static GameController main;
  public Transform[] path;
  public Transform inicio; 

  private void Awake(){
    main=this;
  }
}
