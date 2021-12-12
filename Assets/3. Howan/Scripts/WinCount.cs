using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCount : MonoBehaviour
{
    public static WinCount Instance { get; private set; }
    public int winCount = 0;
    public GameObject Gameover;
    public GameObject Gameclear;
    public GameObject WinPicture;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (winCount == 16)
        {
            //GameClear();
        }
    }
}
