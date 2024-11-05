using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Torreta : MonoBehaviour
{

    [SerializeField] private Transform giro; 
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float rango= 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform canon;
    [SerializeField] private float veldis= 1f; //balas por segundo

    
    private Transform target;
    private float tiempo_disparo;
    
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
        //Debug.Log(Angulo());
        Rotacion();
        if (!CheckRango()){
            target=null;
        } else{
            tiempo_disparo+= Time.deltaTime;
            if(tiempo_disparo >= 1f/veldis){
                Disparar();
                tiempo_disparo=0f;
            }
        }

    }
    private void Disparar(){
        //Debug.Log("Fuego");
        GameObject balaobj=Instantiate(bulletPrefab,canon.position,Quaternion.identity);
        Bala balascript= balaobj.GetComponent<Bala>();
        balascript.fijar_objetivo(target);
        
    }
}
