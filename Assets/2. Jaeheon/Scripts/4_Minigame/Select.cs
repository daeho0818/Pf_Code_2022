using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{
    int Count = 0;
    public GameObject Hand;
    public GameObject Button, Button2, Button3, Button4;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckQuestion()
    {
        if (!QuestionManager.Instance.is_game_start) return;

        // 현재 수행중인 Question을 받아옴
        var q = QuestionManager.Instance.GetQuestion();
        // 정답일 경우
        if (q.Success == Count)
        {
            print($"q : {q.strQuestion}, s : {q.Success}");
            // QuestionCount를 더해줌으로써 수행하던 Question의 다음 Question을 수행하도록 함
            if (QuestionManager.Instance.QuestionCount < 3)
            {
                QuestionManager.Instance.Question[QuestionManager.Instance.GetQuestion().QuestionNumber].SetActive(false);
                QuestionManager.Instance.QuestionCount++;
                QuestionManager.Instance.StartQuestion();
                QuestionManager.Instance.Question[QuestionManager.Instance.GetQuestion().QuestionNumber].SetActive(true);
            }
            else
            {
                var v = FindObjectOfType<GameSystem>();
                v.timer_helper.ShutTimer();
            }
        }
        else if (q.Success != Count && QuestionManager.Instance.WrongCount != 0)
        {
            QuestionManager.Instance.WrongCount--;

            if (QuestionManager.Instance.WrongCount <= 0)
            {
                var v = FindObjectOfType<GameSystem>();
                v.timer_helper.ShutTimer();
            }
        }
    }

    public void PressButton(int index) //버튼 관련 꿀팁
    {
        switch (index)
        {
            case 0:
                Count = 0;
                Hand.SetActive(true);
                Hand.transform.position = Button.transform.position + new Vector3(20, -60, 0);
                CheckQuestion();
                Invoke("HandHide", 1);
                break;
            case 1:
                Count = 1;
                Hand.SetActive(true);
                Hand.transform.position = Button2.transform.position + new Vector3(20, -60, 0);
                CheckQuestion();
                Invoke("HandHide", 1);
                break;
            case 2:
                Count = 2;
                Hand.SetActive(true);
                Hand.transform.position = Button3.transform.position + new Vector3(20, -60, 0);
                CheckQuestion();
                Invoke("HandHide", 1);
                break;
            case 3:
                Count = 3;
                Hand.SetActive(true);
                Hand.transform.position = Button4.transform.position + new Vector3(20, -60, 0);
                CheckQuestion();
                Invoke("HandHide", 1);
                break;
        }
    }
    public void HandHide()
    {
        Hand.SetActive(false);
    }
}
