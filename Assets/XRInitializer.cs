using UnityEngine;
using UnityEngine.XR.Management;
using System.Collections;

public class XRInitializer : MonoBehaviour
{
    private IEnumerator Start()
    {
        Debug.Log(" 正在初始化 XR...");

        // 確保 XR Manager 存在
        XRGeneralSettings xrSettings = XRGeneralSettings.Instance;
        if (xrSettings == null || xrSettings.Manager == null)
        {
            Debug.LogError(" XRGeneralSettings.Instance 或 XR Manager 為 null，請檢查 XR 設定！");
            yield break;
        }

        // 初始化 XR Loader
        yield return xrSettings.Manager.InitializeLoader();

        if (xrSettings.Manager.activeLoader == null)
        {
            Debug.LogError(" XR Loader 載入失敗！");
            yield break;
        }

        // 啟動 XR
        Debug.Log(" XR 初始化完成，開始 XR！");
        xrSettings.Manager.StartSubsystems();
    }
}
