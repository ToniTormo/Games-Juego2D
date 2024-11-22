using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;


public class Smash : MonoBehaviour
{

    // Variables públicas para ser asignadas en el inspector de Unity
    [SerializeField] private LayerMask enemyMask;  // Máscara de capa para identificar los enemigos
    [SerializeField] private float rango = 2.2f;  // Rango de acción para detectar enemigos
    [SerializeField] private float veldis = 4f; // Velocidad de disparo, o frecuencia con la que se activa la congelación
    [SerializeField] private int dmg = 1000;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite new_sr;
    private Sprite sr_copia;

    private float tiempo_disparo;  // Temporizador para controlar la frecuencia de disparo

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
    [SerializeField] TextMeshProUGUI costo_mejora_txt;



    private void Aplastar(){
        // Realiza un CircleCast para detectar los enemigos dentro del rango
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position, 0f, enemyMask);

        // Si se detectaron enemigos
        if (hits.Length > 0)
        {
            // Recorre todos los enemigos detectados
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];  // Toma el enemigo actual

                hit.transform.GetComponent<Vida>().recibir_daño(dmg);
                
            }

            sr.sprite=new_sr;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        sr_copia=sr.sprite;
        rangobase = rango;
        escala = tamaño_area/rangobase;
        veldisbase = veldis;
        costomejorabase = costomejora;
        rangoVisual_obj.SetActive(false);

        upgradeButton.onClick.AddListener(Mejorar);
    }

    // Update is called once per frame
    void Update()
    {
        if(Base.main.game_over) return;
        if (GameController.main.paused) return;
        if(nivel >= 4){
            costo_mejora_txt.text= "Nvl Max";   
        }else{
        costo_mejora_txt.text= costomejora.ToString();   
        }
        escala = tamaño_area/rangobase;    
        AjustarRangoVisual();
       // Incrementa el temporizador de disparo con el tiempo que ha pasado desde el último frame
        tiempo_disparo += Time.deltaTime;

        if (tiempo_disparo >= (1f/veldis)/2f){
            sr.sprite=sr_copia;
        }

        // Si ha pasado suficiente tiempo (en función de veldis), ejecuta la función Congelar
        if (tiempo_disparo >= 1f / veldis)
        {
            Aplastar();
            // Reinicia el temporizador para esperar el siguiente disparo
            tiempo_disparo = 0f;
        } 
    }
    private void OnDrawGizmosSelected()
    {
        // Establece el color de los Gizmos a cian
        Handles.color = Color.cyan;

        // Dibuja un círculo que representa el rango de acción del Slow
        Handles.DrawWireDisc(transform.position, transform.forward, rango);
    }
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
        if (nivel>=4) {
        return;
        }
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
        return veldisbase * Mathf.Pow(nivel, 0.3f);
    }

    // Calcula el nuevo rango en función del nivel.
    private float calcular_rango(){
        return rangobase * Mathf.Pow(nivel, 0.2f);
    }

    // Calcula el costo de la mejora en función del nivel.
    private int calcular_costo(){
        return Mathf.RoundToInt(costomejorabase * Mathf.Pow(nivel, 0.8f));
    }

    private void AjustarRangoVisual()
    {
        
        rangoVisual.localScale = new Vector3(escala*rango, escala*rango, 1f);
        
    }
}
