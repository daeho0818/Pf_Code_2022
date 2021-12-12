using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObj : MonoBehaviour
{
    public float hp;
    public float max_hp;
    public float move_speed;
    public float attack_range;
    public float attack_damage;
    public Vector2 move_direction { get; set; }
    public Ending_System system { get; set; }

    protected SpriteRenderer sr;
    protected Coroutine color_change;

    protected virtual void Awake()
    {

    }
    protected virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (hp <= 0)
        {
            ObjectDead();
        }
    }
    protected virtual void ObjectDead()
    {
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Out Range"))
        {
            ObjectDead();
        }
    }

    protected void Fire(Vector2 position, Vector2 localScale,
        Vector2 direction, float speed, float damage,
        Bullet.BulletType bullet_type, Ending_System system)
    {
        Bullet bullet;
        if (system.bullet_pool.Count > 0) bullet = system.bullet_pool.Dequeue();
        else bullet = Instantiate(system.bullet_prefab, system.bullet_parent);

        bullet.gameObject.SetActive(true);
        bullet.GetComponent<CircleCollider2D>().enabled = true;

        bullet.transform.position = position;
        bullet.transform.localScale = localScale;
        bullet.move_direction = direction;
        bullet.move_speed = speed;
        bullet.attack_damage = damage;
        bullet.bullet_type = bullet_type;
        bullet.system = system;
    }

    public void ChangeColor()
    {
        if (color_change != null) StopCoroutine(color_change);
        color_change = StartCoroutine(_ChangeColor());
    }
    IEnumerator _ChangeColor()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
}
