using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject TheEnemy_1;
    public GameObject TheEnemy_2;
    public GameObject TheEnemy_3;
    public int xPos;
    public int zPos;
    public int enemyCounts;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
        
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

}
