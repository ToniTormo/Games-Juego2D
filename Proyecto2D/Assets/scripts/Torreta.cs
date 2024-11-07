using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class Torreta : MonoBehaviour
{

    [SerializeField] private Transform giro; 
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float rango= 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform canon;
    [SerializeField] private float veldis= 1f; //balas por segundo
    [SerializeField] private int costomejora= 100;

    [SerializeField] private GameObject upgradeUI;
    [SerializeField] private Button upgradeButton;
    private int nivel=1;
    private float rangobase;
    private float veldisbase;
    private int costomejorabase;



    
    private Transform target;
    private float tiempo_disparo;
    
    private void OnDrawGizmosSelected()
    {
        Handles.color= Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, rango);
    }
    private void Findtarget(){
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, rango, (Vector2)transform.position,0f, enemyMask);
        if (hits.Length > 0){
            target= hits[0].transform;
        }
    }

    private bool CheckRango(){
        return Vector2.Distance(target.position, transform.position) <= rango;

    }
    
    private float Angulo(){
        return Mathf.Atan2(target.position.y - transform.position.y, target.position.x-transform.position.x)*Mathf.Rad2Deg -90f;
    }
    private void Rotacion(){
        Quaternion rotar= Quaternion.Euler(new Vector3(0f,0f,Angulo()));
        giro.rotation=rotar;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rangobase=rango;
        veldisbase=veldis;
        costomejorabase=costomejora;

        upgradeButton.onClick.AddListener(Mejorar);
    }

    // Update is called once per frame
    void Update()
    {
       if (target==null){
        Findtarget();
        return;
       }
        //Debug.Log(Angulo());
        Rotacion();
        if (!CheckRango()){
            target=null;
        } else{
            tiempo_disparo+= Time.deltaTime;
            if(tiempo_disparo >= 1f/veldis){
                Disparar();
                tiempo_disparo=0f;
            }
        }

    }
    private void Disparar(){
        //Debug.Log("Fuego");
        GameObject balaobj=Instantiate(bulletPrefab,canon.position,Quaternion.identity);
        Bala balascript= balaobj.GetComponent<Bala>();
        balascript.fijar_objetivo(target);
        
    }
    public void OpenUpgrade(){
        upgradeUI.SetActive(true);
    }
    public void CloseUpgrade(){
        upgradeUI.SetActive(false);
        UIManager.main.SetHovering(false);
    }

    public void Mejorar(){
        if (costomejora > GameController.main.capital){ 
        return;} else{

        costomejora=calcular_costo();
        GameController.main.gastar(costomejora);

        nivel++;

        veldis=calcular_velocidad();
        rango=calcular_rango();

        CloseUpgrade();
        }
    }

    private float calcular_velocidad(){
        return veldisbase*Mathf.Pow(nivel,0.5f);
    }

    private float calcular_rango(){
        return rangobase*Mathf.Pow(nivel,0.4f);
    }

    private int calcular_costo(){
        return Mathf.RoundToInt(costomejorabase*Mathf.Pow(nivel,0.8f));
    }
}
