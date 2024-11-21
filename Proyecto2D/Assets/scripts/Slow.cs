using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;


public class Slow : MonoBehaviour
{
    // Variables públicas para ser asignadas en el inspector de Unity
    [SerializeField] private LayerMask enemyMask;  // Máscara de capa para identificar los enemigos
    [SerializeField] private float rango = 5f;  // Rango de acción para detectar enemigos
    [SerializeField] private float veldis = 4f;  // Velocidad de disparo, o frecuencia con la que se activa la congelación
    [SerializeField] private float freezeTime = 1f;  // Tiempo durante el cual se congela la velocidad del enemigo
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

    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer
    private Color originalColor; // Almacena el color original del sprite


    // Método para congelar enemigos dentro del rango
    private void Congelar()
    {
        // Cambia el color del sprite a azul
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.cyan;
        }
        // Realiza un CircleCast para detectar los enemigos dentro del rango
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position, 0f, enemyMask);

        // Si se detectaron enemigos
        if (hits.Length > 0)
        {
            // Recorre todos los enemigos detectados
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];  // Toma el enemigo actual

                // Obtiene el componente Move del enemigo para modificar su velocidad
                Move em = hit.transform.GetComponent<Move>();
                if (em != null)  // Verifica que el enemigo tenga un componente Move
                {
                    // Cambia la velocidad del enemigo a la mitad
                    em.Cambio_speed(0.5f);

                    // Inicia la coroutine para restablecer la velocidad del enemigo después de un tiempo
                    StartCoroutine(RestablecerVelocidad(em));
                }
            }
        }
        // Restaura el color después del tiempo de congelación
        StartCoroutine(RestaurarColor());
    }

    // Coroutine que restablece la velocidad del enemigo después de un cierto tiempo
    private IEnumerator RestablecerVelocidad(Move em)
    {
        // Espera el tiempo especificado por freezeTime
        yield return new WaitForSeconds(freezeTime);

        // Restaura la velocidad del enemigo al valor original
        em.Reset_speed();
    }

    // El método Start se ejecuta al inicio, no está siendo utilizado en este caso
    void Start()
    {
        rangobase = rango;
        escala = tamaño_area/rangobase;
        veldisbase = veldis;
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
        if (GameController.main.paused) return;
        AjustarRangoVisual();
        // Incrementa el temporizador de disparo con el tiempo que ha pasado desde el último frame
        tiempo_disparo += Time.deltaTime;

        // Si ha pasado suficiente tiempo (en función de veldis), ejecuta la función Congelar
        if (tiempo_disparo >= 1f / veldis)
        {
            Congelar();
            // Reinicia el temporizador para esperar el siguiente disparo
            tiempo_disparo = 0f;
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
    private IEnumerator RestaurarColor()
    {
        yield return new WaitForSeconds(freezeTime);

        // Restaura el color original del sprite
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}
