namespace Entity.UI {
  /// <summary>
  /// 메시지 박스에 대한 정보 입니다.
  /// </summary>
  public class MessageData {
    /// <summary>
    /// 표시할 내용을 지정하거나 가져옵니다.
    /// </summary>
    public string text;

    /// <summary>
    /// 내용의 줄이 길어질 경우 지정합니다. 값에 따라 메시지 박스의 크기가 달라집니다.
    /// </summary>
    public byte line = 1;
    
    private float _exitTime = 0f;
    
    /// <summary>
    /// 메시지 박스가 사라질 시간을 지정합니다. 지정하지 않으면 내용의 길이에 따라 달라집니다.
    /// </summary>
    public float exitTime {
      get => _exitTime == 0f ? defaultExitTime : exitTime;
      set => _exitTime = value;
    }

    /// <summary>
    /// 메시지 박스가 사라질 시간을 내용의 길이에 따라 계산한 값입니다.
    /// </summary>
    public float defaultExitTime => text.Length * 0.05f + 3f;

    /// <summary>
    /// 메시지 박스의 넓이를 지정합니다.
    /// </summary>
    public float panelWidth = 300f;

    /// <summary>
    /// 내용의 여백 간격입니다.
    /// </summary>
    public const float emptyHeight = 40f;
    
    /// <summary>
    /// 글꼴의 길이입니다. 
    /// </summary>
    public const float fontHeight = 20f;

    /// <summary>
    /// 메시지 박스에 대한 정보를 생성합니다.
    /// </summary>
    /// <param name="text">표시 할 내용</param>
    /// <param name="line">내용의 길이(줄)</param>
    /// <param name="exitTime">표시 시간</param>
    public MessageData(string text, byte line = 1, float exitTime = 0f) {
      this.text = text;
      this.line = line;
      this.exitTime = exitTime;
    }
  }
}