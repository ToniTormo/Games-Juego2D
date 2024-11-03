using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Torreta : MonoBehaviour
{

    [SerializeField] private Transform giro; //Probablemente no se use
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float rango= 5f;

    
    private Transform target;
    
    private void OnDrawGizmosSelected()
    {
        Handles.color= Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, rango);
    }
    private void Findtarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position,0f, enemyMask);
        if (hits.Length > 0){
            target= hits[0].transform;
        }
    }

    private bool CheckRango(){
        return Vector2.Distance(target.position, transform.position) <= rango;

    }
    
    private float Angulo(){
        return Mathf.Atan2(target.position.y - transform.position.y, target.position.x-transform.position.x)*Mathf.Rad2Deg -90f;
    }
    private void Rotacion(){
        Quaternion rotar= Quaternion.Euler(new Vector3(0f,0f,Angulo()));
        giro.rotation=rotar;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (target==null){
        Findtarget();
        return;
       }
        Debug.Log(Angulo());
        Rotacion();
        if (!CheckRango()){
            target=null;
        }

    }
}
