using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Data/Question", order = 0)]
public class Question : ScriptableObject
{
    // ȭ�鿡 ��� �ؽ�Ʈ
    public string strQuestion;
    // ���� ��ȣ
    public int Success;
    public int QuestionNumber;
}
