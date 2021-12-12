using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountText : MonoBehaviour
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
        TEXT.text = "제한 시간 : " + QuestionManager.Instance.TimeLimit;
    }
}
