using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Unity UI Text için gerekli namespace

public class GameManager : MonoBehaviour
{
    public GameObject player; // Player objesinin referansı
    public int targetFPS = 60;
    Image progressBar;
    Text currentLevelText, nextLevelText;

    void Awake()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Application.LoadLevel(1);
        }
        // Oyun başladığında hedef FPS'yi ayarla
        QualitySettings.vSyncCount = 0;  // Vertical sync'i kapat
        Application.targetFrameRate = targetFPS;
        player = GameObject.FindGameObjectWithTag("Player");
        progressBar = GameObject.Find("ProgressBar").GetComponent<Image>();
        currentLevelText = GameObject.Find("CurrentLevelText").GetComponent<Text>();
        nextLevelText = GameObject.Find("NextLevelText").GetComponent<Text>();
        currentLevelText.text = SceneManager.GetActiveScene().buildIndex.ToString();
        nextLevelText.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }

    public void CheckProgress(float progress)
    {
        progressBar.fillAmount = progress;
    }
}
