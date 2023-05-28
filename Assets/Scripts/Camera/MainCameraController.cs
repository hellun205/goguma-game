using UnityEngine;
using Entity.Player;

namespace Camera
{
  /// <summary>
  /// 메인 카메라의 대한 컨트롤러 입니다. (싱글톤)
  /// </summary>
  public class MainCameraController : MonoBehaviour
  {
    /// <summary>
    /// 현재 인스턴스를 가져옵니다.
    /// </summary>
    public static MainCameraController Instance { get; private set; }

    /// <summary>
    /// 플레이어를 따라갈 때 부드러움의 정도를 설정합니다.
    /// </summary>
    public float smoothing = 0.6f;

    /// <summary>
    /// 카메라의 최소 위치를 설정합니다.
    /// </summary>
    public Vector2 minCameraBoundary;

    /// <summary>
    /// 카메라의 최대 위치를 설정합니다.
    /// </summary>
    public Vector2 maxCameraBoundary;

    /// <summary>
    /// 카메라의 위치를 제한할지 설정합니다.
    /// </summary>
    public bool clamp = false;

    private void Awake()
    {
      if (Instance == null)
        Instance = this;
      else
        Destroy(gameObject);

      DontDestroyOnLoad(gameObject);

    }

    private void FixedUpdate()
    {
      FollowPlayer();
    }

    /// <summary>
    /// 플레이어의 위치로 부드럽게 이동합니다.
    /// </summary>
    private void FollowPlayer()
    {
      var targetPos = PlayerController.Instance.transform.position;
      targetPos = new Vector3(targetPos.x, targetPos.y, this.transform.position.z);

      if (clamp)
      {
        targetPos.x = Mathf.Clamp(targetPos.x, minCameraBoundary.x, maxCameraBoundary.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraBoundary.y, maxCameraBoundary.y);
      }

      transform.position = Vector3.Lerp(transform.position, targetPos, smoothing * Time.fixedDeltaTime);
    }
  }
}
