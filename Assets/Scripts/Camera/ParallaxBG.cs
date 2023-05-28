using UnityEngine;

namespace Camera
{
  public class ParallaxBG : MonoBehaviour
  {
    [SerializeField]
    private SpriteRenderer[] backgrounds;

    [SerializeField]
    private float[] parallaxEffectMultiplier;

    private new UnityEngine.Camera camera;

    private float tmpSize;

    private Vector3 lastCameraPos;

    // private float textureUnitSizeX;

    private void Awake()
    {
      camera = UnityEngine.Camera.main;

      // var sprite = backgrounds[0].sprite;
      // var texture = sprite.texture;
      // textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void FixedUpdate()
    {
      if (!Mathf.Approximately(camera.orthographicSize, tmpSize))
      {
        tmpSize = camera.orthographicSize;
        ReSizeBG();
      }
    }

    private void Start()
    {
      lastCameraPos = camera.transform.position;
    }

    private void LateUpdate()
    {
      var deltaMovement = camera.transform.position - lastCameraPos;
      for (var i = 0; i < backgrounds.Length; i++)
      {
        var bg = backgrounds[i];
        bg.transform.Translate(-deltaMovement.x * parallaxEffectMultiplier[i], 0f, 0f);
        // if (camera.transform.position.x - bg.transform.position.x >= textureUnitSizeX)
        // {
        //   var offsetPosX = (camera.transform.position.x - bg.transform.position.x) % textureUnitSizeX;
        //   bg.transform.position = new Vector3(camera.transform.position.x, bg.transform.position.y);
        // }
      }
      lastCameraPos = camera.transform.position;
    }

    private void ReSizeBG()
    {
      var spriteY = backgrounds[0].sprite.bounds.size.y;
      var screenY = camera.orthographicSize * 2;
      var y = Mathf.Ceil(screenY / spriteY);

      foreach (var bg in backgrounds)
      {
        bg.transform.localScale = new Vector2(y, y);
      }
    }
  }
}
