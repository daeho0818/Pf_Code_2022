using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class November_System : GameSystem
{
    [SerializeField] Pen pen;
    protected override void Start()
    {
        base.Start();

        timer_helper.system = this;

        StartCoroutine(ShowUIs, 0.5f);

        StartCoroutine(() => { timer_helper.SetTimer(0, true); }, 2);
        StartCoroutine(StartAnimation(), 4);
        StartCoroutine(timer_helper.TimerStart, 10);
        StartCoroutine(AddForcePen, 11);
    }

    protected override void Update()
    {
        base.Update();

        if (!is_game_start || game_end) return;

        if (Input.GetMouseButtonDown(0))
            pen.Touch(Camera.main.ScreenToWorldPoint(Input.mousePosition));

        if (pen.is_falled) timer_helper.ShutTimer();
    }

    protected override void GameEnd()
    {
        int time = timer_helper.GetTime();

        game_clear = true;
        game_over = true;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 12)
            {
                LockDown.Instance.LockCount = 12;
                PlayerPrefs.SetInt("Lock Count", 12);
            }
        }

        if (PlayerPrefs.GetInt("November_Best", 0) < time)
        {
            PlayerPrefs.SetInt("November_Best", time);
        }

        result_text.text = "���� ����!";

        information_text_string = $"��ƾ �ð� : {timer_helper.GetTime()}��\n" +
            $"�ְ� ��� : {PlayerPrefs.GetInt("November_Best")}��\n";

        StartCoroutine(FinishAnimation(), 0.1f);
    }

    void AddForcePen()
    {
        pen.Touch(new Vector2(pen.transform.position.x + Random.Range(-5f, 5f), 0));
        Invoke(nameof(AddForcePen), Random.Range(3f, 5f));
    }
}
