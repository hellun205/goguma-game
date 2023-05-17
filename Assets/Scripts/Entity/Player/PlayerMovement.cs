using System;
using Dialogue;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entity.Player {
  public class PlayerMovement : MonoBehaviour {
    // Inspector Settings
    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    [SerializeField]
    private float moveSpeed = 3f;

    [SerializeField]
    private float jumpSpeed = 7f;

    [SerializeField]
    private LayerMask layerMask = 0;

    // Variables
    [HideInInspector]
    public bool canFlip = true;

    [HideInInspector]
    public bool wasLeft = true;
    
    private bool isJumping;
    private float distanceX = 0f;
    private float distanceY = 0f;

    // Components
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    private void Awake() {
      animator = GetComponent<Animator>();
      rb = GetComponent<Rigidbody2D>();
      sr = GetComponent<SpriteRenderer>();
      boxCol = GetComponent<BoxCollider2D>();

      distanceX = boxCol.bounds.extents.x;
      distanceY = boxCol.bounds.extents.y + 0.2f;
    }

    private void FixedUpdate() {
      TryMove();
    }

    private void Update() {
      TryJump();
      CheckGround();
    }

    private void TryMove() {
      if (DialogueController.Instance.isEnabled) return;
      var horizontal = Input.GetAxisRaw("Horizontal");

      animator.SetBool("isWalking", horizontal != 0);
      transform.Translate(horizontal * Time.deltaTime * moveSpeed, 0f, 0f);
      if (horizontal < 0) wasLeft = false;
      else if (horizontal > 0) wasLeft = true;

      Flip();
    }

    private void CheckGround() {
      try {
        // if (rb.velocity.y < 0) {
        var pos = transform.position;
        var hitLeft = Physics2D.Raycast(new Vector2(pos.x - distanceX, pos.y), Vector2.down, distanceY, layerMask);
        var hitRight = Physics2D.Raycast(new Vector2(pos.x + distanceX, pos.y), Vector2.down, distanceY, layerMask);
        if ((hitLeft || hitRight) &&
            (hitLeft.transform.CompareTag("Ground") || hitRight.transform.CompareTag("Ground"))) {
          SetJumping(false);
        } else {
          SetJumping(true);
        }
        // }
      } catch {
      }
    }

    private void TryJump() {
      if (DialogueController.Instance.isEnabled) return;
      if (!isJumping && Input.GetKeyDown(jumpKey)) {
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