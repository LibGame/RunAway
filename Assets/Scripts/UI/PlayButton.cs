using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayButton : MonoBehaviour
{
    public UnityEvent ParticlePlay;
    public Animation ButtonAnimation;

    public void StartPlayAnimation()
    {
        ButtonAnimation.Play("ButtonPlay");
        ParticlePlay.Invoke();
    }

    public void LoadGameScence()
    {
        SceneManager.LoadScene("GameScene");
    }
}
