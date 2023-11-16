using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private Vector2 touchStartPos;
    public float rotationSpeed = 0.5f;
    private bool isMobilePlatform;

    void Start()
    {
        // Uygulama mobil platformdaysa dokunmatik kontrolü etkinleştir
        isMobilePlatform = Application.isMobilePlatform;
    }

    void Update()
    {
        // Dokunmatik kontrolü (mobil)
        if (isMobilePlatform && Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    break;

                case TouchPhase.Moved:
                    // Dokunmatik sürükleme hareketi
                    Vector2 deltaPos = touch.position - touchStartPos;

                    // Döndürme miktarı
                    float rotationAmount = -deltaPos.x * rotationSpeed;

                    // "Level" objesini döndür
                    transform.Rotate(Vector3.up, rotationAmount, Space.World);

                    // Başlangıç pozisyonunu güncelle
                    touchStartPos = touch.position;
                    break;
            }
        }

        // Fare kontrolü (diğer platformlar)
        if (!isMobilePlatform && Input.GetMouseButton(0))
        {
            // Fare sürükleme hareketi
            float rotationAmount = Input.GetAxis("Mouse X") * rotationSpeed * 10;

            // "Level" objesini döndür
            transform.Rotate(Vector3.up, rotationAmount, Space.World);
        }
    }
}
