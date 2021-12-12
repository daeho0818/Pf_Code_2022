using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : BaseObj
{
    [SerializeField] TMPro.TextMeshProUGUI hp_text;
    [SerializeField] UnityEngine.UI.Slider hp_slider;

    Vector2 mouse_position => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public bool player_dead { get; set; } = false;

    protected override void Start()
    {
        base.Start();
    }

    float current_time = 0;
    protected override void Update()
    {
        base.Update();

        if (!system.is_game_start) return;

        current_time += Time.deltaTime;

        if (current_time >= attack_range && !player_dead)
        {
            current_time = 0;
            Fire(transform.position, Vector2.one * 0.2f, Vector2.up, 5, attack_damage, Bullet.BulletType.Player, system);
        }

        if (Input.GetMouseButton(0) && !player_dead)
        {
            Vector2 mouse_pos = mouse_position;
            transform.position = mouse_pos;//Vector3.MoveTowards(transform.position, mouse_pos, 0.1f);
        }

        if (hp < 0) hp = 0;

        hp_text.text = $"HP : {hp} / {max_hp}";
        hp_slider.value = hp / max_hp;
    }

    protected override void ObjectDead()
    {
        base.ObjectDead();
    }

    public IEnumerator DeadAnimation()
    {
        move_speed = 0;
        transform.position = Vector2.zero;
        CameraManager.Instance.MoveCam(transform.position, 5);
        CameraManager.Instance.ZoomCam(4, 5);

        yield return new WaitForSeconds(2f);

        for (int j = 1; j <= 3; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                system.AddParticle(transform.position + new Vector3(Random.Range(-2.0f * j, 2.0f * j), Random.Range(-2.0f * j, 2.0f * j)));
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1);

        for (int i = 0; i < 10; i++)
        {
            system.AddParticle(transform.position + new Vector3(Random.Range(-0.5f * i, 0.5f * i), Random.Range(-0.5f * i, 0.5f * i)));
        }

        yield return new WaitForSeconds(1);
        system.character = null;
        Destroy(gameObject);
    }
}
