using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePiece : MonoBehaviour
{
    public int Piecenumber;
    public GameObject plane;
    public bool isMove = true;
    public PuzzlePlane[] Plane = new PuzzlePlane[16];
    public float Distance = 0;
    public float MinDistance = 10000000;
    public PuzzlePlane Min_distance_Plane;
    public int wincount = 0;

    void OnMouseDrag()
    {
        if (isMove == true && FindObjectOfType<GameSystem>().is_game_start)
        {
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Distance);
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = objPosition;
        }
    }
    private void Update()
    {
        if (isMove == true)
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            if (pos.x < 0f) pos.x = 0f;
            if (pos.x > 1f) pos.x = 1f;
            if (pos.y < 0f) pos.y = 0f;
            if (pos.y > 1f) pos.y = 1f;
            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
        if (Input.GetMouseButtonUp(0))
        {
            MinDistance = 10000;
            foreach (var Plane in Plane)
            {
                if (Vector2.Distance(transform.position, Plane.transform.position) < MinDistance)
                {
                    MinDistance = Vector2.Distance(transform.position, Plane.transform.position);
                    Min_distance_Plane = Plane;
                }
            }
            if (Vector2.Distance(transform.position, plane.transform.position) <= Distance && Piecenumber == Min_distance_Plane.Planenumber)
            {
                this.gameObject.transform.position = plane.transform.position;
                isMove = false;
                //this.gameObject.GetComponent<Renderer>().material.color = Color.black;
                if (wincount == 0)
                {
                    WinCount.Instance.winCount++;
                    wincount++;
                }
            }
        }
    }
}
