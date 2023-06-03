using System;

namespace Utils
{
  public sealed class StructPointer<T> where T : struct
  {
    private Func<T> _getter;
    private Action<T> _setter;

    public StructPointer(Func<T> getter, Action<T> setter)
    {
      _getter = getter;
      _setter = setter;
    }

    public T value
    {
      get => _getter.Invoke();
      set => _setter.Invoke(value);
    }
  }
}
