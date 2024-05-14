using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Shoot : MonoBehaviour
{
    public GameObject bala;
    public float shotForce = 1500;
    public float shotRate = 0.5f;
    private float shotRateTime = 0;
    public Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Shot();
    }
    void Shot()
    {
        if (Input.GetButtonDown("Fire1"))
        {


            if (Time.time > shotRateTime)
            {
                GameObject newBala;

                newBala = Instantiate(bala, spawnPoint.position, spawnPoint.rotation);

                newBala.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shotForce);

                shotRateTime = Time.time + shotRate;

                Destroy(newBala, 2);


            }

        }
    }
}