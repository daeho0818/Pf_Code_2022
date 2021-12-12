using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHelper : MonoBehaviour
{
    public GameSystem system { get; set; }
    public bool text_spacing_animation { get; set; } = false;

    // 점수 앞에 표시할 문자열 (ex : 점수 : 100점입니다. 의 '점수 : ' 부분)
    [SerializeField] string before_score_str;
    // 점수 뒤에 표시할 문자열 (ex : 점수 : 100점입니다. 의 '점입니다.' 부분)
    [SerializeField] string after_score_str;

    [SerializeField] TMPro.TextMeshProUGUI score_text;

    private int _m_score;
    private int m_score
    {
        get => _m_score;
        set
        {
            if (!system.is_game_start || system.game_end) return;

            if (text_animation)
                TextAnimation((value - _m_score) < 0);
            _m_score = value;
            score_text.text = $"{before_score_str}{_m_score}{after_score_str}";
        }
    }
    private bool text_animation = false;

    float sin_value = 0;
    private void Update()
    {
        if (!system.is_game_start || system.game_end) return;

        if (text_spacing_animation)
        {
            sin_value += 30 * Time.deltaTime;
            score_text.wordSpacing = Mathf.Sin(sin_value * Mathf.Deg2Rad) * 50;
        }
    }

    public void TextAnimation(bool is_negative_value)
    {
        system.ui_helper.Scaling(score_text.rectTransform, Vector2.one * (is_negative_value ? 0.8f : 1.2f), 5);
        system.StartCoroutine(() => { system.ui_helper.Scaling(score_text.rectTransform, Vector2.one, is_negative_value ? 1 : 5); }, 0.5f);
    }

    /// <summary>
    /// 점수 값 설정
    /// </summary>
    /// <param name="score">설정할 점수 값</param>
    public void SetScore(int score) => m_score = score;
    /// <summary>
    /// 점수 받아오는 함수
    /// </summary>
    /// <returns></returns>
    public int GetScore() => m_score;

    /// <summary>
    /// 점수에 매개변수 값을 더하는 함수
    /// </summary>
    /// <param name="score">더할 값, 양수 혹은 음수</param>
    public void ScoreOperation(int score, bool animation = true)
    {
        text_animation = animation;
        m_score += score;
    }
}
