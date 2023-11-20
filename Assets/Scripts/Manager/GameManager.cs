using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private GameObject _playerPrefab;  // Reference to the pre-defined prefab of the player object
    [SerializeField] private Image _progressBar;  // UI element to display the progress bar
    [SerializeField] private Text _currentLevelText, _nextLevelText;  // Texts indicating the current and next levels
    [SerializeField] private GameObject[] _levelPrefabs;  // Level prefabs array

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
        // Reset the progress bar to zero at the start
        CheckProgress(0);
    }

    public void CheckProgress(float progress)
    {
        // Update the progress bar
        _progressBar.fillAmount = progress;
    }

    public void Win()
    {
        // Win condition
        PlayerPrefs.SetInt("Level", _currentLevel + 1);
        Log("Level completed. Moving to the next level.");

        // Call the RestartLevel function after 0.5 seconds
        Invoke("RestartLevel", 0.5f);
    }

    public void StartGame()
    {
        // Call the StartGame function in PlatformController
        _platformController.StartGame();
    }

    // Centralized method for Debug.Log usage
    public void Log(string message)
    {
#if UNITY_EDITOR
        Debug.Log(message);
#endif
    }
}
