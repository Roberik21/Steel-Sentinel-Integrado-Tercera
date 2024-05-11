using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectManagament : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonCharacter_1()
    {
        SceneManager.LoadScene("1_Game");


    }


    public void ButtonCharacter_2()
    {
        SceneManager.LoadScene("2_Game");


    }


    public void ButtonCharacter_3()
    {
        SceneManager.LoadScene("3_Game");


    }


    public void ButtonRetroceso()
    {
        SceneManager.LoadScene("MainMenu");


    }

}
