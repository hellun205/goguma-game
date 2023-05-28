using Audio;
using Camera;
using Dialogue;
using UnityEngine;
using Window;

namespace Entity.Player
{
  public class PlayerMovement : Movement
  {
    public bool isInputCooldown => curInputCooldown < inputCooldown;
    public float inputCooldown = 0.5f;
    private float curInputCooldown;

    [SerializeField]
    private KeyCode jumpKey = KeyCode.Space;

    private PlayerStatus status => controller.status;

    // Components
    private PlayerController controller;

    protected override void Awake()
    {
      base.Awake();
      animator = GetComponent<Animator>();
      controller = GetComponent<PlayerController>();

      curInputCooldown = inputCooldown;
    }

    protected override void Update()
    {
      base.Update();
      if (isInputCooldown)
        curInputCooldown += Time.deltaTime;
      else
      {
        var interactable = !InputBoxWindow.isEnabled &&
                           !DialogueController.Instance.isEnabled;
        
        var horizontal = Input.GetAxisRaw("Horizontal");
        moveSpeed = status.moveSpeed;
        jumpPower = status.jumpPower;
        animator.SetFloat("moveSpeed", Mathf.Max(1f, status.moveSpeed));
        Move(interactable ? horizontal : 0f);
        if (interactable  && Input.GetKeyDown(KeyCode.Space) && canJump)
        {
          AudioManager.Play("jump");
          Jump();
        }
      }

      CheckGround();
    }

    public void EnableInputCooldown() => curInputCooldown = 0f;
  }
}
