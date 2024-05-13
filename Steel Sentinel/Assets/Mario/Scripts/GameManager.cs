using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.Log("Game Manager is null");
            }
            return instance;
        }
    }
    public int points;
    public int winPoints;
    [SerializeField] GameObject WinMenu;
    [SerializeField] TMP_Text pointstoText;
    // Update is called once per frame
    void Update()
    {
        

    }
    public void won()
    {
        if (points >= winPoints)
        {
           // AudioManager.Instance.StopCurrentMusic();
           // AudioManager.Instance.PlaySXF(7);
            WinMenu.gameObject.SetActive(true);
        }


    }
    public void PointsUp(int gain)
    {
        points += gain;
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);

    }
}
