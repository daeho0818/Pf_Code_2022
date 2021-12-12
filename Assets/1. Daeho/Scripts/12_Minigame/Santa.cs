using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
    Coroutine move_side;
    int line_number = 2;
    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary>
    /// 산타를 이동시키는 함수
    /// </summary>
    /// <param name="dir_x">-1 : 왼쪽, 1 : 오른쪽</param>
    public void MoveSide(int dir_x)
    {
        if (dir_x == -1 && line_number == 1) return;
        if (dir_x == 1 && line_number == 3) return;
        if (move_side != null) return;

        line_number += dir_x;

        move_side = StartCoroutine(_MoveSide(dir_x));
    }
    IEnumerator _MoveSide(int dir_x)
    {
        WaitForSeconds second = new WaitForSeconds(0.001f);
        Vector2 target_position = transform.position + new Vector3(dir_x * 1.75f, 0);

        while (true)
        {
            transform.position = Vector2.MoveTowards(transform.position, target_position, Time.deltaTime * 10);
            if (Vector2.Distance(transform.position, target_position) <= 0.01f)
            {
                transform.position = target_position;
                break;
            }
            yield return second;
        }

        move_side = null;
    }
}
