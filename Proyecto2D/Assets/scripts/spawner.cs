using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class spawner : MonoBehaviour
{
    // Configuraciones de los enemigos (asignado desde el inspector)
    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemigos;  // Array de enemigos que se pueden generar

    // Configuraciones de generación de enemigos
    [Header("Spawn Settings")]
    [SerializeField] private int enemigosBase = 6;  // Número base de enemigos en la primera oleada
    [SerializeField] private float enemigosPorSegundo = 0.5f;  // Frecuencia de spawn, cuántos enemigos por segundo
    [SerializeField] private float tiempoEntreOleadas = 5f;  // Tiempo que pasa entre oleadas
    [SerializeField] private float factorDificultad = 0.75f;  // Factor que incrementa la cantidad de enemigos por oleada según el nivel de dificultad

    private int oleada = 1;  // Contador de oleadas
    private float tiempoUltimoSpawn;  // Temporizador para controlar el tiempo entre la aparición de enemigos
    private int enemigosVivos;  // Número de enemigos actualmente vivos
    private int enemigosEnSpawn;  // Número de enemigos que faltan por generar en la oleada
    private bool enSpawn = false;  // Estado que indica si se están generando enemigos en la oleada actual

    public static UnityEvent OnEnemigoMuerto = new UnityEvent();  // Evento que se dispara cuando un enemigo muere

    // Método que se ejecuta cuando el script se inicializa
    private void Awake()
    {
        // Suscribe el método 'EnemigoMuerto' al evento OnEnemigoMuerto
        OnEnemigoMuerto.AddListener(EnemigoMuerto);
    }

    // Método que se ejecuta al inicio del juego
    private void Start()
    {
        // Inicia la primera oleada con la corutina
        StartCoroutine(IniciarOleada());
    }

    // Método que se ejecuta cada frame
    private void Update()
    {
        // Si no estamos en una oleada, no hacemos nada
        if (!enSpawn) return;

        // Incrementamos el tiempo desde el último spawn
        tiempoUltimoSpawn += Time.deltaTime;

        // Si ha pasado el tiempo suficiente para generar un nuevo enemigo
        if (tiempoUltimoSpawn >= (1f / enemigosPorSegundo) && enemigosEnSpawn > 0)
        {
            // Creamos un enemigo
            CrearEnemigo();

            // Disminuimos la cantidad de enemigos por generar
            enemigosEnSpawn--;

            // Incrementamos el número de enemigos vivos
            enemigosVivos++;

            // Reseteamos el temporizador de spawn
            tiempoUltimoSpawn = 0f;
        }

        // Si ya no quedan enemigos vivos ni enemigos por generar, finaliza la oleada
        if (enemigosVivos == 0 && enemigosEnSpawn == 0)
        {
            FinOleada();
        }
    }

    // Método que se llama cuando un enemigo muere
    private void EnemigoMuerto()
    {
        // Decrementa el número de enemigos vivos
        enemigosVivos--;
    }

    // Corutina que se encarga de iniciar una nueva oleada después de un tiempo
    private IEnumerator IniciarOleada()
    {
        // Espera el tiempo de la oleada antes de iniciar
        yield return new WaitForSeconds(tiempoEntreOleadas);

        // Comienza a generar enemigos
        enSpawn = true;

        // Calcula el número de enemigos para esta oleada
        enemigosEnSpawn = CalcularEnemigosPorOleada();
    }

    // Método que calcula cuántos enemigos generaremos en la oleada actual
    private int CalcularEnemigosPorOleada()
    {
        // Calcula el número de enemigos basado en la dificultad y oleada
        return Mathf.RoundToInt(enemigosBase * Mathf.Pow(oleada, factorDificultad));
    }

    // Método que se encarga de crear un enemigo y posicionarlo en el mundo
    private void CrearEnemigo()
    {
        // Escoge un enemigo aleatorio del array de enemigos
        int index = Random.Range(0, enemigos.Length);

        // Muestra un mensaje en la consola de Unity indicando que se ha creado un enemigo
        Debug.Log("Creando enemigo");

        // Crea el enemigo en la posición inicial definida por GameController
        GameObject enemigoACrear = enemigos[index];
        Instantiate(enemigoACrear, GameController.main.inicio.position, Quaternion.identity);
    }

    // Método que se ejecuta al final de cada oleada
    private void FinOleada()
    {
        // Detiene el spawn de enemigos
        enSpawn = false;

        // Resetea el temporizador de spawn
        tiempoUltimoSpawn = 0f;

        // Incrementa el contador de oleada
        oleada++;

        // Inicia la siguiente oleada
        StartCoroutine(IniciarOleada());
    }
}
