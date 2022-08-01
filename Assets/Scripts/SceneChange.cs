using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        AudioManager.instance.Play("Start game");
        TransitionManager.Instance.LoadScene(scene);
    }

    public void ExitAplication()
    {
        Application.Quit();
    }
}
