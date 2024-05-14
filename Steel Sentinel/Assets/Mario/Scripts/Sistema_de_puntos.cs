using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Sistema_de_puntos : MonoBehaviour
{
    [Header("Damage Configuration")]
    [SerializeField] float vida;
    [SerializeField] float maxvida;
    public float playerDam;
    public GameObject Bala;
    public Transform spawnPoint;

    [Header("Feedback System")]
    [SerializeField] Material damaged;
    [SerializeField] float feedbackTime;
    //[SerializeField] Material original;
    GameObject model;
    //Ref al objeto que contiene el mesh del personaje (solo en caso de que el mesh vaya aparte del código)
    MeshRenderer modelRend; //Ref al meshRenderer del objeto con modelado (permite acceder a su material)
    [SerializeField] GameObject deathEffect;
    void Start()
    {
        model = GameObject.Find("Body");
        vida = maxvida;
        //original = modelRend.material;
        modelRend = model.GetComponent<MeshRenderer>();
    }

    void Update()
    {

        /* if (vida <= 0)
         {
             Destroy(gameObject)  ;
         }*/
        HealthManagement();
    }
    void HealthManagement()
    {
        if (vida <= 0)
        {
            deathEffect.SetActive(true);
            deathEffect.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    public void TakeDamage(int damageToTake)
    {
        //Aquí cabe codear cualquier efecto de recibir daño que se desee
       modelRend.material = damaged; //FEEDBACK DE RECIBIR DAÑO (EN ESTE CASO CAMBIO DE COLOR)
       // health -= damageToTake;
      //  Invoke(nameof(ResetMaterial),feedbackTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bala")
        {
            vida = vida - playerDam ;
           
        }
    }
    /*void ResetMaterial()
    {
        modelRend.material = original;
    }*/
}


