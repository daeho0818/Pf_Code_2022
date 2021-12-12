using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bead : MonoBehaviour
{
    public October_System system { get; set; }
    public enum BeadType
    {
        Player,
        None,
    }
    public BeadType bead_type;
    Vector2 basic_position;
    Vector2 basic_scale;

    public Rigidbody2D rigid { get; set; }
    new SpriteRenderer renderer;

    public bool moving { get; set; } = false;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        basic_position = transform.position;
        basic_scale = transform.localScale;

        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 1);

        transform.localScale = Vector2.zero;

        if (system.is_game_start)
            StartCoroutine(StartAnimation());
    }

    void Update()
    {
        if (system.game_end)
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Out Range"))
        {
            if (bead_type == BeadType.None)
            {
                Destroy(gameObject);
                system.score_helper.ScoreOperation(50);
            }
        }
    }

    /// <summary>
    /// �÷��̾� ������ ���, ������ �߻��ϴ� �Լ�
    /// </summary>
    /// <param name="shoot_direction">�߻��ϴ� ����</param>
    /// <param name="shoot_power">�߻��ϴ� ��</param>
    public void ShootThisBead(Vector2 shoot_direction, float shoot_power)
    {
        moving = true;
        rigid.AddForce(shoot_direction * shoot_power, ForceMode2D.Impulse);

        system.StartCoroutine(ResetPosition(), 1);
    }

    /// <summary>
    /// �÷��̾� ���� ������ �� Ŀ���鼭 �����ϴ� �ִϸ��̼�
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartAnimation()
    {
        WaitForSeconds second = new WaitForSeconds(0.01f);

        while (true)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, basic_scale, 0.5f);
            if (Vector2.Distance(transform.localScale, basic_scale) <= 0.01f)
            {
                transform.localScale = basic_scale;
                break;
            }
            yield return second;
        }
    }

    /// <summary>
    /// �÷��̾� ������ ���, �߻� �� ���ڸ��� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetPosition()
    {
        if (system.game_end) yield break;

        rigid.velocity = Vector2.zero;
        Color color = renderer.color;

        WaitForSeconds second = new WaitForSeconds(0.01f);

        while (true)
        {
            renderer.color = Color.Lerp(renderer.color, color - new Color(0, 0, 0, 1), 0.5f);
            if (1 - renderer.color.a >= 0.99f)
            {
                renderer.color = color - new Color(0, 0, 0, 1);
                break;
            }
            yield return second;
        }

        Bead bead = Instantiate(this, basic_position, Quaternion.identity);
        bead.system = system;
        bead.transform.localScale = basic_scale;
        bead.GetComponent<SpriteRenderer>().sprite = system.beads_sprite[Random.Range(0, system.beads_sprite.Length)];

        Destroy(gameObject);
    }
}
