using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    [SerializeField] private Image _livesImg;

    [SerializeField] private Text _gameOverText;

    [SerializeField] private Text _restartGameText;

    [SerializeField] private Sprite[] _liveSprites;

    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private GameObject _player;

    void Start()
    {
        _scoreText.text = "Score: 0";
        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("UIManager: Unable to find Game_Manager");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null) {
            Debug.LogError("UIManager: Unable to find SpawnManager script");
        }

        _player = GameObject.Find("Player");
        if (_player == null)
        {
            Debug.LogError("UIManager: Unable to find Player");
        }
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 0) // Checks for case when Player hit twice by two Lasers
        {
            _livesImg.sprite = _liveSprites[currentLives];

            if (currentLives < 1)
            {
                GameOverSequence();
            }
        }
        
    }

    private void GameOverSequence()
    {
        
        _gameOverText.gameObject.SetActive(true);
        _restartGameText.gameObject.SetActive(true);
        _spawnManager.OnPlayerDeath();
        _gameManager.GameOver();
        StartCoroutine(GameOverFlickerRoutine());
        if (_player != null)
        {
            Destroy(_player);
        }
    }

    private IEnumerator GameOverFlickerRoutine()
    {
        while(true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
}
