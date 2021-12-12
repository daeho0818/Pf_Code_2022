using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public December_System system { get; set; }
    public float move_speed;
    void Start()
    {
        StartCoroutine(Moving());
    }

    void Update()
    {
        if (system.game_end)
            Destroy(gameObject);
    }

    IEnumerator Moving()
    {
        while (true)
        {
            transform.Translate(Vector2.down * Time.deltaTime * move_speed);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Out Range"))
        {
            Destroy(gameObject);
            system.score_helper.ScoreOperation(50);
            system.miss_obstacles++;
        }
        else if(collision.CompareTag("Player"))
        {
            Destroy(gameObject);
            system.DestroySanta();
        }
    }
}
