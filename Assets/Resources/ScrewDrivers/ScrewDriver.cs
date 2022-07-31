using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create a screwdriver")]
public class ScrewDriver : ScriptableObject
{
    public Sprite sprite;
    public int waveUnlock;
    public Sprite spriteUI;
    public KeyCode keyCode;
}
