using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionText : MonoBehaviour
{
    private Text text;
    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "맞은 문제 개수 : " + QuestionManager.Instance.QuestionCount + 1 + " 개";
    }
}
