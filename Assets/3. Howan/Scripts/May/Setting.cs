using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    float randx, randy;

    // Start is called before the first frame update
    void Start()
    {
        randx = Random.Range(-2.5f, 2.5f);
        randy = Random.Range(-4.5f, 4.5f);
        transform.position = new Vector3(randx, randy, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}