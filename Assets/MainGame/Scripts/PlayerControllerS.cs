using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerControllerS : MonoBehaviour
{
    private Rigidbody2D _playerRB;
    private CapsuleCollider2D _playerColider;
    private Animator _playerAnimator;
    private SpriteRenderer sprite;
    private uint _coinCount;
    public UnityEngine.UI.Button ButtonUp;
    public UnityEngine.UI.Button ButtonDown;
    public AudioSource BackGroundSound;
    public AudioSource CoinGetSound;
    public AudioSource DiamondGetSound;
    public AudioSource WalkSound;
    public AudioSource RunSound;
    public AudioSource JumpSound;
    public float JumpWeight;
    public int PlayerSpeed;
    public Text CointCount;
    bool IsInWater = false;
    bool IsRBPressedWalk;
    bool IsLBPressedWalk;
    bool IsRBPressedRun;
    bool IsLBPressedRun;
    bool IsUpBPressed;
    bool IsDownBPressed;

    const float WaterGravityScale = 0.5f;
    const float DefaulthGravityScale = 10f;
    void Start()
    {
        ButtonUp.enabled = false;
        ButtonDown.enabled = false;
        IsRBPressedWalk = false;
        IsLBPressedWalk = false;
        _playerRB = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _playerColider = GetComponent<CapsuleCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {

    }
    public void BGSoundSwicher()
    {
        if(BackGroundSound.isPlaying)
        {
            BackGroundSound.Stop();
        }
        else
        {
            BackGroundSound.Play();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            IsLBPressedWalk = false;
            IsRBPressedWalk = false;
            IsUpBPressed = false;
            IsDownBPressed = false;
            _playerRB.velocity = new Vector2(0, 0);
            _playerAnimator.SetBool("SwimRun", false);
            _playerAnimator.SetBool("SwimIdle", false);
            IsInWater = false;
            ButtonUp.enabled = false;
            ButtonDown.enabled = false;
            _playerRB.gravityScale = DefaulthGravityScale;
            Debug.Log("Water Exit");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            _playerAnimator.SetBool("SwimIdle", true);
            _playerAnimator.SetBool("SwimRun", false);
            ButtonUp.enabled = true;
            ButtonDown.enabled = true;
            IsInWater = true;
            _playerRB.gravityScale = WaterGravityScale;
            Debug.Log("Water enter");
        }
        else if (collision.tag == "Coin")
        {
            CoinGetSound.Play();
            Destroy(collision.gameObject);
            CointCount.text = int.Parse(CointCount.text) + 1 + "";
        }
        else if (collision.tag == "Diamond")
        {
            DiamondGetSound.Play();
            Destroy(collision.gameObject);
            CointCount.text = int.Parse(CointCount.text) + 15 + "";
        }
        else if (collision.tag == "Respawn")
        {
            SceneManager.LoadScene("Game");
        }
    }
    void FixedUpdate()
    {
        if (IsLBPressedWalk)
        {
            _playerRB.velocity = new Vector2(-PlayerSpeed, _playerRB.velocity.y);
        }
        else if (IsRBPressedWalk)
        {
            _playerRB.velocity = new Vector2(PlayerSpeed, _playerRB.velocity.y);
        }
        else if (IsLBPressedRun)
        {
            _playerRB.velocity = new Vector2(-(PlayerSpeed * 3), _playerRB.velocity.y);
        }
        else if (IsRBPressedRun)
        {
            _playerRB.velocity = new Vector2(PlayerSpeed * 3, _playerRB.velocity.y);
        }
        else if (IsUpBPressed)
        {
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, PlayerSpeed);
        }
        else if (IsDownBPressed)
        {
            _playerRB.velocity = new Vector2(_playerRB.velocity.x, -PlayerSpeed);
        }
    }
    public void RightButtonUp()
    {
        if (IsInWater)
        {
            _playerAnimator.SetBool("SwimIdle", true);
            _playerAnimator.SetBool("SwimRun", false);
        }
        else
        {
            _playerAnimator.SetBool("Walk", false);
            _playerAnimator.SetBool("Run", false);
        }
        IsRBPressedRun = false;
        WalkSound.Stop();
        RunSound.Stop();
        IsRBPressedWalk = false;
    }
    public void LeftButtonUp()
    {
        if (IsInWater)
        {
            _playerAnimator.SetBool("SwimIdle", true);
            _playerAnimator.SetBool("SwimRun", false);
        }
        else
        {
            _playerAnimator.SetBool("Walk", false);
            _playerAnimator.SetBool("Run", false);
        }
        IsLBPressedRun = false;
        WalkSound.Stop();
        RunSound.Stop();
        IsLBPressedWalk = false;
    }

    float lastClick = 0f;
    float interval = 0.5f;
    public void JumpButtonClick()
    {
        if (_playerRB.IsTouchingLayers() && !IsInWater)
        {
            _playerAnimator.SetTrigger("Jump");
            _playerRB.AddForce(transform.up * JumpWeight);
            JumpSound.Play();
        }
    }
    public void RightButtonDown()
    {
        if (IsInWater)
        {
            _playerAnimator.SetBool("SwimIdle", false);
            _playerAnimator.SetBool("SwimRun", true);
            IsRBPressedWalk = true;
        }
        else
        {
            if ((lastClick + interval) > Time.time)
            {
                _playerAnimator.SetBool("Run", true);
                IsRBPressedRun = true;
                RunSound.Play();
            }
            else
            {
                _playerAnimator.SetBool("Walk", true);
                IsRBPressedWalk = true;
                WalkSound.Play();
            }
            lastClick = Time.time;
        }
        sprite.flipX = false;
    }
    public void LeftButtonDown()
    {
        if (IsInWater)
        {
            _playerAnimator.SetBool("SwimIdle", false);
            _playerAnimator.SetBool("SwimRun", true);
            IsLBPressedWalk = true;
        }
        else
        {
            if ((lastClick + interval) > Time.time)
            {
                _playerAnimator.SetBool("Run", true);
                IsLBPressedRun = true;
                RunSound.Play();
            }
            else
            {
                WalkSound.Play();
                _playerAnimator.SetBool("Walk", true);
                IsLBPressedWalk = true;
            }
            lastClick = Time.time;
        }
        sprite.flipX = true;
    }
    public void UpButtonUp()
    {
        if (IsInWater)
        {
            _playerAnimator.SetBool("SwimIdle", true);
            _playerAnimator.SetBool("SwimRun", false);
            IsUpBPressed = false;
        }
    }
    public void DownButtonUp()
    {
        if (IsInWater)
        {
            _playerAnimator.SetBool("SwimIdle", true);
            _playerAnimator.SetBool("SwimRun", false);
            IsDownBPressed = false;
        }
    }
    public void UpButtonDown()
    {
        if (IsInWater)
        {
            _playerAnimator.SetBool("SwimIdle", false);
            _playerAnimator.SetBool("SwimRun", true);
            IsUpBPressed = true;
        }
    }
    public void DownButtonDown()
    {
        if (IsInWater)
        {
            _playerAnimator.SetBool("SwimIdle", false);
            _playerAnimator.SetBool("SwimRun", true);
            IsDownBPressed = true;
        }
    }
}
