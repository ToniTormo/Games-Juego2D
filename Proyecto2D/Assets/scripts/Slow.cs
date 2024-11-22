using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;


public class Slow : MonoBehaviour
{
    // Variables públicas para ser asignadas en el inspector de Unity
    [SerializeField] private LayerMask enemyMask;  // Máscara de capa para identificar los enemigos
    [SerializeField] private float rango = 5f;  // Rango de acción para detectar enemigos
    [SerializeField] private float veldis = 2f;  // Velocidad de disparo, o frecuencia con la que se activa la congelación
    [SerializeField] private float freezeTime = 3f;  // Tiempo durante el cual se congela la velocidad del enemigo
    private float tiempo_disparo;  // Temporizador para controlar la frecuencia de disparo
    private float freezeTime_contador;
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
    private float freezetimebase;
    private int costomejorabase; 
    private float escala;

    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer
    private Color originalColor; // Almacena el color original del sprite

    [SerializeField] TextMeshProUGUI costo_mejora_txt;

    private RaycastHit2D[] hits;
    private RaycastHit2D[] congelados;
    private Dictionary<Move, bool> afectados = new Dictionary<Move, bool>();


    

    // Método para congelar enemigos dentro del rango
    private void Congelar()
    {
        congelados = hits;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                Move em = hit.transform.GetComponent<Move>();
                if (em != null && !afectados.ContainsKey(em)) // Si no ha sido afectado por esta torreta
                {
                    em.Congelar(); // Aplica ralentización
                    afectados[em] = true; // Marca al enemigo como afectado por esta torreta
                }
            }
        }
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.cyan;
        }
    }


    private void RestablecerVelocidad()
    {
        foreach (var em in afectados.Keys)
        {
            if (em != null) // Asegura que el enemigo no haya sido destruido
            {
                em.Descongelar(); // Elimina ralentización
            }
        }
        afectados.Clear(); // Limpia el registro de enemigos afectados por esta torreta
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }


    // El método Start se ejecuta al inicio, no está siendo utilizado en este caso
    void Start()
    {
        rangobase = rango;
        escala = tamaño_area/rangobase;
        freezetimebase= freezeTime;
        costomejorabase = costomejora;
        rangoVisual_obj.SetActive(false);

        upgradeButton.onClick.AddListener(Mejorar);
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Guarda el color original
        }
    }

    // El método Update se ejecuta una vez por frame
    void Update()
    {
        if(Base.main.game_over) return;
        if (GameController.main.win) return;
        if (GameController.main.paused) return;
        if(nivel >= 4){
            costo_mejora_txt.text= "Nvl Max";   
        }else{
        costo_mejora_txt.text= costomejora.ToString();   
        }    
        escala = tamaño_area/rangobase;
        AjustarRangoVisual();
        // Incrementa el temporizador de disparo con el tiempo que ha pasado desde el último frame
        
        try{
        hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position, 0f, enemyMask);
        }catch{}

        StartCoroutine(Effect());

        
        // if(freezeTime_contador < freezeTime){
        //         tiempo_disparo = 0f;
        //         freezeTime_contador += Time.deltaTime;
        //     }else{
        //         if (spriteRenderer != null)
        //         {
        //         spriteRenderer.color = originalColor;
        //         }
        //         RestablecerVelocidad();
                
        //     }
    }

    private IEnumerator Effect(){
        tiempo_disparo += Time.deltaTime;        
        if (tiempo_disparo >= 1f / veldis)
        {
        // Si ha pasado suficiente tiempo (en función de veldis), ejecuta la función Congelar
            
            Congelar();
        //         // Reinicia el temporizador para esperar el siguiente disparo

            yield return new WaitForSeconds(freezeTime);

            RestablecerVelocidad();
            tiempo_disparo = 0f;

        //         freezeTime_contador=0f;
            
            
        }

    }

    // Método para dibujar una visualización del rango de acción en el editor de Unity (cuando el objeto está seleccionado)
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
            freezeTime = calcular_velocidad();
            rango = calcular_rango();
            AjustarRangoVisual();
            CloseUpgrade();
        }
    }

    // Calcula la nueva velocidad de disparo en función del nivel.
    private float calcular_velocidad(){
        return freezetimebase * Mathf.Pow(nivel, 0.4f);
    }

    // Calcula el nuevo rango en función del nivel.
    private float calcular_rango(){
        return rangobase * Mathf.Pow(nivel, 0.3f);
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
