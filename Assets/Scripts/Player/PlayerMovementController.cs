using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    float walkingSpeed = 12f;

    [SerializeField]
    float sprintSpeed = 18.0f;

    [Header("Jump")]
    [SerializeField]
    float jumpForce = 25.0f;

    [SerializeField]
    float gravityMultiplier = 12.5f;

    [Header("Crouch")]
    [SerializeField]
    float crouchSpeed = 10.0f;

    [SerializeField]
    float crouchYScale = 0.5f;

    [Header("Sliding")]
    [SerializeField]
    float slideSpeed = 18.0f;

    [SerializeField]
    float slideTime = 5.0f;

    /// <summary>
    /// Private Values
    /// </summary>

    CharacterController _characterController;

    Vector3 _velocity;

    //Private Bools
    bool _isMovePressed;
    bool _isJumpPressed;
    bool _isRunning;
    bool _isCrouch;
    bool _isSliding;

    //Private Floats
    float _inputX;
    float _inputZ;

    float _gravity;

    float _startYScale;
    float _startHeight;

    float _slideTimer;
    float _deaccelerateSlide;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _gravity = Physics.gravity.y;

        _startHeight = _characterController.height;
        _startYScale = transform.localScale.y;
    }

    private void Update()
    {
        HandleInputs();
        HandleGravity();
        HandleCrouch();
        HandleSlide();
        HandleMove();
    }

    void HandleInputs()
    {
        _inputX = Input.GetAxis("Horizontal");
        _inputZ = Input.GetAxis("Vertical");

        _isSliding = Input.GetButton("Sliding");
        _isCrouch = Input.GetButton("Crouch");
        _isJumpPressed = Input.GetButtonDown("Jump");
        _isMovePressed = _inputX != 0.0f || _inputZ != 0.0f;
        _isRunning = _isMovePressed && Input.GetButton("Fire3");
    }

    bool IsGrounded()
    {
        return _characterController.isGrounded;
    }

    IEnumerator WaitForGroundedCoroutine()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(() => IsGrounded());
    }

    /// <summary>
    /// Movement
    /// </summary>
    void HandleMove()
    {
        Vector3 move = transform.right * _inputX + transform.forward * _inputZ;
        move.y = _velocity.y; //Aplica gravedad

        // Si _isRunning es true usará sprintSpeed. 
        // Si es false usará walkingSpeed o crouchSpeed según si está agachado o no. 
        // Si está Sliding, aplicará una aceleración
        float speed = _isRunning ? sprintSpeed : (_isCrouch ? crouchSpeed : (_isSliding ? (walkingSpeed - _deaccelerateSlide) : walkingSpeed));

        _characterController.Move(move * speed * Time.deltaTime);
    }

    /// <summary>
    /// Jump
    /// </summary>
    private void HandleGravity()
    {
        if (IsGrounded())
        {
            if (_velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            if (_isJumpPressed)
            {
                HandleJump();
                StartCoroutine(WaitForGroundedCoroutine());
            }
        }
        _velocity.y += _gravity * gravityMultiplier * Time.deltaTime; //Inmediatamente despues del salto, aplica gravedad
    }

    private void HandleJump()
    {
        _isJumpPressed = false;
        _velocity.y = Mathf.Sqrt(jumpForce * -2f * _gravity);
    }

    /// <summary>
    /// Crouch
    /// </summary>
    void HandleCrouch()
    {
        if (IsGrounded())
        {
            if (_isCrouch)
            {
                if (transform.localScale.y != crouchYScale && _characterController.height == _startHeight)
                {
                    //Modifica la altura para no flotar
                    _characterController.height = crouchYScale;
                    transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                }
            }
            else
            {
                if (transform.localScale.y != _startYScale && _characterController.height != _startHeight)
                {
                    transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
                    //Devuelve la altura original para no hundirse
                    _characterController.height = _startHeight;
                }
            }
        }

    }

    /// <summary>
    /// Sliding
    /// </summary>
    void HandleSlide()
    {
        if (IsGrounded() && _isSliding)
        {
            StartSlide();
        }

        if (_isSliding && _slideTimer > 0)
        {
            // Realiza las acciones de slide
            Slide();
            _slideTimer -= Time.deltaTime;
        }
        else if (_slideTimer <= 0)
        {
            // Termina el slide cuando se agota el tiempo
            EndSlide();
        }
    }

    void StartSlide()
    {
        _isSliding = true;
        _slideTimer = slideTime;
    }

    void Slide()
    {
        _characterController.height = crouchYScale;
        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);

        // Aplica desaceleración a la velocidad durante el slide
        float deacceleration = CalcularDesaceleracion(walkingSpeed * slideSpeed, walkingSpeed, _slideTimer);

        //// Asegúrate de que el temporizador nunca sea negativo
        //_slideTimer = Mathf.Max(0f, Mathf.Round(_slideTimer) - Time.deltaTime);

        // Utiliza la desaceleración calculada para ajustar la velocidad de movimiento en HandleMove
        _deaccelerateSlide = deacceleration;

        Debug.Log(_slideTimer + " " + (walkingSpeed + _deaccelerateSlide));
    }

    void EndSlide()
    {
        _isSliding = false;

        transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
        //Devuelve la altura original para no hundirse
        _characterController.height = _startHeight;
    }


    float CalcularDesaceleracion(float initialVelocity, float finalVelocity, float time)
    {
        // Calcula la desaceleración de manera que disminuye gradualmente
        float deacceleration = (2 * (finalVelocity - initialVelocity)) / (time * time);

        return deacceleration;
    }


}
