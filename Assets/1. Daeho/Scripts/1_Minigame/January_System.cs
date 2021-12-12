using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class January_System : GameSystem
{
    [SerializeField] Moon moon;
    [SerializeField] Basket basket;
    [SerializeField] Tteok _tteok;

    public Tteok tteok { get => _tteok; set { tteok = value; } }

    public int operating_score { get; set; } = 50;

    protected override void Awake()
    {
        base.Awake();
    }


    protected override void Start()
    {
        base.Start();

        score_helper.system = timer_helper.system = moon.system = basket.system = this;
        score_helper.text_spacing_animation = true;
        timer_helper.text_spacing_animation = !score_helper.text_spacing_animation;

        score_helper.SetScore(0);

        CameraManager.Instance.MoveCam(moon.transform.position, 2);
        StartCoroutine(() => { CameraManager.Instance.ShakeCam(1, 0.3f); }, 2);
        StartCoroutine(moon.TteokAnimation(), 2);
        StartCoroutine(() => { CameraManager.Instance.MoveCam(Vector2.zero, 2); }, 7);

        StartCoroutine(ShowUIs, 8);

        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 9);
        StartCoroutine(StartAnimation(), 12);
        StartCoroutine(TteokSpawn(), 18);
        StartCoroutine(timer_helper.TimerStart, 18);

    }

    protected override void Update()
    {
        base.Update();

        if (!is_game_start || game_end) return;

        if (timer_helper.GetTime() == 10)
        {
            CameraManager.Instance.ShakeCam(1, 1.5f);
            StartCoroutine(() => { CameraManager.Instance.ShakeCam(1, 0.3f); }, 0.7f);
            StartCoroutine(() => { CameraManager.Instance.ShakeCam(1, 0.3f); }, 0.7f);
        }
    }
    protected override void GameEnd()
    {
        int score = score_helper.GetScore();

        game_clear = score > 1000;
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 2)
            {
                LockDown.Instance.LockCount = 2;
                PlayerPrefs.SetInt("Lock Count", 2);
            }
        }

        if (PlayerPrefs.GetInt("Janurary_Best", 0) < score)
        {
            PlayerPrefs.SetInt("Janurary_Best", score);
        }

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"내 점수 : {score}점\n" +
            $"최고 점수 : {PlayerPrefs.GetInt("Janurary_Best")}점\n" +
            $"받은 떡 : {score / operating_score}개";

        StartCoroutine(FinishAnimation(), 0.1f);
    }

    /// <summary>
    /// 떡을 생성해주는 함수
    /// </summary>
    IEnumerator TteokSpawn()
    {
        Tteok tteokObj;
        float spawn_range = Random.Range(1.5f, 3f);

        while (true)
        {
            if (game_end) break;

            tteokObj = Instantiate(tteok, new Vector2(Random.Range(-2.3f, 2.3f), 6), Quaternion.identity);
            tteokObj.system = this;

            if (timer_helper.GetTime() <= 10)
                spawn_range = Random.Range(0.3f, 1f);

            yield return new WaitForSeconds(spawn_range);
        }
    }
}
