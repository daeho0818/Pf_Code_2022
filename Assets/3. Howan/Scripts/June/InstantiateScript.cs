using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateScript : MonoBehaviour
{
    int MouseClickCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
            if (hit.collider != null && hit.collider.transform == this.transform)
            {
                MouseClickCount++;
            }
        }
        if (MouseClickCount >= 5)
        {
            PlayerScript.GameScore6 += 50;
            Debug.Log(PlayerScript.GameScore6);
            if (PlayerScript.GameScore6 >= 1000)
            {
                Debug.Log("Game6Clear");
            }
            Destroy(gameObject);
        }
    }
}
