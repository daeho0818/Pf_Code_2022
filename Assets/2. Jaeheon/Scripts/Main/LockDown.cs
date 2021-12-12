using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDown : MonoBehaviour
{
    public static LockDown Instance { get; private set; }
    public UnityEngine.UI.Button[] Button = new UnityEngine.UI.Button[13];
    public GameObject[] Fog = new GameObject[14];
    //public int LockSee = 0;
    public int LockCount;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        LockCount = PlayerPrefs.GetInt("Lock Count", 0);
        if (LockCount == 0)
        {
            LockCount = 1;
            PlayerPrefs.SetInt("Lock Count", 1);
        }

        for (int i = 0; i < LockCount; i++)
        {
            if (Button[i])
            {
                Button[i].enabled = true;
                Fog[i].SetActive(false);
            }
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            LockCount = 55555555;
            PlayerPrefs.SetInt("Lock Count", LockCount);
        }
    }
}
