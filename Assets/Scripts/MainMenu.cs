using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _exampleDropdown;
    [SerializeField] private Sprite _exampleScreenShot;
    [SerializeField] private List<Sprite> _screenShots;
    [SerializeField] private Image _exampleImagePrefab;
    private WaitForSeconds _revertDelay;

    private void OnEnable()
    {
        _exampleDropdown.onValueChanged.AddListener(ChangeScreenShot);
        _revertDelay = new WaitForSeconds(2f);
    }
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
#if PLATFORM_WEBGL
        Application.OpenURL("https://veganimus.itch.io/turn-based-battle-system-demo");
#endif
    }
    public void LinkedInButton() => Application.OpenURL("http://www.linkedin.com/in/aarongrincewicz");

    public void ChangeScreenShot(int value)
    {
        value = _exampleDropdown.value;
        _exampleScreenShot = _screenShots[value];
        _exampleImagePrefab.sprite = _exampleScreenShot;
        if (value > 0)
            StartCoroutine(RevertDropdownValue());
    }
    private IEnumerator RevertDropdownValue()
    {
        yield return _revertDelay;
        _exampleDropdown.value = 0;
        _exampleDropdown.RefreshShownValue();
    }
}