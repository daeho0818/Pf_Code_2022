using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tteok : MonoBehaviour
{
    public January_System system { get; set; }

    [SerializeField] Rigidbody2D rigid;
    void Start()
    {
        rigid.gravityScale = 1;
    }

    void Update()
    {
        if (system.game_end)
            Destroy(gameObject);
    }

    public void SpawnAnimation()
    {
        Vector2 dir = new Vector2(Random.Range(-0.5f, 0.5f), Random.value);
        float power = Random.Range(10, 15);
        rigid.AddForce(dir * power, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Out Range"))
        {
            if (system.is_game_start)
            {
                system.score_helper.ScoreOperation(-system.operating_score);
            }
            Destroy(gameObject);
        }
    }
}
