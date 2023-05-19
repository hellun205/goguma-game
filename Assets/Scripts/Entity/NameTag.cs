using Entity.UI;
using UnityEngine;

namespace Entity {
  /// <summary>
  /// 엔티티의 이름을 표시 하는 컴포넌트
  /// </summary>
  public class NameTag : MonoBehaviour {
    private string _text;
    private Vector3 tempPosition;
    
    /// <summary>
    /// 표시할 텍스트 (엔티티의 이름)을 지정하거나 가져옵니다.
    /// </summary>
    public string text {
      get => _text;
      set {
        _text = value;
        Refresh();
      }
    }
    
    /// <summary>
    /// 이름표 엔티티
    /// </summary>
    private DisplayText obj;
    
    /// <summary>
    /// 이름표를 표시 할 엔티티
    /// </summary>
    private Entity entity;
    
    /// <summary>
    /// 이름표의 위치
    /// </summary>
    [SerializeField]
    private Transform position;

    private void Awake() {
      entity = GetComponent<Entity>();

      _text = entity.entityName;
      tempPosition = entity.position;
    }

    private void Start() {
      obj = (DisplayText) EntityManager.Get(EntityType.DisplayText);
      Refresh();
    }

    private void Update() {
      if (tempPosition == entity.position) return;
      tempPosition = entity.position;
      Refresh();
    }

    /// <summary>
    /// 새로고침 합니다.
    /// </summary>
    private void Refresh() {
      obj.text = text;
      obj.width = 10f * text.Length + 20f;
      obj.position = position.position;
    }
  }
}