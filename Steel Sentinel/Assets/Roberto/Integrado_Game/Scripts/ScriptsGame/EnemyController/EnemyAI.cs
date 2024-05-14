using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Ai Configuration")]
    [SerializeField] NavMeshAgent agent;//Ref al componente que permite que el objeto tenga IA
    [SerializeField] Transform target;//Ref del transform del onjeto que la Ia va a perseguir
    [SerializeField] LayerMask targetLayer;//Determina cual es la capa de deteccion del target
    [SerializeField] LayerMask grounLayer;//Determina cual es la capa de detteccion del suelo

    [Header("Patroling Stats")]
    public Vector3 walkPoint;//Direccion a la IA que se va a mover sino detecta target
    [SerializeField] float walkPointRange;//Rango de direccion de movimiento si la Ia no deteccta target
    bool walkPointSet;//Bool que determina si la Ia a llegado al objetivo y emtoces cambia de objetivo

    [Header("Aittack Configuration")]
    public float timeBetweenAttacks; //Tiempo de espera entre ataque y ataque
    bool alreadyAttacked;//Bool para determinar si se ha atacado
    //Variables necesarias si el ataque es un disparo Fisico
    [SerializeField] GameObject projectile;//Ref al prefab del projectil
    [SerializeField] Transform shootPoint;//Ref a la posicion desde donde se dispara los projectiles
    [SerializeField] float shootSpeedZ;//Velocidad de disparo hacia delante
    [SerializeField] float shootSpeedY;//Velocidad de disparo hacia arriba (solo disparo es catapulta bolea)

    [Header("States & Detection")]
    [SerializeField] float sightRange;//Rango de deteccion de persecucion de la IA
    [SerializeField] float attackRange;//Rango a partir del cual la IA ataca
    [SerializeField] bool targetInSightRange;//Bool que determina si el target esta a distacia de deteccion
    [SerializeField] bool targetInattackRange;//Bool que determina si el target esta a distacia de ataque

    private void Awake()
    {

   //     target = GameObject.Find("Player").transform;//Al poder persegirlo cuando toca

        target = GameObject.Find("Altar").transform;//Al poder persegirlo cuando toca

        agent = GetComponent<NavMeshAgent>();
    }


    // Update is called once per frame
    void Update()
    {
        //Chequear si el target esta enlos rangos de deteccion y de atque
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, targetLayer);
        targetInSightRange = Physics.CheckSphere(transform.position, attackRange, targetLayer);

        //Cambios dinamicos de estado de la Ia
        //Si no dettecta el target ni esta en rango de ataque: PATRULLA
        if (!targetInSightRange && !targetInattackRange) Patroling();
        //Si detecta el target pero no esta en rango de ataque; PERSIGUE
        if (targetInSightRange && !targetInattackRange) ChaseTarget();
        //Si detecta el target y esta en rango de ataque: ATACA
        if (targetInSightRange && targetInattackRange) AttackTarget();
    }
    void Patroling()
    {
        if (!walkPointSet)
        {
            //Si no existe punto al que dirigirse, inicia el metodo de crearlo
            SearchWalkPoint();
        }
        else
        {
            //Si existe punto, el personaje mueve la IA hacia ese punto
            agent.SetDestination(walkPoint);
        }

        //Sistema para que la IA busque un nuevo destino de patrullaje una vez ha llegado al destino actual
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        if (distanceToWalkPoint.magnitude < 1) { walkPointSet = false; }

    }

    void SearchWalkPoint()
    {
        //Crear el sistema de puntos ""random a patrular

        //Sistema de creacion de puntos Random
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        //Diereccion a la que se mueve la Ia
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        //Deteccion del suelo por debajo del personaje,para evitar caidas
        if (Physics.Raycast(walkPoint, -transform.up, 2f, grounLayer))
        {
            walkPointSet = true;//Comienza el movimiento,porque existe SUELO en el DESTINO

        }
    }

    void ChaseTarget()
    {
        //Una vez detecta el target, lo persigue
        agent.SetDestination(target.position);
    }
    void AttackTarget()
    {
        //Cuando comienza a atacar no se mueve(Se persigue a si mismo)
        agent.SetDestination(transform.position);
        //La IA miara directamente al target
        transform.LookAt(target);

        if (!alreadyAttacked)
        {
            //Si no hemos atacado ya, atacamos
            //AQUI iria el codigo del ataque a personalizar
            //En este ejemplo, vamos a generar una bala que se empuja hacia el player
            GenerateProjectile();
            Debug.Log("Enemy esta atacando");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;

    }

    void GenerateProjectile()
    {
        Rigidbody rb = Instantiate(projectile, shootPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * shootSpeedZ, ForceMode.Impulse);
        //rb.AddForce(transform.forward*shootSpeedY, ForceMode.Impulse);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
