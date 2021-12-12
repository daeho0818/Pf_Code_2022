using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; } = null;

    private new Camera camera;

    Coroutine camera_move;
    Coroutine camera_shake;
    Coroutine camera_zoom;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        camera = Camera.main;
    }
    private void Update()
    {
    }

    /// <summary>
    /// 카메라를 이동시키는 함수
    /// </summary>
    /// <param name="position">이동시킬 위치</param>
    /// <param name="speed">카메라 이동 속도</param>
    /// 
    public void MoveCam(Vector3 position, float speed)
    {
        if (camera_move != null) StopCoroutine(camera_move);
        camera_move = StartCoroutine(_MoveCam(new Vector3(position.x, position.y, -10), speed));
    }
    IEnumerator _MoveCam(Vector3 position, float speed)
    {
        WaitForSeconds second = new WaitForSeconds(0.01f);
        while (true)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, position, Time.deltaTime * speed);
            if (Vector2.Distance(camera.transform.position, position) < 0.01f)
            {
                camera.transform.position = position;
                break;
            }
            yield return second;
        }
        camera_move = null;
    }

    Vector3 before_position;
    /// <summary>
    /// 카메라를 흔들림 효과 함수
    /// </summary>
    /// <param name="power">흔들림 강도</param>
    /// <param name="time">흔들리는 시간</param>
    public void ShakeCam(float power, float time)
    {
        if (camera_shake != null)
        {
            StopCoroutine(camera_shake);
            camera.transform.position = before_position;
        }
        camera_shake = StartCoroutine(_ShakeCam(power, time));
    }
    IEnumerator _ShakeCam(float power, float time)
    {
        WaitForSeconds second = new WaitForSeconds(0.01f);

        float random_x, random_y;
        float timer = Time.time;

        while (true)
        {
            if (Time.time - timer > time)
                break;
            else if (Time.time - timer > time / 2 && power > 0.0f)
                power -= 0.1f;

            random_x = Random.Range(-1.0f, 1.0f) * (power / 10);
            random_y = Random.Range(-1.0f, 1.0f) * (power / 10);

            before_position = camera.transform.position;
            camera.transform.position += new Vector3(random_x, random_y);
            yield return second;
            camera.transform.position = before_position;
        }
        camera_shake = null;
    }

    /// <summary>
    /// 카메라를 확대시키는 함수
    /// </summary>
    /// <param name="zoom_size">확대시킬 카메라 크기</param>
    /// <param name="zoom_speed">카메라 확대시키는 속도</param>
    public void ZoomCam(float zoom_size, float zoom_speed)
    {
        if (camera_zoom != null) StopCoroutine(camera_zoom);
        camera_zoom = StartCoroutine(_ZoomCam(zoom_size, zoom_speed));
    }
    IEnumerator _ZoomCam(float zoom_size, float zoom_speed)
    {
        while (true)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, zoom_size, Time.deltaTime * zoom_speed);
            if (Mathf.Abs(Mathf.Abs(camera.orthographicSize) - Mathf.Abs(zoom_size)) <= 0.01f)
            {
                camera.orthographicSize = zoom_size;
                break;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
