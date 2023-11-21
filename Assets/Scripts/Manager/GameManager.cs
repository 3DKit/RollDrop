using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private GameObject _playerPrefab;  // Reference to the pre-defined prefab of the player object
    [SerializeField] private GameObject _decal;
    [SerializeField] private Image _progressBar;  // UI element to display the progress bar
    [SerializeField] private Text _currentLevelText, _nextLevelText;  // Texts indicating the current and next levels
    [SerializeField] private GameObject[] _levelPrefabs;  // Level prefabs array
    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _deadSound;
    [SerializeField] private AudioClip _crashSound;

    private AudioSource _audioSource => GetComponent<AudioSource>();
    private GameObject _player;  // Reference to the player object
    private PlatformController _platformController;  // PlatformController component
    private int _currentLevel;  // Index of the current level
    public static GameManager Instance => _instance;  // Global access point for GameManager
    public GameObject Player => _player;  // Global access point for the player object
    private void RestartLevel()
    {
        // Reload the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void Awake()
    {
        if (_instance == null)
        {
            // If the GameManager instance hasn't been created yet, assign this instance
            _instance = this;
        }
        else
        {
            // If another GameManager instance has already been created, destroy this instance
            Destroy(gameObject);
            return;
        }

        // Level check and error fixing
        _currentLevel = PlayerPrefs.GetInt("Level", 1);
        if (_currentLevel <= 0 || _currentLevel > _levelPrefabs.Length)
        {
            // Set to default 1 if the level is 0 or out of range
            PlayerPrefs.SetInt("Level", 1);
            _currentLevel = 1;
            Log("Level set to 1 due to an invalid value.");
        }

        // Display level information
        _currentLevelText.text = _currentLevel.ToString();
        _nextLevelText.text = (_currentLevel + 1).ToString();

        // Create the player and other objects
        _player = Instantiate(_playerPrefab);
        _platformController = Instantiate(_levelPrefabs[_currentLevel - 1]).GetComponent<PlatformController>();
        _player.transform.position = new Vector3(0, (_platformController.PlatformCount * 3) + 3, -1.6f);
    }

    private void Start()
    {
        CheckProgress(0);
        Application.targetFrameRate = 120;
    }

    public void CheckProgress(float progress)
    {
        _progressBar.fillAmount = progress;
    }

    public void Splasher(Transform origin)
    {
        GameObject splash = Instantiate(_decal, new Vector3(origin.position.x,origin.position.y-0.22f,origin.position.z), Quaternion.Euler(90f, Random.Range(-360,360), 0f));
        splash.transform.parent = _platformController.transform;
    }

    public void Win()
    {
        PlayerPrefs.SetInt("Level", _currentLevel + 1);
        Log("Level completed. Moving to the next level.");
        _audioSource.clip = _winSound;
        _audioSource.Play();
        Invoke("RestartLevel", 1f);
    }
    public void Dead()
    {
        _audioSource.clip = _deadSound;
        _audioSource.Play();
        Invoke("RestartLevel",1f);
    }

    public void CrashSound()
    {
        _audioSource.clip = _crashSound;
        _audioSource.Play();
    }
    public void ResetLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void StartGame()
    {
        _platformController.StartGame();
    }

    public void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
}
