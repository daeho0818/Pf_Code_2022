using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Ending_System : GameSystem
{
    public Character character;
    [SerializeField] Transform enemy_parent;

    [SerializeField] Enemy[] enemy_prefabs;
    [SerializeField] GameObject effect_prefab;

    [SerializeField] Boss boss_obj;

    [SerializeField] BoxCollider2D small_coll;
    [SerializeField] BoxCollider2D big_coll;

    public Bullet bullet_prefab;
    public Transform bullet_parent;

    [SerializeField] TextMeshProUGUI score_text;
    public Queue<Bullet> bullet_pool { get; set; } = new Queue<Bullet>();

    int stage_pattern = 0;

    int _game_score;
    public int game_score
    {
        get => _game_score;
        set
        {
            _game_score = value;
            score_text.text = $"점수 : {_game_score}";
        }
    }

    bool enemy_spawn = true;
    bool boss_spawned = false;

    new void Awake()
    {
        character.system = boss_obj.system = this;
    }
    new void Start()
    {
        base.Start();

        StartCoroutine(ShowUIs, 0.2f);

        StartCoroutine(StartAnimation(), 1f);

        StartCoroutine(ScorePlus(500), 2f);
        StartCoroutine(() => { is_game_start = true; }, 3);

        foreach (Transform tr in bullet_parent)
        {
            bullet_pool.Enqueue(tr.GetComponent<Bullet>());
            tr.GetComponent<CircleCollider2D>().enabled = false;
            tr.gameObject.SetActive(false);
        }
    }

    float[] current_times = { 0, 0, 0, 0 };
    new void Update()
    {
        if (!is_game_start || game_end) return;

        for (int i = 0; i < current_times.Length; i++)
        {
            current_times[i] += Time.deltaTime;
            if (current_times[i] >= enemy_prefabs[i].spawn_range && enemy_spawn)
            {
                SpawnEnemy(i);
                current_times[i] = 0;
            }
        }

        if (game_score >= 2500 && !boss_spawned)
        {
            small_coll.enabled = false;
            big_coll.enabled = true;

            stage_pattern = 1;

            boss_spawned = true;
            enemy_spawn = false;

            StartCoroutine(BossSpawnAnimation());
        }

        if (Input.GetKeyDown(KeyCode.F1))
            character.move_speed = (character.move_speed == 5 ? 15 : 5);
        if (Input.GetKeyDown(KeyCode.F2))
            character.attack_damage = (character.attack_damage == 5 ? 100 : 5);
        if (Input.GetKeyDown(KeyCode.F3))
            character.hp = (character.hp == 100 ? 10000000000000 : 100);

        if (character)
            if (!character.player_dead && character.hp <= 0)
            {
                StartCoroutine(character.DeadAnimation());
                character.player_dead = true;
            }

        if (boss_obj)
            if (!boss_obj.is_dead && boss_obj.hp <= 0)
            {
                StartCoroutine(boss_obj.DestroyAnimation());
            }

        if (!character && !game_end)
        {
            GameEnd();
        }
        else if (!boss_obj && !game_end)
        {
            GameEnd();
        }
    }

    void SpawnEnemy(int index)
    {
        Enemy enemy = Instantiate(enemy_prefabs[index], enemy_parent);
        enemy.transform.position = new Vector2(Random.Range(-1.3f, 1.3f), stage_pattern == 0 ? 1.35f : 6f);
        enemy.move_direction = Vector2.down;
        enemy.system = this;
    }
    public void AddParticle(Vector2 position, Vector2 scale = new Vector2())
    {
        GameObject effect_obj = Instantiate(effect_prefab, position, Quaternion.identity);
        if (scale != new Vector2())
            effect_obj.transform.localScale = scale;
        Destroy(effect_obj, 3);
    }

    public IEnumerator ScorePlus(float score, bool repeat = true)
    {
        while (repeat)
        {
            for (int i = 0; i < score; i++)
            {
                game_score++;
                yield return new WaitForSeconds(0.001f);
            }
            yield return new WaitForSeconds(10);
        }
    }

    IEnumerator BossSpawnAnimation()
    {
        yield return null;

        CameraManager.Instance.ShakeCam(0.3f, 2);

        yield return new WaitForSeconds(2f);

        CameraManager.Instance.ShakeCam(0.6f, 2);

        yield return new WaitForSeconds(2f);

        CameraManager.Instance.ShakeCam(1f, 4);

        yield return new WaitForSeconds(4f);

        CameraManager.Instance.MoveCam(Vector2.zero, 1);
        CameraManager.Instance.ZoomCam(5, 1);

        yield return new WaitForSeconds(8f);

        while (true)
        {
            boss_obj.transform.position = Vector2.Lerp(boss_obj.transform.position, new Vector2(0, 1.57f), 0.01f);
            if (Vector2.Distance(boss_obj.transform.position, new Vector2(0, 1.57f)) <= 0.01f)
            {
                boss_obj.transform.position = new Vector2(0, 1.57f);
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(3f);

        yield return boss_obj.StartCoroutine(boss_obj.StartAnimation());

        enemy_spawn = true;
        boss_obj.is_spawned = true;
        stage_pattern = 2;
    }

    protected override void GameEnd()
    {
        int score = game_score;

        game_clear = character != null;
        game_over = !game_clear;

        if (PlayerPrefs.GetInt("Ending_Best", 0) < score)
        {
            PlayerPrefs.SetInt("Ending_Best", score);
        }

        result_text.text = game_clear ? "게임 클리어!" : "게임 오버..";

        information_text_string = $"내 점수 : {score}점\n" +
            $"최고 점수 : {PlayerPrefs.GetInt("Ending_Best")}점";

        StartCoroutine(FinishAnimation(), 0.1f);
    }
}
