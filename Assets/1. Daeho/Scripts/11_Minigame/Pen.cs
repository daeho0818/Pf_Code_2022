using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    Rigidbody2D rigid { get; set; }

    public bool is_falled { get; set; } = false;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
    }

    public void Touch(Vector3 touch_position)
    {
        if (is_falled) return;

        float distance = Mathf.Abs(touch_position.x) + Mathf.Abs(transform.position.x);
        float direction_x = touch_position.x > transform.position.x ? 1 : -1;
        Vector2 power = Vector2.right * (4 - distance) * -direction_x;

        rigid.AddForce(power, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Out Range"))
        {
            is_falled = true;
            rigid.velocity = Vector2.zero;
            StartCoroutine(FallAnimation());
        }
    }

    IEnumerator FallAnimation()
    {
        WaitForSeconds second = new WaitForSeconds(0.005f);

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, Vector2.one * 0.2f, 0.1f);
            if (Vector2.Distance(transform.localScale, Vector2.one * 0.2f) <= 0.01f)
            {
                transform.localScale = Vector2.one * 0.2f;
                break;
            }
            yield return second;
        }
    }
}
