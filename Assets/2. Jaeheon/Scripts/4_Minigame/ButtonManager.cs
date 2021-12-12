using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void retry()
    {
        SceneManager.LoadScene(1);
    }
    public void next()
    {
        SceneManager.LoadScene(1);
    }
    public void Pressbutton(int index) //¹öÆ° °ü·Ã ²ÜÆÁ
    {
        switch (index)
        {
            case 0:
            SceneManager.LoadScene(1);
            break;
            case 1:
                SceneManager.LoadScene(2);
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            case 3:
                SceneManager.LoadScene(4);
                break;
            case 4:
                SceneManager.LoadScene(5);
                break;
            case 5:
                SceneManager.LoadScene(6);
                break;
            case 6:
                SceneManager.LoadScene(7);
                break;
            case 7:
                SceneManager.LoadScene(8);
                break;
            case 8:
                SceneManager.LoadScene(9);
                break;
            case 9:
                SceneManager.LoadScene(10);
                break;
            case 10:
                SceneManager.LoadScene(11);
                break;
            case 11:
                SceneManager.LoadScene(12);
                break;
            case 12:
                SceneManager.LoadScene(13);
                break;
            case 13:
                SceneManager.LoadScene(14);
                break;
        }
    }
}
