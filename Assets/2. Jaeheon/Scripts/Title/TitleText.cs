using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleText : MonoBehaviour
{
    public TextMeshProUGUI TEXT;
    // Start is called before the first frame update
    void Start()
    {
        TEXT = GetComponent<TextMeshProUGUI>();
    }

    bool fade_in = false;
    void Update()
    {
        //TEXT.text = "Touch To Start";
        //TEXT.color = Color.Lerp(TEXT.color, TEXT.color - new Color(0, 0, 0, 1), 0.01f);

        if (TEXT.color.a <= 0.1f) fade_in = true;
        else if (TEXT.color.a >= 0.9f) fade_in = false;

        if (!fade_in)
        {
            TEXT.color = Color.Lerp(TEXT.color, TEXT.color - new Color(0, 0, 0, 1), 0.002f);
        }
        else
        {
            TEXT.color = Color.Lerp(TEXT.color, TEXT.color + new Color(0, 0, 0, 1), 0.002f);
        }
    }

}
