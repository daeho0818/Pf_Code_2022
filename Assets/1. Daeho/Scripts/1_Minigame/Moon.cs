using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public January_System system { get; set; }
    void Start()
    {
    }

    float sin_value = 0;
    void Update()
    {
        sin_value += 50 * Time.deltaTime;
        transform.position = new Vector3(0, 7 + Mathf.Sin(sin_value * Mathf.Deg2Rad));
    }

    public IEnumerator TteokAnimation()
    {
        Tteok tteok_obj;

        for (int i = 0; i < 25; i++)
        {
            tteok_obj = Instantiate(system.tteok, transform.position, Quaternion.identity);
            tteok_obj.system = system;
            tteok_obj.transform.localScale = Vector2.one / 2;
            tteok_obj.SpawnAnimation();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
