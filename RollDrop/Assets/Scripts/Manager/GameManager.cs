using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Unity UI Text için gerekli namespace

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private int _targetFPS = 60;

    [SerializeField]
    private Image _progressBar;

    [SerializeField]
    private Text _currentLevelText, _nextLevelText;

    private GameObject _player;

    public static GameManager Instance()
    {
        return GameManager._instance;
    }

    public GameObject Player()
    {
        return this._player;
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Application.LoadLevel(1);
        }

        // Oyun başladığında hedef FPS'yi ayarla
        QualitySettings.vSyncCount = 0;  // Vertical sync'i kapat
        Application.targetFrameRate = _targetFPS;

        _currentLevelText.text = SceneManager.GetActiveScene().buildIndex.ToString();
        _nextLevelText.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();
    }

    public void CheckProgress(float progress)
    {
        _progressBar.fillAmount = progress;
    }

    public void Win()
    {
        PlayerPrefs.SetInt("Level", SceneManager.GetActiveScene().buildIndex+1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
