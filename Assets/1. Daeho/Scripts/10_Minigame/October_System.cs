using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class October_System : GameSystem
{
    [SerializeField] Bead player_bead;
    [SerializeField] Bead bead_prefab;
    [SerializeField] Transform board;

    public Sprite[] beads_sprite;

    RaycastHit2D[] hits;

    Vector2 down_position;
    Vector2 up_position;

    bool ready_fire = false;
    protected override void Start()
    {
        base.Start();

        score_helper.system = timer_helper.system = player_bead.system = this;
        score_helper.text_spacing_animation = true;
        score_helper.SetScore(0);

        StartCoroutine(ShowUIs, 0.5f);

        StartCoroutine(() => { timer_helper.SetTimer(game_timer); }, 2);
        StartCoroutine(StartAnimation(), 6);
        StartCoroutine(timer_helper.TimerStart, 12);

        StartCoroutine(SpawnBead(), 12);
        StartCoroutine(player_bead.StartAnimation(), 12);
    }
    protected override void Update()
    {
        base.Update();

        if (!is_game_start || game_end) return;

        if (Input.GetMouseButtonDown(0))
        {
            down_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            hits = Physics2D.RaycastAll(down_position, Vector3.forward, 10);

            ready_fire = false;

            Bead bead;
            foreach (var hit in hits)
            {
                bead = hit.transform.GetComponent<Bead>();
                if (bead)
                {
                    if (bead.bead_type == Bead.BeadType.Player)
                    {
                        ready_fire = true;
                        player_bead = bead;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            up_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (ready_fire && !player_bead.moving)
                ShootBead();
        }
    }

    protected override void GameEnd()
    {
        int score = score_helper.GetScore();

        game_clear = score >= 2500;
        game_over = !game_clear;

        if (game_clear)
        {
            if (LockDown.Instance.LockCount < 11)
            {
                LockDown.Instance.LockCount = 11;
                PlayerPrefs.SetInt("Lock Count", 11);
            }
        }

        if (PlayerPrefs.GetInt("October_Best", 0) < score)
        {
            PlayerPrefs.SetInt("October_Best", score);
        }

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"내 점수 : {score}점\n" +
            $"최고 점수 : {PlayerPrefs.GetInt("October_Best")}점\n" +
            $"떨어뜨린 구슬 : {score / 50}개";

        StartCoroutine(FinishAnimation(), 0.1f);
    }

    /// <summary>
    /// 구슬을 발사하는 함수
    /// </summary>
    void ShootBead()
    {
        Vector2 direction = (down_position - up_position).normalized;
        float distance = Vector2.Distance(down_position, up_position);
        player_bead.ShootThisBead(direction, distance * 3);
        StartCoroutine(() => { }, 3);
    }

    float spawn_range = 3;
    /// <summary>
    /// 구슬을 랜덤 위치에 생성하는 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBead()
    {
        float time;
        while (true)
        {
            time = timer_helper.GetTime();

            Bead bead = Instantiate(bead_prefab, board);
            bead.transform.localPosition = new Vector2(Random.Range(-0.45f, 0.45f), Random.Range(-0.45f, 0.45f));
            bead.transform.localScale = Vector2.one * 2;
            bead.system = this;
            bead.GetComponent<SpriteRenderer>().sprite = beads_sprite[Random.Range(0, beads_sprite.Length)];

            switch (time)
            {
                case float f when f <= 0:
                    yield break;
                case float f when f <= 15:
                    spawn_range = 0.5f;
                    break;
                case float f when f <= 30:
                    spawn_range = 1f;
                    break;
                case float f when f <= 45:
                    spawn_range = 2f;
                    break;
                default:
                    break;
            }

            yield return new WaitForSeconds(spawn_range);
        }
    }
}
