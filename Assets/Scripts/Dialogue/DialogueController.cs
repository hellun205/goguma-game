using System;
using System.Collections;
using TMPro;
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

    [SerializeField]
    private Button previousBtn;

    [Header("Dialogue")]
    public float writeDelay = 0.05f;

    public KeyCode nextKey = KeyCode.Space;

    public bool isEnabled { get; private set; }

    public bool skipable = true;

    private char[] curText;
    private int curTextIndex;
    private bool isWriting;
    private bool isMulti;
    private DialogueData[] multiList;
    private int multIndex;

    private void Awake() {
      if (Instance == null) Instance = this;
      else Destroy(gameObject);
      DontDestroyOnLoad(gameObject);

      ResetUI();
      Close();
    }

    private void ResetUI(Speaker speaker = null) {
      speakerName.text = string.Empty;
      text.text = string.Empty;
      speakerName.alignment = TextAlignmentOptions.TopLeft;
      text.alignment = TextAlignmentOptions.TopLeft;
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
      if (isEnabled && Input.GetKeyDown(nextKey)) {
        if (isWriting) {
          if (!skipable) return;
          for (var i = curTextIndex; i < curText.Length; i++) {
            text.text += curText[i];
          }
          EndWriting();
        } else {
          NextOrClose();
        }
      }
    }

    private void Close() {
      panel.SetActive(false);
      isEnabled = false;
    }

    public void ShowDialogue(DialogueData dialogue) {
      if (isEnabled) return;
      isEnabled = true;
      panel.gameObject.SetActive(true);
      ResetUI(dialogue.speaker);
      curText = dialogue.text.ToCharArray();
      curTextIndex = 0;
      isMulti = false;
      isWriting = true;
      InvokeRepeating("Write", 0f, writeDelay);
    }

    public void ShowDialogues(DialogueData[] dialogues) {
      if (isEnabled) return;
      isEnabled = true;
      panel.gameObject.SetActive(true);
      isMulti = true;
      multiList = dialogues;
      multIndex = 0;
      SetMultiDialogue();
    }

    private void SetMultiDialogue() {
      var dialogue = multiList[multIndex];
      ResetUI(dialogue.speaker);
      curText = dialogue.text.ToCharArray();
      curTextIndex = 0;
      isWriting = true;
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
      if (isMulti) {
        nextBtnText.text = (multIndex < multiList.Length - 1 ? "다음" : "확인");
        previousBtn.gameObject.SetActive(multIndex > 0);
        nextBtn.gameObject.SetActive(true);
      } else {
        nextBtnText.text = "확인";
        nextBtn.gameObject.SetActive(true);
      }
    }

    private void HideBtns() {
      nextBtn.gameObject.SetActive(false);
      previousBtn.gameObject.SetActive(false);
    }

    public void OnNextButtonClick() {
      NextOrClose();
    }

    public void OnPreviousButtonClick() {
      Previous();
    }

    private void NextOrClose() {
      if (isMulti && multIndex < multiList.Length - 1) {
        multIndex++;
        HideBtns();
        SetMultiDialogue();
      } else {
        Close();
      }
    }

    private void Previous() {
      if (multIndex <= 0) return;
      multIndex--;
      HideBtns();
      SetMultiDialogue();
    }
  }
}