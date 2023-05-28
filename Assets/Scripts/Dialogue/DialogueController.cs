using System;
using System.Collections;
using Entity.Player;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Dialogue
{
  /// <summary>
  /// 플레이어와, Npc 등 대화 기능에 관련된 컨트롤러 입니다. (싱글톤)
  /// </summary>
  public class DialogueController : MonoBehaviour
  {
    /// <summary>
    /// 현재 인스턴스를 가져옵니다. (싱글톤)
    /// </summary>
    public static DialogueController Instance { get; private set; }

    /// <summary>
    /// 대화 창 부모 오브젝트
    /// </summary>
    [Header("UI Object")]
    [SerializeField]
    private GameObject panel;

    /// <summary>
    /// 왼쪽 아바타 이미지 컴포넌트
    /// </summary>
    [SerializeField]
    private Image leftAvatar;

    /// <summary>
    /// 오른쪽 아바타 이미지 컴포넌트
    /// </summary>
    [SerializeField]
    private Image rightAvatar;

    /// <summary>
    /// 화자 이름 텍스트 컴포넌트
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI speakerName;

    /// <summary>
    /// 대화 텍스트 컴포넌트
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI text;

    /// <summary>
    /// 긍정 버튼 컴포넌트
    /// </summary>
    [SerializeField]
    private Button nextBtn;

    /// <summary>
    /// 긍정 버튼의 텍스트 컴포넌트
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI nextBtnText;

    /// <summary>
    /// 긍정 버튼의 이미지 컴포넌트
    /// </summary>
    private Image nextBtnImg;

    /// <summary>
    /// 부정 버튼 컴포넌트
    /// </summary>
    [SerializeField]
    private Button previousBtn;

    /// <summary>
    /// 부정 버튼의 텍스트 컴포넌트
    /// </summary>
    [SerializeField]
    private TextMeshProUGUI previousBtnText;

    /// <summary>
    /// 부정 버튼의 이미지 컴포넌트
    /// </summary>
    private Image previousBtnImg;

    /// <summary>
    /// 대화의 속도
    /// </summary>
    [Header("Dialogue")]
    public float writeDelay = 0.05f;

    /// <summary>
    /// 다음 / 확인 / 스킵 키
    /// </summary>
    public KeyCode nextKey = KeyCode.Space;

    /// <summary>
    /// 현재 대화 창이 열려 있는지 여부
    /// </summary>
    public bool isEnabled { get; private set; }

    /// <summary>
    /// 스킵 가능 여부
    /// </summary>
    public bool skipable = true;

    /// <summary>
    /// 예 버튼 색
    /// </summary>
    [Header("Ask Setting")]
    public Color yesColor;

    /// <summary>
    /// 아니요 버튼 색
    /// </summary>
    public Color noColor;

    /// <summary>
    /// 일반 버튼 색
    /// </summary>
    public Color defaultColor;

    /// <summary>
    /// 예 키 (질문)
    /// </summary>
    public KeyCode yesKey = KeyCode.Z;

    /// <summary>
    /// 아니요 키 (질문)
    /// </summary>
    public KeyCode noKey = KeyCode.X;

    /// <summary>
    /// 현재 대화의 글자 배열
    /// </summary>
    private char[] curText;

    /// <summary>
    /// 현재 대화의 글자 인덱스
    /// </summary>
    private int curTextIndex;

    /// <summary>
    /// 현재 글자를 적고 있는지 여부
    /// </summary>
    private bool isWriting;

    /// <summary>
    /// 여러 개의 대화를 실행하고 있는지 여부
    /// </summary>
    private bool isMulti;

    /// <summary>
    /// 여러 개의 대화 데이터 리스트
    /// </summary>
    private DialogueData[] multiList;

    /// <summary>
    /// 여러 개의 대화 중 현재 대화 번호
    /// </summary>
    private int multIndex;

    /// <summary>
    /// 질문 형식 대화 데이터
    /// <param name="isAsk">질문 형식인지 여부</param>
    /// <param name="yes">예 버튼 텍스트</param>
    /// <param name="no">아니요 버튼 텍스트</param>
    /// </summary>
    private (bool isAsk, string yes, string no) ask;

    /// <summary>
    /// 대화가 종료된 후 콜백합니다.
    /// </summary>
    private Action<bool> callback;

    private void Awake()
    {
      if (Instance == null) Instance = this;
      else Destroy(this);
      // DontDestroyOnLoad(gameObject);
      nextBtnImg = nextBtn.GetComponent<Image>();
      previousBtnImg = previousBtn.GetComponent<Image>();

      ResetUI();
      Close();
    }

    /// <summary>
    /// 대화 창을 초기화 합니다.
    /// </summary>
    /// <param name="speaker">현재 말하는 사람의 정보</param>
    /// <param name="isAsk">대화 형식이 질문 형식인지 여부</param>
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

    /// <summary>
    /// 키 입력을 확인하고, 키에 따라 함수 실행
    /// </summary>
    private void Update()
    {
      if (!isEnabled || PlayerController.Instance.isInputCooldown)
        return;

      // 스킵
      if (Input.GetKeyDown(nextKey) && isWriting)
      {
        if (!skipable)
          return;

        for (var i = curTextIndex; i < curText.Length; i++)
          text.text += curText[i];

        EndWriting();
        PlayerController.Instance.EnableInputCooldown();
      }

      if (PlayerController.Instance.isInputCooldown)
        return;

      if (ask.isAsk)
      {
        if (Input.GetKeyDown(yesKey))
          NextOrCloseOrYes();
        else if (Input.GetKeyDown(noKey))
          PreviousOrNo();

      }
      else
      {
        // 다음 / 확인
        if (Input.GetKeyDown(nextKey))
          NextOrCloseOrYes();
      }
    }

    /// <summary>
    /// 대화 창을 닫습니다.
    /// </summary>
    /// <param name="btn"></param>
    private void Close(bool? btn = null)
    {
      panel.SetActive(false);
      isEnabled = false;

      if (btn.HasValue)
        callback?.Invoke(btn.Value);
    }

    /// <summary>
    /// 대화를 시작합니다.
    /// </summary>
    /// <param name="dialogue">대화 데이터</param>
    /// <param name="callback">대화 종료 후 콜백(참 반환)</param>
    /// <param name="isAsk">대화 형식이 질문 형식인지 여부</param>
    public void ShowDialogue(DialogueData dialogue, [CanBeNull] Action<bool> callback = null, bool isAsk = false)
    {
      if (isEnabled)
        return;

      isEnabled = true;
      panel.gameObject.SetActive(true);
      ResetUI(dialogue.speaker, isAsk);
      ask.isAsk = isAsk;
      curText = dialogue.text.ToCharArray();
      isMulti = false;
      this.callback = callback;
      StartWrite();
    }

    /// <summary>
    /// 대화들을 시작합니다. (묶음)
    /// </summary>
    /// <param name="dialogues">대화 데이터 묶음</param>
    /// <param name="callback">대화 종료 후 콜백(참 반환)</param>
    public void ShowDialogues(DialogueData[] dialogues, [CanBeNull] Action<bool> callback = null)
    {
      if (isEnabled)
        return;

      isEnabled = true;
      panel.gameObject.SetActive(true);
      isMulti = true;
      multiList = dialogues;
      multIndex = 0;
      this.callback = callback;
      SetMultiDialogue();
    }

    /// <summary>
    /// 질문 형태의 대화를 시작합니다.
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

    /// <summary>
    /// 다음 대화를 설정하고, 글자를 적기 시작합니다.
    /// </summary>
    private void SetMultiDialogue()
    {
      var dialogue = multiList[multIndex];
      ResetUI(dialogue.speaker);
      curText = dialogue.text.ToCharArray();
      StartWrite();
    }

    /// <summary>
    /// 글자를 적기 시작합니다.
    /// </summary>
    private void StartWrite()
    {
      curTextIndex = 0;
      isWriting = true;
      PlayerController.Instance.EnableInputCooldown();
      InvokeRepeating("Write", 0f, writeDelay);
    }

    /// <summary>
    /// 다음 글자를 적습니다.
    /// </summary>
    private void Write()
    {
      if (curText.Length <= curTextIndex)
      {
        EndWriting();
        return;
      }

      text.text += curText[curTextIndex];
      curTextIndex++;
    }

    /// <summary>
    /// 대화를 중단하고, 버튼들을 활성화 합니다.
    /// </summary>
    private void EndWriting()
    {
      isWriting = false;
      CancelInvoke("Write");
      ShowBtns();
    }

    /// <summary>
    /// 대화 창의 버튼들을 활성화하고, 형식에 따라 버튼의 모양새를 바꿉니다.
    /// </summary>
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

    /// <summary>
    /// 대화 창의 버튼들을 비활성화 합니다.
    /// </summary>
    private void HideBtns()
    {
      nextBtn.gameObject.SetActive(false);
      previousBtn.gameObject.SetActive(false);
    }

    public void OnNextButtonClick() => NextOrCloseOrYes();

    public void OnPreviousButtonClick() => PreviousOrNo();

    /// <summary>
    /// 다음, 확인 또는 예(질문)
    /// </summary>
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

    /// <summary>
    /// 이전 또는 아니요(질문)
    /// </summary>
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

    /// <summary>
    /// 대화 창의 버튼들의 색을 변경합니다.
    /// </summary>
    /// <param name="isAsk">대화 형식이 질문 형식인지의 여부</param>
    /// <param name="yesColor">예 버튼 색</param>
    /// <param name="noColor">아니요 버튼 색</param>
    /// <param name="defColor">일반 버튼 색</param>
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
