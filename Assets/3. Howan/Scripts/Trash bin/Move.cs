using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //���� ĳ���� ���ƺ���, ���ѽð�, ����(���ӿ���), ����(Ŭ����)
    // Start is called before the first frame update
    void Start()
    {
        
    }

    int speed = 2; //���ǵ�
    float xMove, yMove;

    // Update is called once per frame
    void Update()
    {
        xMove = 0;
        yMove = 0;

        //if (Input.GetKey(KeyCode.RightArrow))
        //    xMove = speed * Time.deltaTime;
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //    xMove = -speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
            yMove = speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.DownArrow))
            yMove = -speed * Time.deltaTime;
        this.transform.Translate(new Vector3(xMove, yMove, 0));
    }
}