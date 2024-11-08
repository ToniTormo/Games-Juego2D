using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// Clase serializable Defensa que representa una unidad defensiva (como una torreta).
[Serializable]
public class Defensa
{
    // Nombre de la defensa.
    public string name;

    // Coste de la defensa.
    public int coste;

    // Prefab del objeto que representa visualmente la defensa en el juego.
    public GameObject prefab;

    // Constructor de la clase Defensa, para inicializar sus propiedades.
    public Defensa(string _name, int _coste, GameObject _prefab){
        name = _name;
        coste = _coste;
        prefab = _prefab;
    }
}
