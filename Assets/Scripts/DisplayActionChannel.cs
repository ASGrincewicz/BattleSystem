using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Channels/Display Action Channel")]
public class DisplayActionChannel : ScriptableObject
{
    public UnityEvent<string> OnDisplayAction;

    public void RaiseDisplayActionEvent(string actionTaken)
    {
        if (OnDisplayAction == null)
            return;
        OnDisplayAction.Invoke(actionTaken);
    }
}
