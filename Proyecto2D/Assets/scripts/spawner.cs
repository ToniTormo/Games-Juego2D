using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class spawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemigos;

    [Header("Spawn Settings")]
    [SerializeField] private int enemigosBase = 6;
    [SerializeField] private float enemigosPorSegundo = 0.5f;
    [SerializeField] private float tiempoEntreOleadas = 5f;
    [SerializeField] private float factorDificultad = 0.75f;

    private int oleada = 1;
    private float tiempoUltimoSpawn;
    private int enemigosVivos;
    private int enemigosEnSpawn;
    private bool enSpawn = false;

    public static UnityEvent OnEnemigoMuerto = new UnityEvent();

    private void Awake()
    {
        OnEnemigoMuerto.AddListener(EnemigoMuerto);
    }

    private void Start()
    {
        StartCoroutine(IniciarOleada());
    }

    private void Update()
    {
        if (!enSpawn) return;

        tiempoUltimoSpawn += Time.deltaTime;
        if (tiempoUltimoSpawn >= (1f / enemigosPorSegundo) && enemigosEnSpawn > 0)
        {
            CrearEnemigo();
            enemigosEnSpawn--;
            enemigosVivos++;
            tiempoUltimoSpawn = 0f;
        }
        
        if (enemigosVivos == 0 && enemigosEnSpawn == 0)
        {
            FinOleada();
        }
    }

    private void EnemigoMuerto()
    {
        enemigosVivos--;
    }

    private IEnumerator IniciarOleada()
    {
        yield return new WaitForSeconds(tiempoEntreOleadas);
        enSpawn = true;
        enemigosEnSpawn = CalcularEnemigosPorOleada();
    }

    private int CalcularEnemigosPorOleada()
    {
        return Mathf.RoundToInt(enemigosBase * Mathf.Pow(oleada, factorDificultad));
    }

    private void CrearEnemigo()
    {
        Debug.Log("Creando enemigo");
        GameObject enemigoACrear = enemigos[0];
        Instantiate(enemigoACrear, GameController.main.inicio.position, Quaternion.identity);
        
    }

    private void FinOleada()
    {
        enSpawn = false;
        tiempoUltimoSpawn = 0f;
        oleada++;
        StartCoroutine(IniciarOleada());
    }
}
