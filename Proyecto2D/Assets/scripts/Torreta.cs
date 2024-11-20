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

    [SerializeField] private Transform rangoVisual; // Objeto hijo que representa el rango visual.
    [SerializeField] private GameObject rangoVisual_obj; 
    [SerializeField] private float tamaño_area=34; 




    // Variables de nivel y propiedades base.
    private int nivel = 1;
    private float rangobase;
    private float veldisbase;
    private int costomejorabase;
    private float escala;

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
        escala = tamaño_area/rangobase;
        veldisbase = veldis;
        costomejorabase = costomejora;
        rangoVisual_obj.SetActive(false);

        upgradeButton.onClick.AddListener(Mejorar);
         AjustarRangoVisual();

    }

    // Actualización por cuadro para controlar el comportamiento de la torreta.
    void Update()
    {
        if(Base.main.game_over) return;
       //AjustarRangoVisual();
        if (GameController.main.paused) return;
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
        rangoVisual_obj.SetActive(true);
        upgradeUI.SetActive(true);

    }

    // Cierra la UI de mejora de la torreta.
    public void CloseUpgrade(){
        rangoVisual_obj.SetActive(false);
        upgradeUI.SetActive(false);
        UIManager.main.SetHovering(false);
    }

    // Método para mejorar la torreta, aumentando sus estadísticas si el jugador tiene suficiente capital.
    public void Mejorar(){
        if (costomejora > GameController.main.capital){ 
            return;
        } else {
            GameController.main.gastar(costomejora);
            nivel++;
            costomejora = calcular_costo();
            veldis = calcular_velocidad();
            rango = calcular_rango();
            AjustarRangoVisual();
            

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

    private void AjustarRangoVisual()
    {
        
        // Calcula la escala en función del rango y del tamaño inicial del sprite
        rangoVisual.localScale = new Vector3(escala*rango, escala*rango, 1f);
        
        
    }
}
