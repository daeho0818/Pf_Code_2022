using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class September_System : GameSystem
{
    [SerializeField] TMPro.TextMeshProUGUI ai_click_count_text;

    [SerializeField] Transform rope;
    public int ai_click_count { get; set; }
    // 게임 승리 조건, 상대와 벌어져야 할 점수 차이
    int win_value = 20;
    protected override void Start()
    {
        base.Start();

        score_helper.system = timer_helper.system = this;

        timer_helper.text_spacing_animation = true;

        score_helper.SetScore(0);

        StartCoroutine(ShowUIs, 0.5f);

        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 2);

        StartCoroutine(StartAnimation(), 5);
        StartCoroutine(() =>
        {
            Ai_Click_Up();
            timer_helper.TimerStart();
        }, 8);
    }

    protected override void Update()
    {
        base.Update();

        if (!is_game_start || game_end) return;

        if (Input.GetMouseButtonDown(0) && !game_end)
        {
            score_helper.ScoreOperation(1);
        }

        rope.position =
            Vector2.Lerp(rope.position, new Vector2(rope.position.x, -(score_helper.GetScore() - ai_click_count) / 10f), 0.5f);
    }

    void Ai_Click_Up()
    {
        ai_click_count++;
        ai_click_count_text.text = $"{ai_click_count}점";

        ui_helper.Scaling(ai_click_count_text.rectTransform, Vector2.one * 1.2f, 5);
        StartCoroutine(() => { ui_helper.Scaling(ai_click_count_text.rectTransform, Vector2.one, 1); }, 0.5f);

        Invoke(nameof(Ai_Click_Up), Random.value / Random.Range(4f, 5f));
    }

    protected override void GameEnd()
    {
        CancelInvoke(nameof(Ai_Click_Up));

        int score = score_helper.GetScore();

        game_clear = score - ai_click_count > 0 + win_value;
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 10)
            {
                LockDown.Instance.LockCount = 10;
                PlayerPrefs.SetInt("Lock Count", 10);
            }
        }

        if (PlayerPrefs.GetInt("September_Best", 0) < score)
        {
            PlayerPrefs.SetInt("September_Best", score);
        }

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"내 점수 : {score}점\n" +
            $"상대 점수 : {ai_click_count}점\n\n" +
            $"점수 차이 : {Mathf.Abs(score - ai_click_count)}점\n" +
            $"최고 점수 : {PlayerPrefs.GetInt("September_Best")}점";

        StartCoroutine(FinishAnimation(), 0.1f);
    }
}
