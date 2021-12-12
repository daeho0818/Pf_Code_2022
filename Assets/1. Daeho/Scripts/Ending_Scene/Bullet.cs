using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Player,
        Enemy
    }
    public BulletType bullet_type;

    public float move_speed { get; set; }
    public float attack_damage { get; set; }
    public Vector2 move_direction { get; set; }
    public Ending_System system { get; set; }
    public CircleCollider2D circleC { get; set; }
    void Awake()
    {
    }

    void Start()
    {
        circleC = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (!system.is_game_start) return;

        transform.Translate(move_direction * Time.deltaTime * move_speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bullet_type == BulletType.Enemy && collision.CompareTag("Player"))
        {
            Character character = collision.GetComponent<Character>();
            character.hp -= attack_damage;
            character.ChangeColor();
            system.AddParticle(transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)));

            gameObject.SetActive(false);
            circleC.enabled = false;
            system.bullet_pool.Enqueue(this);
        }
        else if (bullet_type == BulletType.Player && collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.hp -= attack_damage;
            enemy.ChangeColor();
            system.game_score += 100;
            system.AddParticle(transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)));

            gameObject.SetActive(false);
            circleC.enabled = false;
            system.bullet_pool.Enqueue(this);
        }
        else if (bullet_type == BulletType.Player && collision.CompareTag("Boss"))
        {
            Boss boss = collision.GetComponent<Boss>();
            if (boss.is_spawned)
            {
                boss.hp -= attack_damage;
                boss.ChangeColor();
                system.game_score += 100;
                system.AddParticle(transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)));

                gameObject.SetActive(false);
                circleC.enabled = false;
                system.bullet_pool.Enqueue(this);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Out Range"))
        {
            gameObject.SetActive(false);
            circleC.enabled = false;
            system.bullet_pool.Enqueue(this);
        }
    }
}
