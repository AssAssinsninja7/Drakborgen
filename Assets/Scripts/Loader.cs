using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public GameObject gameMGR;

    /*This class just instansiates the gamemanager when the game starts, if
     other managers are added then they should be initiated here aswell*/
    void Awake()
    {
        //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
        if (GameManager.instance == null)
        {
            //Instantiate gameManager prefab
            Instantiate(gameMGR);
        }
        //if (SceneManager.GetActiveScene() == SceneManager.GetSceneAt(1))
        //{
          
        //}
    }   
}
