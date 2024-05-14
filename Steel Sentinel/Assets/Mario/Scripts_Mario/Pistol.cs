using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Pistol : MonoBehaviour
{
    #region General Variables
    [Header("General References")]
    [SerializeField] Camera fpsCam; //Referenca a la camara desde cuyo centro se dispara (Raycast desde centro camara)
    [SerializeField] public Transform shootPoint;//Referencia a la posicion del objeto desde donde se dispara (Raycast desde posicion concreta)
    [SerializeField] RaycastHit hit;//Referebcia a la info de impacto de os disparos
    [SerializeField] LayerMask enemyLayer;//Referencia a la Layer que puede impactar el disparo
    [SerializeField] AudioSource weaponSound;//Referencia al audioSource
    public GameObject bala;
    public float shotForce = 1500;
    public float shotRate = 0.5f;
    private float shotRateTime = 0;

    [Header("Wapon Stats")]
    public int damage;//Daño base del arma por bala
    public float range;//Alcance de disparo
    public float spread;//Dispersion de los didparos
    public float shootingCooldown;//Tiempo enfriamiento del arma
    public float timeBetweenShoots;//Tiempo real entre disparo y disparo 
    public float reloadTime;//Tiempo que tardas en recargar,suele igualarse a la duracion de la animacion de recarga
    public bool allowButtonHold;//Permite disparar por pulsacion (false) o mantenimiento (true)

    [Header("Buller Managament")]
    public int ammoSize;//Numero de balas por cargador
    public int bulletsPerTap;//Numero de balas que s disparan por cada disparo unico
    [SerializeField] int bulletsLeft;//Numero de balas en el cargador actuar
    [SerializeField] int bulletsShot;//Numero de balas YA DISPARADAS dentro del cargador actual

    [Header("State Bools")]
    [SerializeField] bool shooting;//Verdadero cunado estamos DISPARANDO
    [SerializeField] bool canShoot;//Verdadero cuando podemos DISPARAR
    [SerializeField] bool reloading;//Verdadero cuando estamos RECARGANDO

    [Header("Feedback & Graphics")]
    [SerializeField] GameObject muzzleFlash;//Objeto de feedback del fogonazo
    [SerializeField] GameObject hitGraphic;//Elemento de feedback de impacto
    [SerializeField] bool attackIsSounding;

    #endregion

    private void Awake()
    {
        weaponSound = GetComponent<AudioSource>();
        attackIsSounding = false;
        bulletsLeft = ammoSize;
        canShoot = true;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        VisualEffects();

    }

    private void Inputs()
    {
        //Lectura constante del disparo si se reunen las condiciones
        //Si podemos dispara + el imput de disparo se lee + no estamos recargando + nos quedan balas en el cargargador
        if (canShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }

    }


    void VisualEffects()
    {

    }

    void Shoot()
    {
        canShoot = false;//estamos en el proceso de disparo,por lo tanto YA NO PODEMOS DISPARAR hasta que acabe
        //Al inicio del disparo,si hay dispersion, se genera la randomizacion de dicha dispersion(cada disparo tiene una dispersion diferente)
        float spreadX = Random.Range(-spread, spread);
        float spreadY = Random.Range(-spread, spread);
        float spreadZ = Random.Range(-spread, spread);
        Vector3 direction = fpsCam.transform.forward + new Vector3(spreadX, spreadY, spreadZ);

        if (Time.time > shotRateTime)
        {
            GameObject newBala;

            newBala = Instantiate(bala, shootPoint.position, shootPoint.rotation);

            newBala.GetComponent<Rigidbody>().AddForce(shootPoint.forward * shotForce);

            shotRateTime = Time.time + shotRate;

            Destroy(newBala, 2);


        }
        //Raycast del disparo
        //Generar un Raycast: Physics.Raycast(origen, Direccion, Variable Almacen del impacto, longitud del rayo, a que Layer golpea el rayo
        //Si no declaramos layer en un Raycast, golpea a todo lo que tenga colliders
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range, enemyLayer))
        {
           
            //Debug.DrawRay(fpsCam.transform.position, direction, Color.red);
            Debug.Log(hit.collider.name);
            //Aparti de aqui se codean los efectos del Raycast.En este caso es un disparo
            //En este caso se codea hacer daño
            if (hit.collider.CompareTag("Enemy"))
            {
                //Hacer daño concreto
               // Instantiate(GameObject.Find("DisparoFuego1"), hit.point, Quaternion.identity);
                EnemyDamage enemyScript = hit.collider.GetComponent<EnemyDamage>();//ACCESO DIRECTO DEL ENEMIGO HITEADO
                enemyScript.TaikeDamage(damage);
            }

        }
        //Instanciar o visualizar los rfectos del disparo (hitGraphics)
        bulletsLeft--;//Restamos una bala al cargador actual
        bulletsShot--;//Le indicamos al ordenador que hemos disparado X cantidad de balas

        if (!IsInvoking(nameof(ResetShoot)) && !canShoot)
        {
            Invoke(nameof(ResetShoot), shootingCooldown);

        }

        //Disparo mas balas de una
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke(nameof(Shoot), timeBetweenShoots);
        }
      
    }

    void ResetShoot()
    {
        canShoot = true;//la accoin de disparo ha acabado y por lo tanto(si se reunen las condiciones)podemos volver a disparar)

    }

    void Reload()
    {
        reloading = true;//Entrar en estado de reacarga(No se pueden hacer otras acciones con el arma)
        Invoke(nameof(ReloadFinished), reloadTime);//Intentar hacer coincidir el valor de reloadTime con la duracion de la animacion de recarga
    }


    private void ReloadFinished()
    {
        bulletsLeft = ammoSize;//Balas actuales pasan a ser el maximo por cargador actual
        reloading = false;//Salir del estado de recarga(Se pueden hacer otras cosas con el arma)

    }
    #region New Imput Methods

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started && !allowButtonHold)
        {
            muzzleFlash.SetActive(true);
            shooting = true;
            if (!attackIsSounding)
            {
                weaponSound.Play();
                attackIsSounding = true;
            }
        }
        if (context.canceled)
        {
            shooting = false;
            attackIsSounding = false;
            muzzleFlash.SetActive(false);
            weaponSound.Stop();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (bulletsLeft < ammoSize && !reloading)
            {
                Reload();
            }
        }
    }
    #endregion

}

