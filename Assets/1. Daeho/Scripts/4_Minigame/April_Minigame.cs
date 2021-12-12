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

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"걸린 시간 : {game_timer - time}초\n" +
            $"최고 기록 : {PlayerPrefs.GetInt("April_Best")}초";

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
