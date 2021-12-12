using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class July_System : GameSystem
{
    [SerializeField] Movingplane plane;

    [SerializeField] TMPro.TextMeshProUGUI success_text;
    [SerializeField] TMPro.TextMeshProUGUI fail_text;

    protected override void GameEnd()
    {
        int time = timer_helper.GetTime();

        game_clear = plane.SuccessCount >= 3 && time > 0;
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 8)
            {
                LockDown.Instance.LockCount = 8;
                PlayerPrefs.SetInt("Lock Count", 8);
            }
        }

        if (PlayerPrefs.GetInt("July_Best", game_timer) > game_timer - time)
        {
            PlayerPrefs.SetInt("July_Best", game_timer - time);
        }

        result_text.text = game_clear ? "���� Ŭ����!" : "���� ����..";

        information_text_string = $"�ɸ� �ð� : {game_timer - time}��\n" +
            $"�ְ� ��� : {PlayerPrefs.GetInt("July_Best")}��";

        StartCoroutine(FinishAnimation(5), 0.1f);
    }

    protected override void Start()
    {
        base.Start();

        timer_helper.system = this;

        ShowUIs();

        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 2);
        StartCoroutine(StartAnimation(), 5);

        StartCoroutine(timer_helper.TimerStart, 11);
        StartCoroutine(() => { plane.start = true; }, 11);
    }

    protected override void Update()
    {
        base.Update();

        if (!is_game_start || game_end) return;

        if (plane.SuccessCount >= 3 || plane.FailCount >= 3)
            timer_helper.ShutTimer();

        success_text.text = $"���� Ƚ�� : {plane.SuccessCount} / {3}";
        fail_text.text = $"���� Ƚ�� : {plane.FailCount} / {3}";
    }
}
