using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerScript_InputAction : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 3.5f;
    [SerializeField] private float _speedBoostMultiplier = 2.0f;
    [SerializeField] private float _fireRate = 0.15f;

    private float _canFire = -1.0f;
    private Vector2 m_move;
    private Player _player;
    private GameManager _gameManager;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("PlayerScript_InputAction: Unable to find Player script.");
        }
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("PlayerScript_InputAction: Unable to find GameManager script.");
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_move = context.ReadValue<Vector2>();
    }

    private float lastClickTime = 0.0f;
    private float debounceDelay = 0.005f;

    public void OnFire(InputAction.CallbackContext context)
    {
        if ((Time.time - lastClickTime) < debounceDelay) // To remove the double trigger on press
        {
            return;
        }
        lastClickTime = Time.time;

        if (lastClickTime > _canFire)
        {
            _canFire = lastClickTime + _fireRate;
            if (_player != null)
            {
                _player.FireLaser();
            }
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if ((Time.time - lastClickTime) < debounceDelay) // To remove the double trigger on press
        {
            return;
        }
        lastClickTime = Time.time;

        if (lastClickTime > _canFire)
        {
            _canFire = lastClickTime + _fireRate;
            _gameManager.RestartGame();
        }
    }

    public void OnQuit(InputAction.CallbackContext context)
    {
        Application.Quit();
    }

    private void Update()
    {
        Move(m_move);
    }

    private void Move(Vector2 direction)
    {
        if (_player != null)
        {
            if (direction.sqrMagnitude < 0.01)
                return;
            var scaledMoveSpeed = _moveSpeed * Time.deltaTime;
            var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(direction.x, direction.y, 0);
            _player.transform.position += move * scaledMoveSpeed;
        }
    }

    public void SpeedPowerUp()
    {
        _moveSpeed *= _speedBoostMultiplier;
        StartCoroutine(SpeedPowerDown());
    }

    private IEnumerator SpeedPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        _moveSpeed /= _speedBoostMultiplier;
    }
}
