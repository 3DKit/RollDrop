using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Unity UI Text için gerekli namespace

public class GameManager : MonoBehaviour
{
    public int targetFPS = 60;
    public Text fpsText; // FPS değerini göstermek için Unity UI Text nesnesi

    void Start()
    {
        // Oyun başladığında hedef FPS'yi ayarla
        QualitySettings.vSyncCount = 0;  // Vertical sync'i kapat
        Application.targetFrameRate = targetFPS;

        StartCoroutine(UpdateFPS());
    }

    IEnumerator UpdateFPS()
    {
        while (true)
        {
            // FPS değerini hesapla
            float fps = 1.0f / Time.deltaTime;

            // Unity UI Text nesnesine FPS değerini yaz
            if (fpsText != null)
            {
                fpsText.text = "FPS: " + Mathf.Round(fps);
            }

            yield return new WaitForSeconds(1.0f); // Her saniyede bir güncelle
        }
    }
}
