using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public int PointsSum;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            //AudioManager.Instance.PlaySXF();
            GameManager.Instance.PointsUp(PointsSum);
           // AudioManager.Instance.PlaySXF(2);
            gameObject.SetActive(false);
        }

    }
}
