using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public void LoadScene(string scene)
    {
        Debug.Log("Loading scene " + scene);
        TransitionManager.Instance.LoadScene(scene);
        AudioManager.instance.Play("Start game v2");
    }
}
