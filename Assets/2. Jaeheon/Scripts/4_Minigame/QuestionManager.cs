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
    /// Question�� �����ϴ� �Լ�
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
    /// ���� �������� Question�� ��ȯ��
    /// </summary>
    /// <returns></returns>
    public Question GetQuestion()
    {
        // QuestionCount�� �ε����� �Ͽ� q�� ����
        return q[QuestionCount];
    }
    
    /// <summary>
    /// questions �ȿ��� �Ű������� �Ѱ��� ���� ũ�⸸ŭ �������� �����z ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <param name="count">��ȯ�� ����Ʈ ũ��</param>
    /// <returns></returns>
    public List<Question> GetQuestions(int count)
    {
        // ��ȯ�� ���� ���� ����Ʈ
        List<Question> temp = new List<Question>();
        int Cheak;
        // �Ű������� �Ѱܹ��� count��ŭ �ݺ�, ����Ʈ�� ũ�Ⱑ count�� ��
        for (int i = 0; i < count; i++)
        {
            Cheak = Random.Range(0, questions.Count);
            while (temp.Contains(questions[Cheak]))
            {
                Cheak = Random.Range(0, questions.Count);
            }
            // �������� ���� Question�� temp�� ����
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
