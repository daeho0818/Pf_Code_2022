using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerHelper : MonoBehaviour
{
    public GameSystem system { get; set; }
    public bool text_spacing_animation { get; set; } = false;

    [SerializeField] TextMeshProUGUI timer_text;

    private bool is_start = false;
    private bool time_attack = false;
    private bool timer_end = false;
    private int timer;
    void Start()
    {

    }

    float sin_value = 0;
    void Update()
    {
        if (!system.is_game_start || system.game_end) return;

        if (text_spacing_animation)
        {
            sin_value += 30 * Time.deltaTime;
            timer_text.wordSpacing = Mathf.Sin(sin_value * Mathf.Deg2Rad) * 40;
        }
    }
    void CountDown()
    {
        if (timer <= 0 || timer_end)
        {
            CancelInvoke(nameof(CountDown));
            timer_end = true;
            return;
        }
        if (!system.game_end)
        {
            timer--;
            timer_text.text = $"{timer / 60} : {timer % 60}";
        }
    }
    void CountUp()
    {
        if (timer <= -1 || timer_end)
        {
            CancelInvoke(nameof(CountUp));
            timer_end = true;
            return;
        }

        if (!system.game_end)
        {
            timer++;
            timer_text.text = $"{timer / 60} : {timer % 60}";
        }
    }

    public int GetTime()
    {
        return timer;
    }
    public void SetTimer(int time, bool timeAttack = false)
    {
        time_attack = timeAttack;

        if (time_attack)
            return;
        else
            StartCoroutine(TimerAnimation(time));
    }
    public void TimerStart()
    {
        is_start = true;
        system.is_game_start = true;

        if (time_attack)
            InvokeRepeating(nameof(CountUp), 0, 1);
        else
            InvokeRepeating(nameof(CountDown), 0, 1);
    }

    public bool TimerEnd() => timer_end;
    public void ShutTimer() => timer_end = true;

    IEnumerator TimerAnimation(int time)
    {
        WaitForSeconds second = new WaitForSeconds(0.05f);

        for (int i = 0; i <= time; i++)
        {
            timer = i;
            timer_text.text = $"{timer / 60} : {timer % 60}";
            yield return second;
        }
    }
}
