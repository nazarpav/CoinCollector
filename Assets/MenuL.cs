using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuL : MonoBehaviour
{
    public GameObject Creators;
    void Start()
    {

    }
    void Update()
    {

    }
    public void StartGameButtonPressed()
    {
        //Start game
        SceneManager.LoadScene("Game");
    }
    public void ExitGameButtonPressed()
    {
        //Exit game
        Application.Quit();
    }
    public void GameCreatorsButtonPressed()
    {
        //Creators 
        Creators.SetActive(true);
    }
    public void CloseCreatorsWindow()
    {
        Creators.SetActive(false);
    }
}
