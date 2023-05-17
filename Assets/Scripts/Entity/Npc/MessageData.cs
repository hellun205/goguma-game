namespace Entity.Npc {
  public class MessageData {
    public string name;
    
    public string text;

    private byte _line = 1;

    public byte line {
      get => (byte)(string.IsNullOrEmpty(name) ? _line : _line + 1);
      set => _line = value;
    }

    private float _exitTime = 0f;
    
    public float exitTime {
      get => _exitTime == 0f ? defaultExitTime : exitTime;
      set => _exitTime = value;
    }

    public float defaultExitTime => text.Length * 0.05f + 3f;

    public float panelWidth = 100f;

    public const float emptyHeight = 15f;

    public const float fontHeight = 16f; 

    public MessageData(string name, string text, byte line = 1, float exitTime = 0f) {
      this.name = name;
      this.text = text;
      _line = line;
      _exitTime = exitTime;
    }

    public MessageData(string text, byte line = 1, float exitTime = 0f) {
      this.text = text;
      _line = line;
      _exitTime = exitTime;
    }
  }
}