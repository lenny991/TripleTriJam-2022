using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public string scene;
    public void LoadScene()
    {
        TransitionManager.Instance.LoadScene(scene);
    }
}
