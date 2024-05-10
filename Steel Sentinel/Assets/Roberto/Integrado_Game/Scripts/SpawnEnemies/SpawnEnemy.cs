using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    private static SpawnEnemy instance;

    public static SpawnEnemy Instance
    {
        get { return instance; }
    }


    public GameObject TheEnemy_1;
    public GameObject TheEnemy_2;
    public GameObject TheEnemy_3;
    public int xPos;
    public int zPos;
    public int enemyCounts;
    [SerializeField] int maxEnemies;
    [SerializeField] bool canSpawn;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(EnemyDrop());
        
    }

    private void Update()
    {
        
        if (enemyCounts < maxEnemies) { Spawn(); }
    }

    IEnumerator EnemyDrop()
    {
        while (enemyCounts < 10)//Numero de enemigos
        {
            xPos = Random.Range(-80, -62);//Limite de rango en X del Spawn
            zPos = Random.Range(3, 21);//Limite de rango en Z del Spawn
            Instantiate(TheEnemy_1, new Vector3(xPos, 0.75f, zPos), Quaternion.identity);//Cambiar el 43 por la altura donde respawnea el enemigo
            Instantiate(TheEnemy_2, new Vector3(xPos, 0.75f, zPos), Quaternion.identity);
            Instantiate(TheEnemy_3, new Vector3(xPos, 0.75f, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);//Tiempo que tienen de aparecer unos a otros
            enemyCounts += 1;

        }

    }

    void Spawn()
    {
        if (canSpawn) 
        {
            canSpawn = false;
            xPos = Random.Range(-80, -62);//Limite de rango en X del Spawn
            zPos = Random.Range(3, 21);//Limite de rango en Z del Spawn
            int nEnemy = Random.Range(1, 3);
            Debug.Log(nEnemy);
            if (nEnemy == 1) { Instantiate(TheEnemy_1, new Vector3(xPos, 0.75f, zPos), Quaternion.identity); } //Cambiar el 43 por la altura donde respawnea el enemigo
            if (nEnemy == 2) { Instantiate(TheEnemy_2, new Vector3(xPos, 0.75f, zPos), Quaternion.identity); }
            if (nEnemy == 3) { Instantiate(TheEnemy_3, new Vector3(xPos, 0.75f, zPos), Quaternion.identity); }
            enemyCounts += 1;
            Invoke(nameof(ResetSpawn), 1f);
        }
    }

    void ResetSpawn()
    {
        canSpawn = true;
    }
}
