using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 3f; // Silinme s�resi

    void Start()
    {
        Invoke("FadeOutAndDestroy", 2.0f); // 2 saniye sonra FadeOutAndDestroy fonksiyonunu �a��r
    }

    void FadeOutAndDestroy()
    {
        StartCoroutine(FadeOut()); // Yava��a silinme i�lemini ba�lat
    }

    IEnumerator FadeOut()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;

        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            renderer.material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Silindikten sonra objeyi tamamen yok et
        Destroy(gameObject);
    }
}
