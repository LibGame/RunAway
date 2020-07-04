using UnityEngine;

public class MenuModel : MonoBehaviour
{

    public float SpeedRotation;

    private void Update()
    {
        transform.Rotate(0, SpeedRotation * Time.deltaTime, 0);
    }
}
