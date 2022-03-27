using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class March_System : GameSystem
{
    [SerializeField] Flag flag_prefab;

    [SerializeField] RectTransform game_canvas;

    [SerializeField] TMPro.TextMeshProUGUI flag_text;

    public int flag_get_count;

    float spawn_timer = 0;
    float waiting_time;
    float rand_x, rand_y;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        score_helper.system = timer_helper.system = this;
        score_helper.text_spacing_animation = true;
        score_helper.SetScore(0);

        StartCoroutine(ShowUIs, 1f);

        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 3f);
        StartCoroutine(StartAnimation(), 7f);

        StartCoroutine(timer_helper.TimerStart, 12f);
    }

    protected override void Update()
    {
        base.Update();

        if (!is_game_start || game_end) return;

        spawn_timer += Time.deltaTime;
        if (spawn_timer > waiting_time)
        {
            waiting_time = Random.Range(1.0f, 1.5f);
            rand_x = Random.Range(-400f, 400f);
            rand_y = Random.Range(-750f, 750f);

            Flag flag =
                Instantiate(flag_prefab, game_canvas);

            flag.system = this;

            flag.transform.localPosition =
                new Vector2(rand_x, rand_y);

            flag.transform.localScale =
                Vector2.one;

            spawn_timer = 0;
        }
        flag_text.text = $"x {score_helper.GetScore() / 50}��";

    }

    protected override void GameEnd()
    {
        int score = score_helper.GetScore();

        game_clear = score >= 1000;
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 4)
            {
                LockDown.Instance.LockCount = 4;
                PlayerPrefs.SetInt("Lock Count", 4);
            }
        }

        if (PlayerPrefs.GetInt("March_Best", 0) < score)
        {
            PlayerPrefs.SetInt("March_Best", score);
        }

        result_text.text = game_clear ? "���� Ŭ����!" : "���� ����..";

        information_text_string = $"�� ���� : {score}��\n" +
            $"�ְ� ���� : {PlayerPrefs.GetInt("March_Best")}��\n" +
            $"���� �±ر� : {score / 50}��";

        StartCoroutine(FinishAnimation(), 0.1f);
    }
}
