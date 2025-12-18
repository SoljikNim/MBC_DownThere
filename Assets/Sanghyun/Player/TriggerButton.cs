using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerButton : MonoBehaviour
{
    public Color deault;
    public Color hover;
    public TextMeshPro text;
    public string moveSceneName;

    public void ChangeColor(bool _isHover)
    {
        if (_isHover)
            text.color = hover;
        else
            text.color = deault;
    }

    public void MoveScene()
    {
        SceneManager.LoadScene(moveSceneName);
    }
}
