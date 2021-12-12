using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using TMPro;

public class Result_Window : MonoBehaviour
{
    // On / Off 할 창
    public RectTransform result_window;
    // "게임 오버.." or "게임 클리어"로 값 설정
    public TextMeshProUGUI result_text;
    // 중앙에 들어갈 여러 가지 정보들 담은 Text
    public TextMeshProUGUI information_text;
    // fade 처리할 모든 텍스트
    public TextMeshProUGUI[] fading_texts;
    // fade 처리할 모든 이미지
    public UnityEngine.UI.Image[] fading_images;

    public void AcceptButton()
    {
        if (FindObjectOfType<GameSystem>().game_clear)
            LoadScene("Main");
        else
            LoadScene(GetActiveScene().name);
    }
}
