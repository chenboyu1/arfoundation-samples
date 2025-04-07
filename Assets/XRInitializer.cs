using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections;

public class XRInitializer : MonoBehaviour
{
    private IEnumerator Start()
    {
        Debug.Log(" ���b��l�� XR...");

        // �T�O XR Manager �s�b
        XRGeneralSettings xrSettings = XRGeneralSettings.Instance;
        if (xrSettings == null || xrSettings.Manager == null)
        {
            Debug.LogError(" XRGeneralSettings.Instance �� XR Manager �� null�A���ˬd XR �]�w�I");
            yield break;
        }

        // ��l�� XR Loader
        yield return xrSettings.Manager.InitializeLoader();

        if (xrSettings.Manager.activeLoader == null)
        {
            Debug.LogError(" XR Loader ���J���ѡI");
            yield break;
        }

        // �Ұ� XR
        Debug.Log(" XR ��l�Ƨ����A�}�l XR�I");
        xrSettings.Manager.StartSubsystems();
    }
}
