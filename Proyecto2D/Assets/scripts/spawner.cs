using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class spawner : MonoBehaviour
{
    public static spawner main;
    // Configuraciones de los enemigos (asignado desde el inspector)
    [Header("Enemy Settings")]
    [SerializeField] private GameObject[] enemigos;  // Array de enemigos que se pueden generar

    // Configuraciones de generación de enemigos
    [Header("Spawn Settings")]
    [SerializeField] private int enemigosBase = 6;  // Número base de enemigos en la primera oleada
    [SerializeField] private float enemigosPorSegundo = 0.5f;  // Frecuencia de spawn, cuántos enemigos por segundo
    [SerializeField] public float tiempoEntreOleadas = 5f;  // Tiempo que pasa entre oleadas
    [SerializeField] private float factorDificultad = 0.75f;  // Factor que incrementa la cantidad de enemigos por oleada según el nivel de dificultad

    public int oleada = 1;  // Contador de oleadas
    private float tiempoUltimoSpawn;  // Temporizador para controlar el tiempo entre la aparición de enemigos
    private int enemigosVivos;  // Número de enemigos actualmente vivos
    private int enemigosEnSpawn;  // Número de enemigos que faltan por generar en la oleada
    public bool enSpawn = false;  // Estado que indica si se están generando enemigos en la oleada actual
    [SerializeField] private RawImage barImg;
    float currentTime;

    private int[] pesos;
    private GameObject enemigoACrear;
    [SerializeField] TextMeshProUGUI oleada_num;



    public static UnityEvent OnEnemigoMuerto = new UnityEvent();  // Evento que se dispara cuando un enemigo muere

    // Método que se ejecuta cuando el script se inicializa
    private void Awake()
    {
        main = this;
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
        if(GameController.main.win) return;
        if(Base.main.game_over) return;
        if (GameController.main.paused) return;
        if (oleada >= 5){
            oleada_num.text= "Final!" ;
        }else{
            oleada_num.text= oleada.ToString() ;
        }
        // Si no estamos en una oleada
        if (!enSpawn) {
            EnableTimeOutBar(true);
            currentTime -= Time.deltaTime;
            SetTimeOutValue(currentTime / tiempoEntreOleadas);
            return;
        }

        // Incrementamos el tiempo desde el último spawn
        tiempoUltimoSpawn += Time.deltaTime;

        // Si ha pasado el tiempo suficiente para generar un nuevo enemigo
        if (oleada < 5){
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
        } else{
            if (tiempoUltimoSpawn >= (1f / enemigosPorSegundo) && enemigosEnSpawn > 0)
            {
                if (enemigosVivos < 1){
                // Creamos un enemigo
                CrearEnemigo();

                // Disminuimos la cantidad de enemigos por generar
                enemigosEnSpawn--;

                // Incrementamos el número de enemigos vivos
                enemigosVivos++;

                // Reseteamos el temporizador de spawn
                tiempoUltimoSpawn = 0f;
                }
            }
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
        EnableTimeOutBar(false);
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
        //int index = Random.Range(0, enemigos.Length);

        switch (oleada)
        {
            case 1:
                pesos = new int[] { 100, 0, 0, 0 };
                break;
            case 2:
                pesos = new int[] { 70, 0, 30, 0 };
                break;
            case 3:
                pesos = new int[] { 40, 30, 30, 0 };
                break;
            case 4:
                pesos = new int[] { 30, 35, 35, 0};
                break;
            case 5:
                pesos = new int[] { 0, 0, 0, 100};
                break;
            default:
                break;
        }

        int numeroAleatorio = Random.Range(0, 100);
        // Seleccionar el elemento basado en el número aleatorio y los pesos
        int acumulado = 0;
        for (int i = 0; i < enemigos.Length; i++)
        {
            acumulado += pesos[i];
            if (numeroAleatorio < acumulado){
                enemigoACrear = enemigos[i];
                break;
            }
        }
        Instantiate(enemigoACrear, GameController.main.inicio.position, Quaternion.identity);

        // Muestra un mensaje en la consola de Unity indicando que se ha creado un enemigo
        Debug.Log("Creando enemigo");

        // Crea el enemigo en la posición inicial definida por GameController
        
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
        ResetTimeOut();
        EnableTimeOutBar(true);

        // Inicia la siguiente oleada
        StartCoroutine(IniciarOleada());
    }
    public void SetTimeOutValue(float timeout)
    {
        barImg.transform.localScale = new Vector3(timeout, 1.0f);
    }
    public void EnableTimeOutBar(bool enable)
    {
        barImg.gameObject.SetActive(enable);
    }
    void ResetTimeOut() {
        currentTime = tiempoEntreOleadas;
    }
}
