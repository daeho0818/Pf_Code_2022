using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float MaxDistance = 25f;
    Vector3 MousePosition;
    Camera Camera;

    // Start is called before the first frame update
    private void Start()
    {
        Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MousePosition = Input.mousePosition;
            MousePosition = Camera.ScreenToWorldPoint(MousePosition);

            RaycastHit2D hit = Physics2D.Raycast(MousePosition, transform.forward, MaxDistance);
            Debug.DrawRay(MousePosition, transform.forward * 10, Color.red, 0.3f);
            if(hit)
            {
                hit.transform.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }
}
