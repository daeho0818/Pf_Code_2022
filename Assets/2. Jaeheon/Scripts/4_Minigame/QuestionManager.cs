using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class QuestionManager : MonoBehaviour
{
    
    public static QuestionManager Instance { get; private set; }
    public List<Question> questions = new List<Question>();
    public List<Question> q = new List<Question>();
    public TextMeshProUGUI txtQuestion;
    public int QuestionCount;
    public int WrongCount = 3;
    public float TimeLimit = 30;
    public GameObject []Question;
    public GameObject Gameover;
    public GameObject Gameclear;
    public bool is_game_start = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        is_game_start = false;
    }

    public void InitStart()
    {
        q = GetQuestions(4);
        StartQuestion();

        Question[GetQuestion().QuestionNumber].SetActive(true);
    }

    /// <summary>
    /// Question을 시작하는 함수
    /// </summary>
    public void StartQuestion()
    {
        txtQuestion.text = q[QuestionCount].strQuestion;
    }

    public void Update()
    {
        //Question[GetQuestion().QuestionNumber].SetActive(true);
        
    }
    /// <summary>
    /// 현재 수행중인 Question을 반환함
    /// </summary>
    /// <returns></returns>
    public Question GetQuestion()
    {
        // QuestionCount를 인덱스로 하여 q에 접근
        return q[QuestionCount];
    }
    
    /// <summary>
    /// questions 안에서 매개변수로 넘겨준 수의 크기만큼 랜덤으로 퀘스틑 반환하는 함수
    /// </summary>
    /// <param name="count">반환할 리스트 크기</param>
    /// <returns></returns>
    public List<Question> GetQuestions(int count)
    {
        // 반환할 값을 담을 리스트
        List<Question> temp = new List<Question>();
        int Cheak;
        // 매개변수로 넘겨받은 count만큼 반복, 리스트의 크기가 count가 됨
        for (int i = 0; i < count; i++)
        {
            Cheak = Random.Range(0, questions.Count);
            while (temp.Contains(questions[Cheak]))
            {
                Cheak = Random.Range(0, questions.Count);
            }
            // 랜덤으로 뽑은 Question을 temp에 담음
            temp.Add(questions[Cheak]);

        }

        return temp;
    }
    public void GameOver()
    {
        Gameover.SetActive(true);
        Time.timeScale = 0;
    }
    public void GameClear()
    {
        Gameclear.SetActive(true);
        Time.timeScale = 0;
    }
}
