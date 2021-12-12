using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kangwongi : MonoBehaviour
{
    public float TimeCount = 0;
    public GameObject Warning;
    public int LightCount = 1;
    public GameObject Wongi;
    public GameObject Wongi2;

    // Start is called before the first frame update
    void Start()
    {
        Wongi2.gameObject.SetActive(false);
        Wongi.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<GameSystem>().is_game_start) return;

        TimeCount += Time.deltaTime;
        if (TimeCount >= Random.Range(5, 8) && LightCount == 1)
        {
            LightCount = 2;
        }
        if (LightCount == 2)
        {
            Warning.SetActive(true);
            Debug.Log("說除碳 遽綠");
            Invoke("RedLight", 3);
        }
    }
    void RedLight()
    {
        LightCount = 3;
        Warning.SetActive(false);
        Debug.Log("說除碳");
        GetComponent<Renderer>().material.color = Color.red;
        Wongi2.gameObject.SetActive(true);
        Wongi.gameObject.SetActive(false);
        Invoke("GreenLight", 3);
        Player.MoveCount = 2;
    }
    void GreenLight()
    {
        Player.MoveCount = 1;
        LightCount = 1;
        Debug.Log("蟾煙碳");
        Wongi2.gameObject.SetActive(false);
        Wongi.gameObject.SetActive(true);
        //this.gameObject.GetComponent<Renderer>().material.color = new Color(0,0,0);
        TimeCount = 0;
    }
}
