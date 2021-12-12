using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WrongText : MonoBehaviour
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
        TEXT.text = "±‚»∏ : " + QuestionManager.Instance.WrongCount;
    }
}
