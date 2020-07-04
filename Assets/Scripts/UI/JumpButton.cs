using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{

    [SerializeField] private PlayerController _playerController;

    public void JumpPress()
    {
        _playerController.IsJump = true;
    }

}
