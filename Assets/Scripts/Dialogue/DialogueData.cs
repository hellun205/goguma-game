using System;

namespace Dialogue {
  /// <summary>
  /// 대화의 대한 데이터 입니다.
  /// </summary>
  [Serializable]
  public class DialogueData {
    /// <summary>
    /// 화자의 대한 데이터를 설정합니다.
    /// </summary>
    public Speaker speaker;
    
    /// <summary>
    /// 화자의 말을 설정합니다.
    /// </summary>
    public string text;

    /// <summary>
    /// 대화의 대한 데이터를 생성합니다.
    /// </summary>
    /// <param name="speaker">화자의 대한 데이터</param>
    /// <param name="text">화자의 말</param>
    public DialogueData(Speaker speaker, string text) {
      this.speaker = speaker;
      this.text = text;
    }
  }
}