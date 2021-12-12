using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class June_System : GameSystem
{
    [SerializeField] Virus[] virus_prefab;

    [SerializeField] RectTransform game_canvas;

    public int virus_death_count;

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

            Virus virus =
                Instantiate(virus_prefab[Random.Range(0, 3)],
                    game_canvas);

            virus.transform.localPosition =
                new Vector2(rand_x, rand_y);

            virus.transform.localScale =
                Vector2.one;

            virus.system = this;

            spawn_timer = 0;
        }
    }

    protected override void GameEnd()
    {
        int score = score_helper.GetScore();

        game_clear = score >= 2000;
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 7)
            {
                LockDown.Instance.LockCount = 7;
                PlayerPrefs.SetInt("Lock Count", 7);
            }
        }

        if (PlayerPrefs.GetInt("June_Best", 0) < score)
        {
            PlayerPrefs.SetInt("June_Best", score);
        }

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"내 점수 : {score}점\n" +
            $"최고 점수 : {PlayerPrefs.GetInt("June_Best")}점\n" +
            $"박멸 바이러스 : {score / 50}마리";

        StartCoroutine(FinishAnimation(), 0.1f);
    }
}
