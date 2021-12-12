using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.SceneManagement.SceneManager;
using TMPro;

public class Result_Window : MonoBehaviour
{
    // On / Off �� â
    public RectTransform result_window;
    // "���� ����.." or "���� Ŭ����"�� �� ����
    public TextMeshProUGUI result_text;
    // �߾ӿ� �� ���� ���� ������ ���� Text
    public TextMeshProUGUI information_text;
    // fade ó���� ��� �ؽ�Ʈ
    public TextMeshProUGUI[] fading_texts;
    // fade ó���� ��� �̹���
    public UnityEngine.UI.Image[] fading_images;

    public void AcceptButton()
    {
        if (FindObjectOfType<GameSystem>().game_clear)
            LoadScene("Main");
        else
            LoadScene(GetActiveScene().name);
    }
}
