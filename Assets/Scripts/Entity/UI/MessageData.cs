namespace Entity.UI
{
  /// <summary>
  /// 메시지 박스에 대한 정보 입니다.
  /// </summary>
  public class MessageData
  {
    /// <summary>
    /// 표시할 내용을 지정하거나 가져옵니다.
    /// </summary>
    public string text;

    private float _exitTime = 0f;

    /// <summary>
    /// 메시지 박스가 사라질 시간을 지정합니다. 지정하지 않으면 내용의 길이에 따라 달라집니다.
    /// </summary>
    public float exitTime
    {
      get => _exitTime == 0f ? defaultExitTime : exitTime;
      set => _exitTime = value;
    }

    /// <summary>
    /// 메시지 박스가 사라질 시간을 내용의 길이에 따라 계산한 값입니다.
    /// </summary>
    public float defaultExitTime => text.Length * 0.05f + 3f;

    /// <summary>
    /// 메시지 박스에 대한 정보를 생성합니다.
    /// </summary>
    /// <param name="text">표시 할 내용</param>
    /// <param name="exitTime">표시 시간</param>
    public MessageData(string text, float exitTime = 0f)
    {
      this.text = text;
      this.exitTime = exitTime;
    }
  }
}
