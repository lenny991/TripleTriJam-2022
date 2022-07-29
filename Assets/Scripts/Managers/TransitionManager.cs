using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : DDOLSingleton<TransitionManager>
{
    Image image;
    Canvas canvas;

    private void Start()
    {
        image = GetComponentInChildren<Image>();
        canvas = GetComponentInChildren<Canvas>();
    }

    Tween loading;

    public void LoadScene(string scene)
    {
        if (loading.IsActive())
            return;
        canvas.sortingOrder = 32767;
        image.color = Color.clear;
        loading = image.DOFade(1, 1);
        loading.onComplete += () =>
        {
            SceneManager.LoadScene(scene);
            loading = image.DOFade(0, 1);
            loading.onComplete += () => canvas.sortingOrder = -32767;
        };
    }
}
