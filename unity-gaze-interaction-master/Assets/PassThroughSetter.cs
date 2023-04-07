using Unity.XR.PXR;
using UnityEngine;

public class PassThroughSetter : MonoBehaviour
{
    void Start()
    {
        // �V�[�X���[�ݒ�
        PXR_Boundary.EnableSeeThroughManual(true);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        // Pause����̕��A���A�ēx�V�[�X���[�ɂ���
        if (!pauseStatus)
        {
            PXR_Boundary.EnableSeeThroughManual(true);
        }
    }
}

