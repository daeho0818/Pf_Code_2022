using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;

public class Setting_Window : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GoHome()
    {
        LoadScene("Main");
    }

    public void ReTry()
    {
        LoadScene(GetActiveScene().name);
    }
}
