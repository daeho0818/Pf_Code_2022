using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript_ : MonoBehaviour
{
    public static int GameScore3 = 0;
    public GameObject prefab;
    float Timer = 0;
    float WaitingTime;
    float randx, randy;

    // Start is called before the first frame update
    void Start()
    {
        //WaitingTime = Random.Range(1.0f, 8.0f);
        WaitingTime = Random.Range(1.0f, 1.5f);
        randx = Random.Range(-2.3f, 2.3f);
        randy = Random.Range(-4.5f, 4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > WaitingTime)
        {
            Instantiate(prefab, new Vector3(randx, randy, transform.position.z), Quaternion.identity);
            randx = Random.Range(-2.3f, 2.3f);
            randy = Random.Range(-4.5f, 4.5f);
            Timer = 0;
        }
    }
}
