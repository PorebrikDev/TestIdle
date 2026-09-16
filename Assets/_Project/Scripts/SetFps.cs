using UnityEngine;

public class SetFps : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
}
