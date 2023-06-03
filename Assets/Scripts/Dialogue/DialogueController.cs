using System;
using System.Collections;
using Animation.Combined;
using Entity.Player;
using JetBrains.Annotations;
using Manager;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Dialogue
{
  public class DialogueController : SingleTon<DialogueController>
  {
    [Header("Dialogue Objects")]
    [SerializeField]
    private Image leftAvatar;

    [SerializeField]
    private Image rightAvatar;

    [SerializeField]
    private TextMeshProUGUI speakerName;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Button nextBtn;

    [SerializeField]
    private TextMeshProUGUI nextBtnText;

    private Image nextBtnImg;

    [SerializeField]
    private Button previousBtn;

    [SerializeField]
    private TextMeshProUGUI previousBtnText;

    private Image previousBtnImg;

    private CanvasGroup canvasGroup;

    [Header("Dialogue")]
    private const float WriteDelay = 0.05f;

    public bool isEnabled { get; private set; }

    public bool skipable = true;

    [Header("Ask Setting")]
    public Color yesColor;

    public Color noColor;

    public Color defaultColor;

    private char[] curText;

    private int curTextIndex;

    private bool isWriting;

    private bool isMulti;

    private DialogueData[] multiList;

    private int multIndex;

    private (bool isAsk, string yes, string no) ask;

    private Action<bool> callback;

    private Coroutiner writeCoroutiner;

    // Animation
    private SmoothSizeAndFade anim;

    private readonly Vector2 zero = new(0.7f, 0.7f);

    protected override void Awake()
    {
      base.Awake();

      nextBtnImg = nextBtn.GetComponent<Image>();
      previousBtnImg = previousBtn.GetComponent<Image>();
      canvasGroup = GetComponent<CanvasGroup>();

      anim = new(this,
        new(() => transform.localScale, value => transform.localScale = value),
        new(() => canvasGroup.alpha, value => canvasGroup.alpha = value))
      {
        minSize = zero,
        maxSize = transform.localScale,
        fadeAnimSpeed = 6f,
        sizeAnimSpeed = 8f
      };
      anim.onStarted += sender => canvasGroup.blocksRaycasts = true;
      anim.onHid += sender => canvasGroup.blocksRaycasts = false;
      writeCoroutiner = new(this, WriteCoroutine);
      canvasGroup.alpha = 0f;
      canvasGroup.blocksRaycasts = false;

      ResetUI();
      Close();
    }

    /// <summary>
    /// Reset the Dialogue panel
    /// </summary>
    /// <param name="speaker">speaker data</param>
    /// <param name="isAsk">type is ask</param>
    private void ResetUI(Speaker speaker = null, bool isAsk = false)
    {
      speakerName.text = string.Empty;
      text.text = string.Empty;
      speakerName.alignment = TextAlignmentOptions.TopLeft;
      text.alignment = TextAlignmentOptions.TopLeft;
      ask.isAsk = isAsk;
      leftAvatar.gameObject.SetActive(false);
      rightAvatar.gameObject.SetActive(false);
      nextBtn.gameObject.SetActive(false);
      previousBtn.gameObject.SetActive(false);

      if (speaker == null)
        return;

      switch (speaker.avatarPosition)
      {
        case AvatarPosition.Left:
          leftAvatar.gameObject.SetActive(true);
          leftAvatar.sprite = speaker.avatarSprite;
          // speakerName.alignment = TextAlignmentOptions.Left;
          // text.alignment = TextAlignmentOptions.Left;
          break;

        case AvatarPosition.Right:
          rightAvatar.gameObject.SetActive(true);
          rightAvatar.sprite = speaker.avatarSprite;
          speakerName.alignment = TextAlignmentOptions.TopRight;
          text.alignment = TextAlignmentOptions.TopRight;
          break;
      }

      speakerName.color = speaker.nameColor;
      speakerName.text = speaker.name;
    }

    private void Update()
    {
      if (!isEnabled || PlayerController.Instance.isInputCooldown)
        return;

      // 스킵
      if (Input.GetKeyDown(Managers.Key.next) && isWriting)
      {
        if (!skipable)
          return;

        isWriting = false;
        PlayerController.Instance.EnableInputCooldown();
      }

      if (PlayerController.Instance.isInputCooldown)
        return;

      if (ask.isAsk)
      {
        if (Input.GetKeyDown(Managers.Key.yes))
          NextOrCloseOrYes();
        else if (Input.GetKeyDown(Managers.Key.no))
          PreviousOrNo();
      }
      else
      {
        // 다음 / 확인
        if (Input.GetKeyDown(Managers.Key.next))
          NextOrCloseOrYes();
      }
    }

    /// <summary>
    /// Close Dialogue panel
    /// </summary>
    /// <param name="btn"></param>
    private void Close(bool? btn = null)
    {
      anim.Hide();
      isEnabled = false;

      if (btn.HasValue)
        callback?.Invoke(btn.Value);
    }

    /// <summary>
    /// Start Dialogue
    /// </summary>
    /// <param name="dialogue">dialogue data</param>
    /// <param name="callback">callback at end of dialogue</param>
    /// <param name="isAsk">type is ask</param>
    public void ShowDialogue(DialogueData dialogue, [CanBeNull] Action<bool> callback = null, bool isAsk = false)
    {
      if (isEnabled)
        return;

      isEnabled = true;
      anim.Show();
      ResetUI(dialogue.speaker, isAsk);
      ask.isAsk = isAsk;
      curText = dialogue.text.ToCharArray();
      isMulti = false;
      this.callback = callback;
      writeCoroutiner.Start();
    }

    /// <summary>
    /// Start Dialogues
    /// </summary>
    /// <param name="dialogues">dialogue collection</param>
    /// <param name="callback">callback at end of dialogue</param>
    public void ShowDialogues(DialogueData[] dialogues, [CanBeNull] Action<bool> callback = null)
    {
      if (isEnabled)
        return;

      isEnabled = true;
      anim.Show();
      isMulti = true;
      multiList = dialogues;
      multIndex = 0;
      this.callback = callback;
      SetMultiDialogue();
    }

    /// <summary>
    /// Start ask type dialogue
    /// </summary>
    /// <param name="dialogue">대화 데이터</param>
    /// <param name="callback">대화 종료 후 콜백(예: 참, 아니요: 거짓 반환)</param>
    /// <param name="yesText">예 버튼의 텍스트</param>
    /// <param name="noText">아니요 버튼의 텍스트</param>
    public void Ask(DialogueData dialogue, Action<bool> callback, string yesText = "예", string noText = "아니요")
    {
      ask.yes = yesText;
      ask.no = noText;
      ShowDialogue(dialogue, callback, true);
    }

    private void SetMultiDialogue()
    {
      var dialogue = multiList[multIndex];
      ResetUI(dialogue.speaker);
      curText = dialogue.text.ToCharArray();
      writeCoroutiner.Start();
    }

    private IEnumerator WriteCoroutine()
    {
      curTextIndex = 0;
      isWriting = true;
      PlayerController.Instance.EnableInputCooldown();
      
      while (curTextIndex < curText.Length)
      {
        text.text += curText[curTextIndex];
        curTextIndex++;
        yield return isWriting ? new WaitForSeconds(WriteDelay) : null;
      }
      isWriting = false;
      ShowBtns();
    }

    private void ShowBtns()
    {
      SetBtnColor(ask.isAsk, yesColor, noColor, defaultColor);
      Debug.Log(ask.isAsk);

      if (ask.isAsk)
      {
        nextBtnText.text = ask.yes;
        previousBtnText.text = ask.no;
        previousBtn.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);
      }
      else
      {
        if (isMulti)
        {
          nextBtnText.text = (multIndex < multiList.Length - 1 ? "다음" : "확인");
          previousBtn.gameObject.SetActive(multIndex > 0);
          nextBtn.gameObject.SetActive(true);
        }
        else
        {
          nextBtnText.text = "확인";
          nextBtn.gameObject.SetActive(true);
        }
      }
    }

    private void HideBtns()
    {
      nextBtn.gameObject.SetActive(false);
      previousBtn.gameObject.SetActive(false);
    }

    public void OnNextButtonClick() => NextOrCloseOrYes();

    public void OnPreviousButtonClick() => PreviousOrNo();

    private void NextOrCloseOrYes()
    {
      if (ask.isAsk)
        Close(true);
      else
      {
        if (isMulti && multIndex < multiList.Length - 1)
        {
          multIndex++;
          HideBtns();
          SetMultiDialogue();
        }
        else
          Close(true);
      }

      PlayerController.Instance.EnableInputCooldown();
    }

    private void PreviousOrNo()
    {
      if (ask.isAsk)
        Close(false);
      else
      {
        if (multIndex <= 0)
          return;

        multIndex--;
        HideBtns();
        SetMultiDialogue();
      }

      PlayerController.Instance.EnableInputCooldown();
    }

    private void SetBtnColor(bool isAsk, Color yesColor, Color noColor, Color defColor)
    {
      if (isAsk)
      {
        nextBtnImg.color = yesColor;
        previousBtnImg.color = noColor;
      }
      else
      {
        nextBtnImg.color = defColor;
        previousBtnImg.color = defColor;
      }
    }
  }
}
