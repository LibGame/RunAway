using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public Animation ExitAnimation;
    public GameObject ExitScreen;

    public void StartExitAnimation()
    {
        ExitScreen.SetActive(true);
        ExitAnimation.Play("EndScreen");
    }

}
