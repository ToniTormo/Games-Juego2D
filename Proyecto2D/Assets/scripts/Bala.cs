using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bala : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float velocidad = 5f;
    [SerializeField] private int dmg = 1;

    private Transform target;

    public void fijar_objetivo(Transform objetivo){
        target=objetivo;
    }
    
    void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction= (target.position-transform.position).normalized;
        rb.velocity=direction*velocidad;
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Vida>().recibir_daño(dmg);
        Destroy(gameObject);
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
