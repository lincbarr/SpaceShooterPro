using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{ 
    [SerializeField] private int _lives = 3;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shieldVisualizer;
    [SerializeField] private GameObject _rightEngine, _leftEngine;
    [SerializeField] private AudioClip _laserSoundClip;

    private AudioSource _audioSource;

    [SerializeField] private int _score;

    private UIManager _uiManager;
    
    private bool _areShieldsActive = false;
    private bool _isTripleShotActive = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();

        if (_uiManager == null)
        {
            Debug.LogError("Player: Unable to find UIManager script.");
        }

        if (_audioSource == null)
        {
            Debug.LogError("Player: Unable to find AudioSource");
        } else
        {
            _audioSource.clip = _laserSoundClip;
        }

        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.0f, 0.0f));

        if (transform.position.x >= 11.4f)
        {
            transform.position = new Vector3(-11.4f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.4f)
        {
            transform.position = new Vector3(11.4f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (_areShieldsActive == true)
        {
            _shieldVisualizer.SetActive(false);
            _areShieldsActive = false;
            return;
        } 

        _lives--;

        if (_lives == 2)
        {
            _leftEngine.SetActive(true);
        } else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        } 

        _uiManager.UpdateLives(_lives);

    }

    public void FireLaser()
    {
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }
       
        _audioSource.Play();
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    private IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isTripleShotActive = false;
    }

    public void ActivateShields()
    {
        _shieldVisualizer.SetActive(true);
        _areShieldsActive = true;
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }
}
