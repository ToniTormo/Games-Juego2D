using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

// Clase Torreta, que controla el comportamiento de una torreta en el juego.
public class Torreta : MonoBehaviour
{
    // Transform que rota la torreta para apuntar hacia los enemigos.
    [SerializeField] private Transform giro;

    // Capa que representa a los enemigos, para detectar sólo los objetos correspondientes.
    [SerializeField] private LayerMask enemyMask;

    // Rango de alcance de la torreta.
    [SerializeField] private float rango = 5f;

    // Prefab de la bala que dispara la torreta.
    [SerializeField] private GameObject bulletPrefab;

    // Transform que representa el punto de origen de los disparos.
    [SerializeField] private Transform canon;

    // Velocidad de disparo (balas por segundo).
    [SerializeField] private float veldis = 1f;

    // Costo de mejora de la torreta.
    [SerializeField] private int costomejora = 100;

    // UI para la mejora de la torreta.
    [SerializeField] private GameObject upgradeUI;

    // Botón de mejora.
    [SerializeField] private Button upgradeButton;

    // Variables de nivel y propiedades base.
    private int nivel = 1;
    private float rangobase;
    private float veldisbase;
    private int costomejorabase;

    // Objetivo actual de la torreta.
    private Transform target;

    // Temporizador para controlar el tiempo entre disparos.
    private float tiempo_disparo;

    // Método para dibujar el alcance de la torreta en el editor.
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, rango);
    }

    // Encuentra el objetivo más cercano en el rango de la torreta.
    private void Findtarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position, 0f, enemyMask);
        if (hits.Length > 0){
            target = hits[0].transform;
        }
    }

    // Verifica si el objetivo está dentro del rango de la torreta.
    private bool CheckRango(){
        return Vector2.Distance(target.position, transform.position) <= rango;
    }

    // Calcula el ángulo de rotación necesario para apuntar al objetivo.
    private float Angulo(){
        return Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
    }

    // Rota la torreta para apuntar al objetivo.
    private void Rotacion(){
        Quaternion rotar = Quaternion.Euler(new Vector3(0f, 0f, Angulo()));
        giro.rotation = rotar;
    }

    // Inicialización al comenzar el juego, guarda las propiedades base y configura el botón de mejora.
    void Start()
    {
        rangobase = rango;
        veldisbase = veldis;
        costomejorabase = costomejora;

        upgradeButton.onClick.AddListener(Mejorar);
    }

    // Actualización por cuadro para controlar el comportamiento de la torreta.
    void Update()
    {
       if (target == null){
            Findtarget();
            return;
       }

       Rotacion();

       if (!CheckRango()){
            target = null;
       } else {
            tiempo_disparo += Time.deltaTime;
            if(tiempo_disparo >= 1f / veldis){
                Disparar();
                tiempo_disparo = 0f;
            }
       }
    }

    // Método para disparar balas hacia el objetivo.
    private void Disparar(){
        GameObject balaobj = Instantiate(bulletPrefab, canon.position, Quaternion.identity);
        Bala balascript = balaobj.GetComponent<Bala>();
        balascript.fijar_objetivo(target);
    }

    // Abre la UI de mejora de la torreta.
    public void OpenUpgrade(){
        upgradeUI.SetActive(true);
    }

    // Cierra la UI de mejora de la torreta.
    public void CloseUpgrade(){
        upgradeUI.SetActive(false);
        UIManager.main.SetHovering(false);
    }

    // Método para mejorar la torreta, aumentando sus estadísticas si el jugador tiene suficiente capital.
    public void Mejorar(){
        if (costomejora > GameController.main.capital){ 
            return;
        } else {
            costomejora = calcular_costo();
            GameController.main.gastar(costomejora);

            nivel++;

            veldis = calcular_velocidad();
            rango = calcular_rango();

            CloseUpgrade();
        }
    }

    // Calcula la nueva velocidad de disparo en función del nivel.
    private float calcular_velocidad(){
        return veldisbase * Mathf.Pow(nivel, 0.5f);
    }

    // Calcula el nuevo rango en función del nivel.
    private float calcular_rango(){
        return rangobase * Mathf.Pow(nivel, 0.4f);
    }

    // Calcula el costo de la mejora en función del nivel.
    private int calcular_costo(){
        return Mathf.RoundToInt(costomejorabase * Mathf.Pow(nivel, 0.8f));
    }
}
