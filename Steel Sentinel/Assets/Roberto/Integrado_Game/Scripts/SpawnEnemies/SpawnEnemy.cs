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

    Ienumerator EnemyDrop()
    {
        while (enemyCounts < 10)//Numero de enemigos
        {
            xPos = Random.Range(1, 50);//Limite de rango en X del Spawn
            zPos = Random.Range(1, 31);//Limite de rango en Z del Spawn
            Instiate(TheEnemy_1, new Vector3(xPos, 43, zPos), Quaternion.identity);//Cambiar el 43 por la altura donde respawnea el enemigo
            Instiate(TheEnemy_2, new Vector3(xPos, 43, zPos), Quaternion.identity);
            Instiate(TheEnemy_3, new Vector3(xPos, 43, zPos), Quaternion.identity);
            yield return new WaitForSecondos(0.1f);//Tiempo que tienen de aparecer unos a otros
            enemyCounts += 1;

        }

    }

}
