using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : BaseObj
{

    [SerializeField] TMPro.TextMeshProUGUI hp_text;
    [SerializeField] UnityEngine.UI.Slider hp_slider;

    Coroutine pattern_1;
    Coroutine pattern_2;

    float pattern_1_time = 0;
    float pattern_2_time = 0;

    public bool is_dead = false;
    public bool is_spawned = false;
    protected override void ObjectDead()
    {
        base.ObjectDead();
    }

    protected override void Start()
    {
        base.Start();

        sr = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (hp < 0) hp = 0;
        hp_text.text = $"HP : {hp} / {max_hp}";

        pattern_1_time += Time.deltaTime;
        pattern_2_time += Time.deltaTime;

        if (is_spawned && pattern_1 == null && pattern_2 == null && pattern_1_time >= 15)
            pattern_1 = StartCoroutine(Pattern1());

        if (is_spawned && pattern_1 == null && pattern_2 == null && pattern_2_time >= 10)
            pattern_2 = StartCoroutine(Pattern2());
    }

    public IEnumerator StartAnimation()
    {
        RectTransform rt = hp_slider.GetComponent<RectTransform>();

        while (true)
        {
            rt.localPosition = Vector2.Lerp(rt.localPosition, new Vector2(0, 757), 0.3f);
            if (Vector2.Distance(rt.localPosition, new Vector2(0, 757)) <= 0.01f)
            {
                rt.localPosition = new Vector2(0, 757);
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Pattern1()
    {
        Vector2 target_position = transform.position + Vector3.up * 1.5f;
        while (true)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, target_position, 0.01f);
            if (Vector2.Distance(transform.localPosition, target_position) <= 0.1f)
            {
                transform.localPosition = target_position;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1f);

        target_position = transform.position + Vector3.down * 1.5f;
        while (true)
        {
            transform.localPosition = Vector2.Lerp(transform.localPosition, target_position, 0.7f);
            if (Vector2.Distance(transform.localPosition, target_position) <= 0.01f)
            {
                transform.localPosition = target_position;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
        CameraManager.Instance.ShakeCam(1.5f, 2);

        Vector3 position;
        for (int i = 0; i < 360; i += 360 / 60)
        {
            position = new Vector2(Mathf.Cos(i * Mathf.Deg2Rad), Mathf.Sin(i * Mathf.Deg2Rad));
            Fire(position, Vector2.one * 0.2f, (position - transform.position).normalized, 5, 1, Bullet.BulletType.Enemy, system);
        }

        pattern_1 = null;
        pattern_1_time = 0;
    }

    IEnumerator Pattern2()
    {
        while (true)
        {
            sr.color = Color.Lerp(sr.color, Color.yellow, 0.1f);
            if (sr.color.r >= 0.9f)
            {
                sr.color = Color.yellow;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        Vector2[] dirs = new Vector2[4];
        dirs[0] = new Vector2(-0.5f, 0.5f);
        dirs[1] = new Vector2(0.5f, 0.5f);
        dirs[2] = new Vector2(-0.5f, -0.5f);
        dirs[3] = new Vector2(0.5f, -0.5f);

        int repeat_count = 0;
        float[] value = { 0, 0, 0, 0 };
        while (true)
        {
            for (int i = 0; i < 4; i++)
            {
                Fire(transform.position, Vector2.one * 0.2f, dirs[i], 5, 1, Bullet.BulletType.Enemy, system);
            }

            repeat_count++;
            yield return new WaitForSeconds(0.05f);

            if (repeat_count >= 500)
            {
                pattern_2_time = 0;
                pattern_2 = null;
                yield break;
            }
            else if (repeat_count >= 100)
            {
                for (int i = 0; i < 4; i++)
                {
                    value[i] += 50 * Time.deltaTime;

                    dirs[i] += new Vector2(Mathf.Cos(value[i] * Mathf.Deg2Rad), Mathf.Sin(value[i] * Mathf.Deg2Rad)) / 5;
                    dirs[i].Normalize();
                }
            }
        }
    }

    public IEnumerator DestroyAnimation()
    {
        CameraManager.Instance.MoveCam(transform.position, 5);
        CameraManager.Instance.ZoomCam(5, 5);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                float size = Random.Range(1, 3);
                system.AddParticle(transform.position + new Vector3(Random.Range(-0.5f * j, 0.5f * i), Random.Range(-0.5f * i, 0.5f * j)),
                    new Vector2(size, size));
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(0.5f);
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 15; j++)
            {
                float size = Random.Range(2, 4);
                system.AddParticle(transform.position + new Vector3(Random.Range(-0.5f * j, 0.5f * i), Random.Range(-0.5f * i, 0.5f * j)),
                    new Vector2(size, size));
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}