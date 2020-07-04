using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Interface : MonoBehaviour
{
    public Text HelthText;
    public Text FatigueText;
    public Button ReplayButton;
    public GameObject LosePanel;
    [SerializeField] private PlayerController _player;
    private Coroutine _playerTiresCoroutine;
    private Coroutine _recoveryCoroutine;
    [SerializeField] private MonoBehaviour[] _scripts; // Компоненты которые отключатьс я при проигрыше

    private void Start()
    {
        ReplayButton.onClick.AddListener(Replay);
    }

    public void StartTires(float tick, float time)
    {
        _playerTiresCoroutine = StartCoroutine(PlayerTires(tick,time));
    }

    public void StopTires()
    {
        StopCoroutine(_playerTiresCoroutine);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartRecovory()
    {
        _recoveryCoroutine = StartCoroutine(FatigueRecovery(1f, 0.5f));
    }

    public void StopRecovory()
    {
        StopCoroutine(_recoveryCoroutine);
    }

    public void InvokeRecovery(float time)
    {
        Invoke("StartRecovory", time);
    }

    public void DispaleyStatsFatigue(Text text , float num)
    {
        text.text = num.ToString();
    }

    public void DispaleyStatsHealth(float num)
    {
        HelthText.text = num.ToString();
    }

    // Показывает панель проигрыша
    public void ShowLoseTable()
    {
        foreach (var scripts in _scripts)
            scripts.enabled = false;

        _player.enabled = false;
        Cursor.visible = true;
        LosePanel.SetActive(true);
    }

    public IEnumerator PlayerTires(float tick, float time)
    {
        while (true)
        {
            if(_player.Fatigue <= 1)
            {          
                _player.MoveSpeed = _player.DefaulSpeed;
                InvokeRecovery(2f);
                StopTires();
            }

            _player.Fatigue -= tick;
            DispaleyStatsFatigue(FatigueText, _player.Fatigue);

            yield return new WaitForSeconds(time);
        }

    }


    public IEnumerator FatigueRecovery(float tick, float time)
    {
        while (true)
        {
            if(_player.Fatigue >= 100)
            {
                break;
            }

            _player.Fatigue += tick;
            DispaleyStatsFatigue(FatigueText, _player.Fatigue);

            yield return new WaitForSeconds(time);
        }
    }
}
