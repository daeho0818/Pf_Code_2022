using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameName : MonoBehaviour
{
    private float sin_value = 180;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        sin_value += 100 * Time.deltaTime;
        transform.position += new Vector3(0, Mathf.Sin(sin_value * Mathf.Deg2Rad) * 0.1f);
    }
}
