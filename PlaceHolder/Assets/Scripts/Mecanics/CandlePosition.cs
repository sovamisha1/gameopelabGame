using UnityEngine;

public class CandlePositioning : MonoBehaviour
{
    public GameObject candle;
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        if (candle == null)
            candle = this.gameObject;
        if (candle != null && mainCamera != null)
        {
            candle.transform.parent = mainCamera.transform;
            PositionCandle();
        }
    }

    void Update()
    {
        // Обновляем позицию свечи при каждом кадре, на случай если камера движется или меняется её размер
        PositionCandle();
    }

    void PositionCandle()
    {
        // Устанавливаем положение свечи в правом нижнем углу камеры
        Vector3 screenPoint = new Vector3(Screen.width - 100, 100, mainCamera.nearClipPlane + 1);
        candle.transform.position = mainCamera.ScreenToWorldPoint(screenPoint);
        candle.transform.rotation = mainCamera.transform.rotation; // Может понадобиться подровнять угол свечи
    }
}
