using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Data/Question", order = 0)]
public class Question : ScriptableObject
{
    // 화면에 띄울 텍스트
    public string strQuestion;
    // 정답 번호
    public int Success;
    public int QuestionNumber;
}
