using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agust_System : GameSystem
{
    [SerializeField] RectTransform backgrounds;
    [SerializeField] RectTransform cake_ui_transform;

    [SerializeField] TMPro.TextMeshProUGUI cake_count_text;

    [SerializeField] int all_cake_count;

    int _cake_count = 0;
    int cake_count
    {
        get => _cake_count;
        set
        {
            _cake_count = value;
            cake_count_text.text = $"x {cake_count}";
        }
    }
    int current_bg_index = 0;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();

        timer_helper.system = this;
        timer_helper.text_spacing_animation = true;

        StartCoroutine(ShowUIs, 2);
        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 4);
        StartCoroutine(StartAnimation(), 7);
        StartCoroutine(timer_helper.TimerStart, 13);
    }

    Vector2 down_position;
    Vector2 up_position;
    float[] target_positions_x = { 0, -1080, -2160 };

    protected override void Update()
    {
        base.Update();

        if (!is_game_start || game_end) return;

        if (Input.GetMouseButtonDown(0))
        {
            down_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            up_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Vector2.Distance(new Vector2(down_position.x, 0), new Vector2(up_position.x, 0)) >= 1)
            {
                if (Vector2.Distance(new Vector2(down_position.y, 0), new Vector2(up_position.y, 0)) < 1)
                {
                    int dir_x = down_position.x > up_position.x ? -1 : 1;
                    if (dir_x == 1 && current_bg_index == 0) return;
                    else if (dir_x == -1 && current_bg_index == 2) return;

                    current_bg_index -= dir_x;
                    ui_helper.Moving(backgrounds, new Vector3(target_positions_x[current_bg_index], 0), 10);
                }
            }
        }

        if (all_cake_count - cake_count == 0)
        {
            timer_helper.ShutTimer();
        }
    }

    public void GetCake(Cake cake)
    {
        RectTransform cake_tr = cake.GetComponent<RectTransform>();
        ui_helper.Moving(cake_tr, new Vector2(-458, -846), 20);
        StartCoroutine(CakeUISizeAnimation(cake_tr));
    }

    protected override void GameEnd()
    {
        int time = timer_helper.GetTime();

        game_clear = time > 0;
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 9)
            {
                LockDown.Instance.LockCount = 9;
                PlayerPrefs.SetInt("Lock Count", 9);
            }
        }

        if (PlayerPrefs.GetInt("Agust_Best", 0) < time)
        {
            PlayerPrefs.SetInt("Agust_Best", time);
        }

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"걸린 시간 : {time}초\n" +
            $"최고 기록 : {PlayerPrefs.GetInt("Agust_Best")}초\n" +
            $"모은 케이크 : {cake_count}개\n" +
            $"못 찾은 케이크 : {all_cake_count - cake_count}개";

        StartCoroutine(FinishAnimation(), 0.1f);
    }

    IEnumerator CakeUISizeAnimation(RectTransform cake)
    {
        while (true)
        {
            if (Vector2.Distance(cake.position, cake_ui_transform.position) <= 5)
            {
                ui_helper.Scaling(cake_ui_transform, Vector2.one * 1.5f, 10);
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        cake_count++;
        yield return new WaitForSeconds(0.5f);

        ui_helper.Scaling(cake_ui_transform, Vector2.one, 10);
        ui_helper.Scaling(cake, Vector2.zero, 20);
    }
}
