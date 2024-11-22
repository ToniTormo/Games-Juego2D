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
            speed= baseSpeed/2;
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

    // public void Cambio_speed(float reduction = 0.8f)
    // {
    //     slowCounter++;
    //     adjust_speed(reduction);
    // }

    // public void Reset_speed(float reduction = 0.8f)
    // {
    //     slowCounter--;
    //     if (slowCounter < 0) slowCounter = 0; // Asegúrate de que nunca sea negativo.
    //     adjust_speed(reduction);
    // }

    // private void adjust_speed(float reduction)
    // {
    //     if (slowCounter != 0)
    //     {
    //         speed = baseSpeed - (reduction * slowCounter);
    //         if (speed < 0.3f) speed = 0.3f; // Velocidad mínima.
    //     }
    //     else
    //     {
    //         speed = baseSpeed; // Restaura la velocidad original.
    //     }
    // }
}
