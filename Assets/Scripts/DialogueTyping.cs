using System.Collections;
using TMPro;
using UnityEngine;
using Veganimus.BattleSystem;
///<summary>
///@author
///Aaron Grincewicz
///</summary>

public class DialogueTyping : MonoBehaviour
{
    [SerializeField] private TMP_Text _screenText;
    public bool isAutoTurnOn;
    [Header("Setting")]
    [SerializeField] private Chapter _chapter;
    [Multiline(5)]
    [SerializeField] private string _typedText;
    public float DelayTime { get; set; } = 0.0251f;
    public float PageDelayTime { get; set; }
    //[SerializeField] private AudioClip _typingClip;
    private bool _isTyping;
    private int _index = 0;
    private int _currentPage = 0;
    private int _textLength;
    private string _textToType;
    private char _characterToType;
    public WaitForSeconds PageTurnDelay { get; set; }
    public WaitForSeconds CharacterDelay { get; set; }

    private void OnEnable()
    {
        PageTurnDelay = new WaitForSeconds(PageDelayTime);
        CharacterDelay = new WaitForSeconds(DelayTime);
    }
    private void Start() => StartTyping(_chapter, _screenText);

    public void StartTyping(Chapter chapter, TMP_Text screenText)
    {
        _chapter = ScriptableObject.CreateInstance<Chapter>();
        foreach (var page in chapter.chapterPages)
        {
            _chapter.chapterPages.Add(page);
        }
        _chapter.chapterPages.Sort();
        _textToType = _chapter.chapterPages[_currentPage].content;
        _screenText = screenText;
        _textLength = _chapter.chapterPages[_currentPage].content.Length;
        StartCoroutine(TypeCharacter(_textToType));
    }

    public void TurnPage(string direction)
    {
        if (!_isTyping)
        {
            if (direction == "next" && _currentPage < _chapter.chapterPages.Count - 1)
                _textToType = _chapter.chapterPages[_currentPage++].content;

            else if (direction == "previous" && _currentPage != 0)
                _textToType = _chapter.chapterPages[_currentPage--].content;

            _screenText.text = string.Empty;
            _typedText = string.Empty;
            _textLength = _chapter.chapterPages[_currentPage].content.Length;
            StartCoroutine(TypeCharacter(_textToType));
        }
    }

    private IEnumerator TypeCharacter(string textToType)
    {
        _textToType = textToType;
        _isTyping = true;

        if (_textLength > _typedText.Length)
        {
            for (int index = 0; index < _textLength; index++)
            {
                _characterToType = _chapter.chapterPages[_currentPage].content[index];
                _typedText = $"{_typedText += _characterToType}";
                _screenText.text = $"{_typedText}";
                //if (DelayTime > 0.025f)
                //{
                //    TypingUI.Instance.audioSource.pitch = UnityEngine.Random.Range(0.75f, 1.0f);
                //    TypingUI.Instance.audioSource.PlayOneShot(_typingClip);
                //}
                yield return CharacterDelay;
            }
        }

        if (isAutoTurnOn)
        {
            if (_currentPage < _chapter.chapterPages.Count - 1)
            {
                yield return PageTurnDelay;
                _isTyping = false;
                TurnPage("next");
            }
            else
                _isTyping = false;
        }
        else
            _isTyping = false;
    }
}