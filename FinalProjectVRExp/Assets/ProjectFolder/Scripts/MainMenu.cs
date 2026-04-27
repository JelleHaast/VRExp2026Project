using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private AsyncOperation op;

    void Start()
    {
        op = SceneManager.LoadSceneAsync("Level1");
        op.allowSceneActivation = false;
    }

    public void StartButton()
    {
        if (op != null && op.progress >= 0.9f)
        {
            op.allowSceneActivation = true;
        }
        else
        {
            StartCoroutine(WaitThenActivate());
        }
    }

    IEnumerator WaitThenActivate()
    {
        while (op.progress < 0.9f)
        {
            yield return null;
        }
        op.allowSceneActivation = true;
    }
}