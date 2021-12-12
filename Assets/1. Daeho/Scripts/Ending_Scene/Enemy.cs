using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseObj
{
    [SerializeField] bool dir_bullet;
    public float spawn_range;

    float current_time = 0;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!system.is_game_start) return;

        transform.Translate(move_direction * Time.deltaTime * move_speed);

        current_time += Time.deltaTime;

        if (current_time >= attack_range)
        {
            Vector2 dir;
            if (dir_bullet && system.character)
                dir = (system.character.transform.position - transform.position).normalized;
            else dir = Vector2.down;
            Fire(transform.position, Vector2.one * 0.2f, dir, 3, attack_damage, Bullet.BulletType.Enemy, system);
            current_time = 0;
        }
    }

    protected override void ObjectDead()
    {
        base.ObjectDead();
        Destroy(gameObject);
    }
}
