using System.Collections;
using UnityEngine;
using System.Collections.Generic; 


public class Move : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Rigidbody2D rb;

    private Transform target;
    private int pathIndex = 0;
    private float baseSpeed;
    private float slow=3;

    private int slowCounter = 0; // Número de torretas ralentizando este enemigo.

    [SerializeField] private int ataque = 5;

    private void Start()
    {
        baseSpeed = speed;
        target = GameController.main.path[pathIndex];
    }

    private void Update()
    {
        if (Base.main.game_over) return;
        if (GameController.main.paused) return;
        if(slowCounter > 0){
            speed= baseSpeed/slow;
        }else{
            speed=baseSpeed;
        }
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            if (pathIndex >= GameController.main.path.Length)
            {
                spawner.OnEnemigoMuerto.Invoke();
                Base.main.dmgbase(ataque);
                Destroy(gameObject);
                return;
            }
            else
            {
                target = GameController.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    public void Congelar(){
        slowCounter++;
    }

    public void Descongelar(){
        slowCounter--;
        if (slowCounter < 0){
            slowCounter=0;
        }
    }

    
}
