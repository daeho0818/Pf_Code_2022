using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class April_Minigame : GameSystem
{
    protected override void GameEnd()
    {
        int time = timer_helper.GetTime();

        if (!game_clear && !game_over)
        {
            game_clear = time > 0 && QuestionManager.Instance.WrongCount > 0;
            game_over = !game_clear;
        }

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 5)
            {
                LockDown.Instance.LockCount = 5;
                PlayerPrefs.SetInt("Lock Count", 5);
            }
        }

        if (PlayerPrefs.GetInt("April_Best", game_timer) > game_timer - time)
        {
            PlayerPrefs.SetInt("April_Best", game_timer - time);
        }

        result_text.text = game_clear ? "���� Ŭ����!" : "���� ����..";

        information_text_string = $"�ɸ� �ð� : {game_timer - time}��\n" +
            $"�ְ� ��� : {PlayerPrefs.GetInt("April_Best")}��";

        StartCoroutine(FinishAnimation(), 0.1f);
    }

    protected override void Start()
    {
        base.Start();

        timer_helper.system = this;

        ShowUIs();

        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 2);
        StartCoroutine(StartAnimation(), 5);

        StartCoroutine(timer_helper.TimerStart, 11);
        StartCoroutine(QuestionManager.Instance.InitStart, 11);
        StartCoroutine(() => { QuestionManager.Instance.is_game_start = true; }, 11);
    }

    protected override void Update()
    {
        base.Update();
    }
}
