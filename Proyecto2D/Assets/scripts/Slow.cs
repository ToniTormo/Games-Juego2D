using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Slow : MonoBehaviour
{
 
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float rango= 5f;
    [SerializeField] private float veldis= 4f;
    [SerializeField] private float freezeTime= 1f;
    private float tiempo_disparo;

    private void Congelar(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position,0f, enemyMask);

        if (hits.Length > 0){
            for (int i=0; i < hits.Length; i++){
                RaycastHit2D hit= hits[i];

                Move em= hit.transform.GetComponent<Move>();
                em.Cambio_speed(0.5f);

                StartCoroutine(RestablecerVelocidad(em));
            }
        }

    }

    private IEnumerator RestablecerVelocidad(Move em){
        yield return new WaitForSeconds(freezeTime);
        em.Reset_speed();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tiempo_disparo+= Time.deltaTime;
        if(tiempo_disparo >= 1f/veldis){
            Congelar();
            tiempo_disparo=0f;
        }
         
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color= Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, rango);
    }
}
