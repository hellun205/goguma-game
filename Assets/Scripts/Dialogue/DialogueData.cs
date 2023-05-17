using System;

namespace Dialogue {
  [Serializable]
  public class DialogueData {
    public Speaker speaker;
    
    public string text;

    public DialogueData(Speaker speaker, string text) {
      this.speaker = speaker;
      this.text = text;
    }
  }
}