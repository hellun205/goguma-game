using System;
using Player;
using UnityEngine;
using Entity.Player;

namespace Camera {
  public class MainCameraController : MonoBehaviour {
    public static MainCameraController Instance { get; private set; }
    public float smoothing = 0.6f;   
    public Vector2 minCameraBoundary;
    public Vector2 maxCameraBoundary;
    public bool clamp = false;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);

    }

    private void OnDestroy() {
      if (Instance == this) Instance = null;
    }

    private void FixedUpdate() {
      FollowPlayer();
    }

    private void FollowPlayer() {
      var targetPos = PlayerController.Instance.transform.position;
      targetPos = new Vector3(targetPos.x, targetPos.y, this.transform.position.z);
      if (clamp) {
        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);
      }
      transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
    }
  }
}