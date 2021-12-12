using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movingplane : MonoBehaviour
{
    public float RightMax = 2f; 
    public float LeftMax = -2f; 
    public float currentPosition; 
    public float MoveSpeed = 3f;
    public bool Success = false;
    public bool start = false;
    public int FailCount = 0, SuccessCount = 0, GameCount = 0;
    public GameObject[] Game;
    public GameObject Gameover, Gameclear;
    public Transform ArrowPosition;
    Vector3 TargetPosition;
    Vector3 TargetPosition2;


    void Start()
    {
        TargetPosition = new Vector3(0.03f, 0.67f, 7.81f);
        TargetPosition2 = new Vector3(0.03f, -2.5f, 7.81f);
        currentPosition = transform.position.x;
    }

    void Update()
    {
        if (!start) return;

        currentPosition += Time.deltaTime * MoveSpeed;
        if (currentPosition >= RightMax)
        {
            MoveSpeed = MoveSpeed * -1;
            currentPosition = RightMax;
        }
        else if (currentPosition <= LeftMax)
        {
            MoveSpeed = MoveSpeed * -1;
            currentPosition = LeftMax;
        }
        transform.position = new Vector3(currentPosition, -4, 0);
        if (Input.GetMouseButtonDown(0))
        {
            GameCount++;
            if(Success == true)
            {
                Debug.Log("성공");
                SuccessCount++;
                if(MoveSpeed > 0) {
                    MoveSpeed += 2;
                }
                else if(MoveSpeed < 0)
                {
                    MoveSpeed -= 2;
                }          
            }
            else if(Success == false)
            {
                Debug.Log("실패");
                FailCount++;
            }
            NextGame();
        }
        if(SuccessCount >= 3)
        {
            GameClear();

        }
        else if(FailCount >= 3)
        {
            GameOver();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Success")
        {
            Success = true;
        }
        else if(collision.gameObject.tag == "Fail")
        {
            Success = false;
        }
    }
    void GameOver()
    {
        //Gameover.SetActive(true);
        ArrowPosition.position = Vector3.MoveTowards(ArrowPosition.position, TargetPosition2, 0.015f);
        ArrowPosition.rotation = Quaternion.RotateTowards(ArrowPosition.rotation, Quaternion.Euler(110, 0, 0), 0.25f);
    }
    void GameClear()
    {
        //Gameclear.SetActive(true);
        ArrowPosition.position = Vector3.MoveTowards(ArrowPosition.position, TargetPosition, 0.02f);
        ArrowPosition.rotation = Quaternion.RotateTowards(ArrowPosition.rotation, Quaternion.Euler(70, 0, 0), 0.2f);
    }
    void NextGame()
    {
        Game[GameCount].SetActive(true);
        Game[GameCount-1].SetActive(false);
    }
}
