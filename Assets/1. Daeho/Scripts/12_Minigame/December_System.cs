using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class December_System : GameSystem
{
    [SerializeField] Santa santa;

    [Space(20)]
    [SerializeField] Transform backgrounds;
    [SerializeField] float scroll_speed;

    [Space(20)]
    [SerializeField] Transform[] spawn_points = new Transform[3];
    [SerializeField] Obstacle[] obstacles = new Obstacle[3];

    public int miss_obstacles { get; set; } = 0;

    protected override void Start()
    {
        base.Start();

        score_helper.system = timer_helper.system = this;
        score_helper.text_spacing_animation = true;
        score_helper.SetScore(0);

        StartCoroutine(Scrolling());

        StartCoroutine(() =>
        {
            CameraManager.Instance.ZoomCam(2.5f, 2);
            CameraManager.Instance.MoveCam(new Vector2(0, -3.31f), 2);
        }, 0.1f);
        StartCoroutine(() =>
        {
            CameraManager.Instance.ZoomCam(5, 4);
            CameraManager.Instance.MoveCam(Vector2.zero, 4);
        }, 3);

        StartCoroutine(ShowUIs, 5);

        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 7);
        StartCoroutine(StartAnimation(), 11);
        StartCoroutine(timer_helper.TimerStart, 17);
        StartCoroutine(SpawnObstacle, 17);
        StartCoroutine(ScorePlus, 17);

    }

    Vector2 down_position;
    Vector2 up_position;
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
                if (Vector2.Distance(new Vector2(down_position.y, 0), new Vector2(up_position.y, 0)) < 3)
                {
                    int dir_x = down_position.x > up_position.x ? -1 : 1;
                    santa.MoveSide(dir_x);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            santa.MoveSide(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            santa.MoveSide(1);
        }
    }

    protected override void GameEnd()
    {
        CancelInvoke(nameof(ScorePlus));
        CancelInvoke(nameof(SpawnObstacle));

        game_clear = timer_helper.GetTime() <= 0;
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 13)
            {
                LockDown.Instance.LockCount = 13;
                PlayerPrefs.SetInt("Lock Count", 13);
            }
        }


        int score = score_helper.GetScore();

        if (PlayerPrefs.GetInt("December_Best", 0) < score)
        {
            PlayerPrefs.SetInt("December_Best", score);
        }

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"내 점수 : {score}점\n" +
            $"최고 점수 : {PlayerPrefs.GetInt("December_Best")}점\n" +
            $"피한 장애물 : {miss_obstacles}개\n";

        StartCoroutine(FinishAnimation(), 0.1f);
    }

    /// <summary>
    /// 장애물과 충돌했을 경우 산타를 없애는 함수
    /// </summary>
    public void DestroySanta()
    {
        CancelInvoke(nameof(ScorePlus));
        CancelInvoke(nameof(SpawnObstacle));
        Destroy(santa.gameObject);
        CameraManager.Instance.ShakeCam(1, 1);
        StartCoroutine(timer_helper.ShutTimer, 2);
    }

    /// <summary>
    ///  배경을 스크롤해주는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator Scrolling()
    {
        Transform bg1 = backgrounds.GetChild(0);
        Transform bg2 = backgrounds.GetChild(1);

        SpriteRenderer bg1_renderer = bg1.GetComponent<SpriteRenderer>();
        SpriteRenderer bg2_renderer = bg2.GetComponent<SpriteRenderer>();

        while (true)
        {
            bg1.Translate(Vector2.down * Time.deltaTime * scroll_speed);
            bg2.Translate(Vector2.down * Time.deltaTime * scroll_speed);

            if (bg1.localPosition.y <= -23.02f)
            {
                bg1.localPosition = Vector2.up * 23.02f;
                bg2.localPosition = Vector2.zero;

                bg1_renderer.sortingOrder = -5;
                bg2_renderer.sortingOrder = -6;
            }
            else if (bg2.localPosition.y <= -23.02f)
            {
                bg2.localPosition = Vector2.up * 23.02f;
                bg1.localPosition = Vector2.zero;

                bg1_renderer.sortingOrder = -6;
                bg2_renderer.sortingOrder = -5;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    float spawn_range = 2;
    /// <summary>
    ///  장애물을 랜덤 생성하는 함수
    /// </summary>
    void SpawnObstacle()
    {
        int spawn_index = Random.Range(0, 3);
        Transform spawn_point = spawn_points[spawn_index];
        Obstacle obstacle = obstacles[Random.Range(0, 3)];

        Obstacle temp = Instantiate(obstacle, spawn_point);
        temp.system = this;
        temp.transform.localPosition = Vector2.zero;
        temp.move_speed += scroll_speed;

        int current_time = timer_helper.GetTime();


        switch (current_time)
        {
            // 15초 남았을 경우 생성 주기 감소
            case int i when i <= 15:
                spawn_range = 1;
                break;
            // 30초 남았을 경우 스크롤 속도 증가, 올라가는 점수 양 증가
            case int i when i <= 30:
                scroll_speed = 5;
                score_plus_time = 0.05f;
                break;
            // 45초 남았을 경우 한마리 추가 생성
            case int i when i <= 45:
                int repeat_spawn_index;
                do
                {
                    repeat_spawn_index = Random.Range(0, 3);
                }
                while (repeat_spawn_index == spawn_index);

                spawn_point = spawn_points[repeat_spawn_index];

                temp = Instantiate(obstacles[Random.Range(0, 3)], spawn_point);
                temp.system = this;
                temp.transform.localPosition = Vector2.zero;
                temp.move_speed += scroll_speed;
                break;
            default:
                break;
        }

        Invoke(nameof(SpawnObstacle), spawn_range);
    }

    float score_plus_time = 0.1f;
    void ScorePlus()
    {
        score_helper.ScoreOperation(1);
        Invoke(nameof(ScorePlus), score_plus_time);
    }
}
