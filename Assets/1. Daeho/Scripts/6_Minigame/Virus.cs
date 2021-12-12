using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Virus : MonoBehaviour, IPointerClickHandler
{
    public June_System system { get; set; }

    RectTransform rectTr { get; set; }
    Image image { get; set; }

    Vector2 target_scale;

    private int death_count = 0;

    void Start()
    {
        rectTr = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        InvokeRepeating(nameof(SizeDown), 0, 2);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        death_count++;
        target_scale = transform.localScale + Vector3.one * 0.4f;

        if (death_count >= system.virus_death_count)
        {
            system.score_helper.ScoreOperation(50);
            system.ui_helper.Fading(image,
                null, 0, 10, true);
            image.raycastTarget = false;
            Destroy(gameObject, 5);
        }
        else SizeAnimation();
    }

    void SizeDown()
    {
        if (death_count <= 0 || 
            death_count >= system.virus_death_count - 1) return;

        death_count--;
        target_scale = transform.localScale - Vector3.one * 0.4f;

        system.ui_helper.Scaling(rectTr,
            target_scale, 10);

        system.StartCoroutine(() =>
        {
            system.ui_helper.Scaling(rectTr,
                target_scale + Vector2.one * 0.2f, 15);
        }, 0.25f);
    }

    void SizeAnimation()
    {
        system.ui_helper.Scaling(rectTr,
            target_scale, 10);

        system.StartCoroutine(() =>
        {
            system.ui_helper.Scaling(rectTr,
                target_scale - Vector2.one * 0.2f, 15);
        }, 0.25f);
    }
}
