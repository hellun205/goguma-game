using System;
using System.Collections;
using Entity.Player;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Dialogue {
  public class DialogueController : MonoBehaviour {
    public static DialogueController Instance { get; private set; }

    [Header("UI Object")]
    [SerializeField]
    private GameObject panel;

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

    [Header("Dialogue")]
    public float writeDelay = 0.05f;

    public KeyCode nextKey = KeyCode.Space;

    public bool isEnabled { get; private set; }

    public bool skipable = true;

    [Header("Ask Setting")]
    public Color yesColor;

    public Color noColor;

    public Color defaultColor;

    public KeyCode yesKey = KeyCode.Z;

    public KeyCode noKey = KeyCode.X;

    private char[] curText;
    private int curTextIndex;
    private bool isWriting;
    private bool isMulti;
    private DialogueData[] multiList;
    private int multIndex;
    private (bool isAsk, string yes, string no) ask;
    private Action<bool> callback;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);
      nextBtnImg = nextBtn.GetComponent<Image>();
      previousBtnImg = previousBtn.GetComponent<Image>();

      ResetUI();
      Close();
    }

    private void ResetUI(Speaker speaker = null, bool isAsk = false) {
      speakerName.text = string.Empty;
      text.text = string.Empty;
      speakerName.alignment = TextAlignmentOptions.TopLeft;
      text.alignment = TextAlignmentOptions.TopLeft;
      ask.isAsk = isAsk;
      leftAvatar.gameObject.SetActive(false);
      rightAvatar.gameObject.SetActive(false);
      nextBtn.gameObject.SetActive(false);
      previousBtn.gameObject.SetActive(false);

      if (speaker == null) return;

      switch (speaker.avatarPosition) {
        case AvatarPosition.Left:
          leftAvatar.gameObject.SetActive(true);
          leftAvatar.sprite = speaker.sprite;
          // speakerName.alignment = TextAlignmentOptions.Left;
          // text.alignment = TextAlignmentOptions.Left;
          break;

        case AvatarPosition.Right:
          rightAvatar.gameObject.SetActive(true);
          rightAvatar.sprite = speaker.sprite;
          speakerName.alignment = TextAlignmentOptions.TopRight;
          text.alignment = TextAlignmentOptions.TopRight;
          break;
      }

      speakerName.color = speaker.nameColor;
      speakerName.text = speaker.name;
    }

    private void Update() {
      if (!isEnabled || PlayerController.Instance.isInputCooldown) return;
      
      if (Input.GetKeyDown(nextKey) && isWriting) {
        if (!skipable) return;
        for (var i = curTextIndex; i < curText.Length; i++) {
          text.text += curText[i];
        }
        EndWriting();
        PlayerController.Instance.EnableInputCooldown();
      }
      
      if (PlayerController.Instance.isInputCooldown) return;
      if (ask.isAsk) {
        if (Input.GetKeyDown(yesKey)) NextOrCloseOrYes();
        else if (Input.GetKeyDown(noKey)) PreviousOrNo();
        
      } else {
        if (Input.GetKeyDown(nextKey)) {
          NextOrCloseOrYes();
        }
      }
    }

    private void Close(bool? btn = null) {
      panel.SetActive(false);
      isEnabled = false;
      if (btn.HasValue) callback?.Invoke(btn.Value);
    }

    public void ShowDialogue(DialogueData dialogue, [CanBeNull] Action<bool> callback = null, bool isAsk = false) {
      if (isEnabled) return;
      isEnabled = true;
      panel.gameObject.SetActive(true);
      ResetUI(dialogue.speaker, isAsk);
      ask.isAsk = isAsk;
      curText = dialogue.text.ToCharArray();
      isMulti = false;
      this.callback = callback;
      StartWrite();
    }

    public void ShowDialogues(DialogueData[] dialogues, [CanBeNull] Action<bool> callback = null) {
      if (isEnabled) return;
      isEnabled = true;
      panel.gameObject.SetActive(true);
      isMulti = true;
      multiList = dialogues;
      multIndex = 0;
      this.callback = callback;
      SetMultiDialogue();
    }

    public void Ask(DialogueData dialogue, Action<bool> callback, string yesText = "예", string noText = "아니요") {
      ask.yes = yesText;
      ask.no = noText;
      ShowDialogue(dialogue, callback, true);
    }

    private void SetMultiDialogue() {
      var dialogue = multiList[multIndex];
      ResetUI(dialogue.speaker);
      curText = dialogue.text.ToCharArray();
      StartWrite();
    }

    private void StartWrite() {
      curTextIndex = 0;
      isWriting = true;
      PlayerController.Instance.EnableInputCooldown();
      InvokeRepeating("Write", 0f, writeDelay);
    }

    private void Write() {
      if (curText.Length <= curTextIndex) {
        EndWriting();
        return;
      }

      text.text += curText[curTextIndex];
      curTextIndex++;
    }

    private void EndWriting() {
      isWriting = false;
      CancelInvoke("Write");
      ShowBtns();
    }

    private void ShowBtns() {
      SetBtnColor(ask.isAsk, yesColor, noColor, defaultColor);
      Debug.Log(ask.isAsk);
      if (ask.isAsk) {
        nextBtnText.text = ask.yes;
        previousBtnText.text = ask.no;
        previousBtn.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);
      } else {
        if (isMulti) {
          nextBtnText.text = (multIndex < multiList.Length - 1 ? "다음" : "확인");
          previousBtn.gameObject.SetActive(multIndex > 0);
          nextBtn.gameObject.SetActive(true);
        } else {
          nextBtnText.text = "확인";
          nextBtn.gameObject.SetActive(true);
        }
      }
    }

    private void HideBtns() {
      nextBtn.gameObject.SetActive(false);
      previousBtn.gameObject.SetActive(false);
    }

    public void OnNextButtonClick() {
      NextOrCloseOrYes();
    }

    public void OnPreviousButtonClick() {
      PreviousOrNo();
    }

    private void NextOrCloseOrYes() {
      if (ask.isAsk) {
        Close(true);
      } else {
        if (isMulti && multIndex < multiList.Length - 1) {
          multIndex++;
          HideBtns();
          SetMultiDialogue();
        } else {
          Close(true);
        }
      }

      PlayerController.Instance.EnableInputCooldown();
    }

    private void PreviousOrNo() {
      if (ask.isAsk) {
        Close(false);
      } else {
        if (multIndex <= 0) return;
        multIndex--;
        HideBtns();
        SetMultiDialogue();
      }

      PlayerController.Instance.EnableInputCooldown();
    }

    private void SetBtnColor(bool isAsk, Color yesColor, Color noColor, Color defColor) {
      if (isAsk) {
        nextBtnImg.color = yesColor;
        previousBtnImg.color = noColor;
      } else {
        nextBtnImg.color = defColor;
        previousBtnImg.color = defColor;
      }
    }
  }
}