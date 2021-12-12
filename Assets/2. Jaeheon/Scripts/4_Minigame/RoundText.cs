using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundText : MonoBehaviour
{
    public TextMeshProUGUI TEXT;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        TEXT = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (QuestionManager.Instance.QuestionCount == 4)
        {
            TEXT.text = QuestionManager.Instance.QuestionCount + " Round";
        }
        else
        {
            TEXT.text = QuestionManager.Instance.QuestionCount + 1 + " Round";
        }
    }
}
