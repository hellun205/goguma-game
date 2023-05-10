using System;
using UnityEngine;

namespace Player {
  public class Movement : MonoBehaviour {

    public float moveSpeed = 3f;
    
    private Animator animator;

    private void Awake() {
      animator = GetComponent<Animator>();

    }

    private void FixedUpdate() {
      var horizontal = Input.GetAxisRaw("Horizontal");
      var vertical = Input.GetAxisRaw("Vertical");

      animator.SetFloat("vertical", vertical);
      animator.SetFloat("horizontal", vertical == 0 ? horizontal : 0);
      animator.SetBool("isWalking", horizontal != 0 || vertical != 0);

      
      transform.Translate(horizontal * Time.deltaTime * moveSpeed, vertical * Time.deltaTime * moveSpeed, 0f);

    }
  }
}