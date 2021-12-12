using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHelper : MonoBehaviour
{
    Dictionary<string, Coroutine> moving = new Dictionary<string, Coroutine>();
    Dictionary<string, Coroutine> scaling = new Dictionary<string, Coroutine>();
    Dictionary<string, Coroutine> fading = new Dictionary<string, Coroutine>();

    /// <summary>
    /// UI를 이동시키는 함수 (local position)
    /// </summary>
    /// <param name="r_transform">이동시킬 UI의 RectTransform</param>
    /// <param name="target_position">UI를 이동시킬 목표 위치</param>
    /// <param name="move_speed">UI를 이동시킬 때 이동속도</param>
    public void Moving(RectTransform r_transform, 
        Vector2 target_position, float move_speed, bool destroy = false)
    {
        string objName = r_transform.name;
        if (moving.ContainsKey(objName))
        {
            if (moving[objName] != null) StopCoroutine(moving[objName]);
            moving[objName] = StartCoroutine(
                _Moving(r_transform, target_position, move_speed, destroy));
        }
        else
            moving.Add(objName, StartCoroutine(
                _Moving(r_transform, target_position, move_speed, destroy)));
    }
    IEnumerator _Moving(RectTransform r_transform, 
        Vector2 target_position, float move_speed, bool destroy)
    {
        WaitForSeconds second = new WaitForSeconds(0.01f);

        while (true)
        {
            if (Vector2.Distance(r_transform.localPosition, 
                target_position) <= 0.01f)
            {
                r_transform.localPosition = target_position;
                if (destroy) Destroy(r_transform.gameObject);
                break;
            }

            r_transform.localPosition = 
                Vector2.Lerp(r_transform.localPosition, 
                target_position, Time.deltaTime * move_speed);

            yield return second;
        }
    }

    /// <summary>
    /// UI의 크기를 변경해주는 함수
    /// </summary>
    /// <param name="r_transform">크기를 변경시킬 UI의 RectTransform</param>
    /// <param name="target_scale">해당 UI의 변경시킬 크기</param>
    /// <param name="scaling_speed">크기를 변경시킬 때 변경속도</param>
    public void Scaling(RectTransform r_transform, Vector2 target_scale, float scaling_speed)
    {
        string objName = r_transform.name;
        if (scaling.ContainsKey(objName))
        {
            if (scaling[objName] != null) 
                StopCoroutine(scaling[objName]);

            scaling[objName] = 
                StartCoroutine(
                    _Scaling(r_transform, target_scale, scaling_speed));
        }
        else
            scaling.Add(objName, 
                StartCoroutine(
                    _Scaling(r_transform, target_scale, scaling_speed)));
    }
    IEnumerator _Scaling(RectTransform r_transform, Vector2 target_scale, float scaling_speed)
    {
        WaitForSeconds second = new WaitForSeconds(0.01f);

        while (true)
        {
            if (Vector2.Distance(r_transform.localScale, target_scale)
                < 0.001f)
            {
                r_transform.localScale = target_scale;
                break;
            }

            r_transform.localScale = 
                Vector2.Lerp(r_transform.localScale, 
                target_scale, 
                Time.deltaTime * scaling_speed);

            yield return second;
        }
    }

    /// <summary>
    /// UI를 Fading해주는 함수, image나 text 중 하나만 할당 (나머지는 null 할당)
    /// </summary>
    /// <param name="image">Fading할 이미지</param>
    /// <param name="text">Fading할 텍스트</param>
    /// <param name="target_alpha">목표 Fading alpha 값</param>
    /// <param name="fading_speed">Fading시킬 속도</param>
    public void Fading(Image image, TextMeshProUGUI text, 
        float target_alpha, float fading_speed, bool destroy = false)
    {
        Graphic graphic;
        if (image)
            graphic = image;
        else if (text)
            graphic = text;
        else
        {
            Debug.LogError($"{nameof(image)} is null, " +
                $"{nameof(text)} is null too");
            return;
        }

        string objName = graphic.name;
        if (fading.ContainsKey(objName))
        {
            if (fading[objName] != null) 
                StopCoroutine(fading[objName]);

            fading[objName] = 
                StartCoroutine(_Fading(graphic, target_alpha, fading_speed, destroy));
        }
        else
            fading.Add(objName, 
                StartCoroutine(
                    _Fading(graphic, target_alpha, fading_speed, destroy)));
    }
    IEnumerator _Fading(Graphic graphic, float target_alpha, 
        float fading_speed, bool destroy)
    {
        WaitForSeconds second = new WaitForSeconds(0.01f);

        while (true)
        {
            if (Mathf.Abs(target_alpha - graphic.color.a) < 0.001f)
            {
                graphic.color = new Color(graphic.color.r, 
                    graphic.color.g, graphic.color.b, target_alpha);
                if (destroy) Destroy(graphic.gameObject);
                break;
            }

            graphic.color = 
                Color.Lerp(graphic.color, 
                new Color(graphic.color.r,  graphic.color.g, 
                graphic.color.b,  target_alpha),
                Time.deltaTime * fading_speed);

            yield return second;
        }
    }
}
