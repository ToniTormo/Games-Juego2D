using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private Rigidbody2D rb;
    private Transform target;
    private int pathIndex = 0;

    private float baseSpeed;

    private void Start()
    {
        // Set the initial target as the first point in the path
        baseSpeed=speed;
        target = GameController.main.path[pathIndex];
    }

    private void Update()
    {
        // Check if the object is close to the target point
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            // If the path is complete, invoke the enemy death event and destroy the object
            if (pathIndex >= GameController.main.path.Length)
            {
                spawner.OnEnemigoMuerto.Invoke();
                Destroy(gameObject);
                return;
            }
            else
            {
                // Move to the next target point
                target = GameController.main.path[pathIndex];
            }
        }
    }

    private void FixedUpdate()
    {
        // Calculate the movement direction and apply it to the rigidbody
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }
    public void Cambio_speed(float _speed){
        speed -= _speed;
    }
    public void Reset_speed(){
        speed=baseSpeed;
    }
}
