using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player {
  public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float jumpSpeed = 7f;

    [SerializeField]
    private LayerMask layerMask = 0;

    public bool canFlip = true;

    private Vector3 startScale;
    private bool isJumping;
    private bool wasLeft = true;
    private float distance = 0f;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private void Awake() {
      animator = GetComponent<Animator>();
      rb = GetComponent<Rigidbody2D>();
      sr = GetComponent<SpriteRenderer>();
      distance = GetComponent<BoxCollider2D>().bounds.extents.y + 0.1f;

      startScale = transform.localScale;
    }

    private void FixedUpdate() {
      TryMove();
    }

    private void Update() {
      TryJump();
      CheckGround();
    }

    private void TryMove() {
      var horizontal = Input.GetAxisRaw("Horizontal");

      animator.SetBool("isWalking", horizontal != 0);
      transform.Translate(horizontal * Time.deltaTime * moveSpeed, 0f, 0f);
      if (horizontal < 0) wasLeft = false;
      else if (horizontal > 0) wasLeft = true;
      
      Flip();
    }

    private void CheckGround() {
      if (rb.velocity.y < 0) {
        var hit = Physics2D.Raycast(transform.position, Vector2.down, distance, layerMask);

        if (hit && hit.transform.CompareTag("Ground")) {
          SetJumping(false);
        }
      }
    }

    private void TryJump() {
      if (!isJumping && Input.GetKey(jumpKey)) {
        SetJumping(true);
        // rb.AddForce(Vector2.up * (jumpSpeed * 100f));
        rb.velocity = Vector2.up * jumpSpeed;
      }
    }

    private void SetJumping(bool value) {
      isJumping = value;
      animator.SetBool("isJumping", value);
    }

    private void Flip() {
      // rotate
      // sr.flipX = !wasLeft;
      if (!canFlip) return;
      if (wasLeft) {
        transform.localScale = new Vector3(1f, 1f, 1f); 
      } else {
        transform.localScale = new Vector3(-1f, 1f, 1f);
      }
    }
  }
}