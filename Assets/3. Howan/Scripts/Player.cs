using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player Instance { get; set; }
    public float speed; //����Ƽ���� �ӵ��� �ٲ� �� �ְ� ����
    Rigidbody2D rb; //�浹ó�� ǥ��
    public bool IsMove = false; //���� �����⸦ ���� ���������� �Ǻ��ϴ� ��
    public static int MoveCount = 1;
    public GameObject Gameover;
    public GameObject Gameclear;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    bool temp = false;
    void Update()
    {
        IsMove = false;
        if (Input.GetMouseButton(0) && FindObjectOfType<GameSystem>().is_game_start)
        {
            Move();
        }
        if (MoveCount == 2 && IsMove == true&& !temp)
        {
            temp = true;
            //GameOver();
            FindObjectOfType<GameSystem>().StartCoroutine(FindObjectOfType<GameSystem>().timer_helper.ShutTimer, 0.5f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            FindObjectOfType<GameSystem>().timer_helper.ShutTimer();
        }
    }
    public void Move()
    {
        IsMove = true;
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}