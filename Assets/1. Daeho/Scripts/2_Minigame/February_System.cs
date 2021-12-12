using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class February_System : GameSystem
{
    protected override void GameEnd()
    {
        int time = timer_helper.GetTime();

        game_clear = time > 0 && !(Player.MoveCount == 2 && Player.Instance.IsMove);
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 3)
            {
                LockDown.Instance.LockCount = 3;
                PlayerPrefs.SetInt("Lock Count", 3);
            }
        }

        if (PlayerPrefs.GetInt("February_Best", game_timer) > game_timer - time)
        {
            PlayerPrefs.SetInt("February_Best", game_timer - time);
        }

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"걸린 시간 : {game_timer - time}초\n" +
            $"최고 기록 : {PlayerPrefs.GetInt("February_Best")}초";

        StartCoroutine(FinishAnimation(5), 0.1f);
    }

    protected override void Start()
    {
        base.Start();

        timer_helper.system = this;
        timer_helper.text_spacing_animation = true;

        ShowUIs();

        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 2);
        StartCoroutine(StartAnimation(), 5);

        StartCoroutine(timer_helper.TimerStart, 11);
    }

    protected override void Update()
    {
        base.Update();
    }
}
