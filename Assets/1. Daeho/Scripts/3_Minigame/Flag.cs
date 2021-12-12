using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Flag : MonoBehaviour, IPointerClickHandler
{
    public March_System system { get; set; }

    RectTransform rectTr { get; set; }
    Image image { get; set; }

    Vector2 target_scale;

    private int get_count = 0;
    private bool is_growing = true;

    void Start()
    {
        rectTr = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        InvokeRepeating(nameof(SizeUp), 0, 2);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        get_count++;
        target_scale = transform.localScale - Vector3.one * 0.4f;

        if (get_count >= system.flag_get_count)
        {
            is_growing = false;

            system.score_helper.ScoreOperation(50);

            system.ui_helper.Scaling(rectTr,
                Vector2.zero, 15);

            system.ui_helper.Moving(rectTr,
                new Vector2(-458, -846), 15, true);

            Destroy(gameObject, 3);
        }
        else SizeAnimation();
    }

    void SizeUp()
    {
        if (get_count <= 0 || !is_growing) return;

        get_count--;
        target_scale = transform.localScale + Vector3.one * 0.4f;

        system.ui_helper.Scaling(rectTr,
            target_scale, 10);

        system.StartCoroutine(() =>
        {
            system.ui_helper.Scaling(rectTr,
                target_scale - Vector2.one * 0.2f, 15);
        }, 0.25f);
    }

    void SizeAnimation()
    {
        system.ui_helper.Scaling(rectTr,
            target_scale, 10);

        system.StartCoroutine(() =>
        {
            system.ui_helper.Scaling(rectTr,
                target_scale + Vector2.one * 0.2f, 15);
        }, 0.25f);
    }
}
