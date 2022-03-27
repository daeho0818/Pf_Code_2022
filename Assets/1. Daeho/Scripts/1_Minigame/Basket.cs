using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public January_System system { get; set; }
    public float move_speed;

    Coroutine size_animation;

    Vector2 basic_scale;

    bool is_click = false;
    int direction_x;

    void Start()
    {
        basic_scale = transform.localScale;
    }

    void Update()
    {
        if (is_click)
        {
            if (transform.position.x > 2.3f)
                transform.position = new Vector2(2.3f, transform.position.y);
            else if (transform.position.x < -2.3f)
                transform.position = new Vector2(-2.3f, transform.position.y);
        }
        else
        {
            direction_x = ((int)Input.GetAxisRaw("Horizontal"));
            transform.Translate((Vector2.right * direction_x) * Time.deltaTime * move_speed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tteok"))
        {
            if (system.is_game_start)
            {
                system.score_helper.ScoreOperation(system.operating_score);

                if (size_animation != null)
                {
                    transform.localScale = basic_scale;
                    StopCoroutine(size_animation);
                }
                size_animation = StartCoroutine(SizeAnimation());
            }
            Destroy(collision.gameObject);
        }
    }

    public void MoveBasket(int dir_x)
    {
        if (!system.is_game_start) return;

        direction_x = dir_x;

        is_click = dir_x != 0;
    }

    IEnumerator SizeAnimation()
    {
        Vector2[] target_size = { basic_scale * 1.2f, basic_scale * 0.8f, basic_scale };

        for (int i = 0; i < 3; i++)
        {
            while (true)
            {
                transform.localScale = Vector2.Lerp(transform.localScale, target_size[i], 0.5f);
                if (Vector2.Distance(transform.localScale, target_size[i]) <= 0.01f) break;
                yield return new WaitForSeconds(0.01f);
            }
        }
        size_animation = null;
    }
}
