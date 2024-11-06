using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Defensa
{
    public string name;
    public int coste;
    public GameObject prefab;

    public Defensa (string _name, int _coste, GameObject _prefab){
        name=_name;
        coste=_coste;
        prefab=_prefab;
    }
}
