using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public abstract class GameSystem : MonoBehaviour
{
    public ScoreHelper score_helper;
    public UIHelper ui_helper;
    public TimerHelper timer_helper;

    [Space(20)]
    [SerializeField] TextMeshProUGUI countdown_text;

    [Space(20)]
    [SerializeField] Result_Window resultWindow;

    protected RectTransform result_window => resultWindow.result_window;
    protected TextMeshProUGUI result_text => resultWindow.result_text;
    protected TextMeshProUGUI information_text => resultWindow.information_text;
    protected TextMeshProUGUI[] fading_texts => resultWindow.fading_texts;
    protected UnityEngine.UI.Image[] fading_images => resultWindow.fading_images;

    [Space(20)]
    [SerializeField] RectTransform setting_window;
    [Space(20)]
    public int game_timer;

    protected string information_text_string;


    public delegate void Function();
    Function[] show_setting_window_animation_functions;
    Function[] hide_setting_window_animation_functions;

    public bool is_game_start { get; set; }
    public bool game_clear { get; set; }
    public bool game_over { get; set; }
    public bool game_end { get => game_clear || game_over; }

    protected virtual void Awake()
    {
    }
    protected virtual void Start()
    {
        show_setting_window_animation_functions = new Function[6]
        {
            () => { setting_window.localScale = Vector2.zero; },
            () => { setting_window.gameObject.SetActive(true); },
            () => { ui_helper.Scaling(setting_window, Vector2.one * 1.2f, 20f); },
            () => { ui_helper.Scaling(setting_window, Vector2.one * 0.8f, 20f); },
            () => { ui_helper.Scaling(setting_window, Vector2.one, 20f); },
            () => { Time.timeScale = 0; }
        };

        hide_setting_window_animation_functions = new Function[2]
        {
            () => { ui_helper.Scaling(setting_window, Vector2.zero, 20); },
            () => { setting_window.gameObject.SetActive(false); },
        };
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = (Time.timeScale == 1 ? 3 : 1);
        }

        if (Input.GetKeyDown(KeyCode.C))
            무조건클리어();
        if (Input.GetKeyDown(KeyCode.H))
            PlayerPrefs.SetInt("Lock Count", 0);

        if (!is_game_start) return;

        if (timer_helper.TimerEnd() && !game_end)
        {
            GameEnd();
        }
    }

    void 무조건클리어()
    {
        game_clear = true;
        game_over = false;

        LockDown.Instance.LockCount++;
        PlayerPrefs.SetInt("Lock Count", LockDown.Instance.LockCount);

        StartCoroutine(FinishAnimation());
    }

    protected abstract void GameEnd();
    protected void ShowUIs()
    {
        UnityEngine.UI.Image[] images = FindObjectsOfType<UnityEngine.UI.Image>();
        TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>();

        foreach (var image in images)
            ui_helper.Fading(image, null, 1, 3);

        foreach (var text in texts)
            ui_helper.Fading(null, text, 1, 3);
    }

    /// <summary>
    /// 미니게임 시작 시 3, 2, 1, 게임 시작 애니메이션
    /// </summary>
    /// <returns></returns>
    public IEnumerator StartAnimation()
    {
        countdown_text.gameObject.SetActive(true);

        Function function = () =>
        {
            countdown_text.rectTransform.localScale = Vector2.one * 5;
            ui_helper.Scaling(countdown_text.rectTransform, Vector2.one, 7);
        };

        countdown_text.text = "3";
        function();
        yield return new WaitForSeconds(1f);

        countdown_text.text = "2";
        function();

        yield return new WaitForSeconds(1f);

        countdown_text.text = "1";
        function();

        yield return new WaitForSeconds(1f);

        countdown_text.text = "게임 시작!";
        function();

        yield return new WaitForSeconds(2f);

        countdown_text.gameObject.SetActive(false);
    }
    public IEnumerator FinishAnimation(float wait_time = 0)
    {
        yield return new WaitForSeconds(wait_time);

        result_window.parent.gameObject.SetActive(true);

        ui_helper.Moving(result_window, Vector2.zero, 7);

        StartCoroutine(() => { ui_helper.Moving(result_text.rectTransform, new Vector2(0f, 360f), 7); }, 0.75f);

        foreach (var fading_text in fading_texts)
        {
            StartCoroutine(() => { ui_helper.Fading(null, fading_text, 1, 3); }, 0.5f);
        }
        foreach (var fading_image in fading_images)
        {
            StartCoroutine(() => { ui_helper.Fading(fading_image, null, 1, 3); }, 0.5f);
        }

        yield return new WaitForSeconds(2f);

        for (int i = 0; i < information_text_string.Length; i++)
        {
            information_text.text += information_text_string[i];
            yield return new WaitForSeconds(0.05f);
        }
    }

    /// <summary>
    /// 설정 창 연출과 함께 띄우는 함수
    /// </summary>
    public void ShowSettingWindow()
    {
        StartCoroutines(show_setting_window_animation_functions, new float[6] { 0, 0, 0.3f, 0.3f, 0.3f, 1f });
    }
    /// <summary>
    /// 설정 창 연출과 함께 가리는 함수
    /// </summary>
    public void HideSettingWindow()
    {
        if (Time.timeScale != 0) return;
        Time.timeScale = 1;
        StartCoroutines(hide_setting_window_animation_functions, new float[2] { 0.1f, 0.5f });
    }
    /// <summary>
    /// 일정 시간 뒤 StartCoroutine에 enumerator를 넘겨 실행해주는 함수
    /// </summary>
    /// <param name="enumerator">실행할 enumerator</param>
    /// <param name="time">기다릴 시간 (단위 : 초)</param>
    public void StartCoroutine(IEnumerator enumerator, float time)
    {
        StartCoroutine(_StartCoroutine(enumerator, time));
    }
    IEnumerator _StartCoroutine(IEnumerator enumerator, float time)
    {
        yield return new WaitForSeconds(time);
        yield return StartCoroutine(enumerator);
        Debug.Log($"operated {nameof(enumerator)}.");
        yield break;
    }

    /// <summary>
    /// 일정 시간 뒤 void 형식과 매개변수가 없는 함수를 실행해주는 함수
    /// </summary>
    /// <param name="function">실행할 함수 (void, 매개변수 없음)</param>
    /// <param name="time">기다릴 시간 (단위 : 초)</param>
    public void StartCoroutine(Function function, float time)
    {
        StartCoroutine(_StartCoroutine(function, time));
    }
    IEnumerator _StartCoroutine(Function function, float time)
    {
        yield return new WaitForSeconds(time);
        function();
        Debug.Log($"operated {nameof(function)}.");
        yield break;
    }

    public void StartCoroutines(Function[] functions, float[] times)
    {
        StartCoroutine(_StartCoroutines(functions, times));
    }
    public void StartCoroutines(IEnumerator[] enumerators, float[] times)
    {
        StartCoroutine(_StartCoroutines(enumerators, times));
    }
    IEnumerator _StartCoroutines(Function[] functions, float[] times)
    {
        for (int i = 0; i < functions.Length; i++)
        {
            yield return _StartCoroutine(functions[i], times[i]);
        }
    }
    IEnumerator _StartCoroutines(IEnumerator[] enumerators, float[] times)
    {
        for (int i = 0; i < enumerators.Length; i++)
        {
            yield return _StartCoroutine(enumerators[i], times[i]);
        }
    }
}
