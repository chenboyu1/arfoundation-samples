using UnityEngine;
using Vuforia;

public class ShowObjectInFrontOfCamera1 : MonoBehaviour
{
    public GameObject objectToShow;              // 要顯示的物件
    public float distanceInFront = 500f;           // 距離相機的距離
    public Transform vrCamera;                   // VR 相機（例如 OVRCameraRig/CenterEyeAnchor）

    private ObserverBehaviour observerBehaviour;
    private Rigidbody rb;
    public GameObject[] specialSubObjects;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        if (objectToShow != null)
        {
            objectToShow.SetActive(false);
            for (int i = 0; i < 1; i++)
                specialSubObjects[i].SetActive(false); // 預設隱藏
        }
    }

    void OnDestroy()
    {
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
            Debug.LogWarning("status: ");
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED ||
                         status.Status == Status.EXTENDED_TRACKED;
        Debug.LogWarning("status: " + status.Status);

        if (isTracked && vrCamera != null && objectToShow != null)
        {
            ShowObjectInFrontOfCamera.Instance.objectID = 20;
            //if (!objectToShow.activeSelf)
            //{
            // 將物件移動到相機正前方 distanceInFront 單位
            Vector3 forwardPosition = vrCamera.position + vrCamera.forward * distanceInFront;
            objectToShow.transform.position = forwardPosition;
            //objectToShow.transform.rotation = Quaternion.LookRotation(vrCamera.forward); // 讓它面對相機前方
            objectToShow.SetActive(true);
            for (int i = 0; i < 1; i++)
                specialSubObjects[i].SetActive(true);
            Debug.LogWarning("------place object");
            rb = objectToShow.AddComponent<Rigidbody>();
            // 完全凍結位置和旋轉
            rb.constraints = RigidbodyConstraints.FreezeAll;
            // 禁用物理重力影響
            rb.useGravity = false;

            // 確保物件不會被物理系統移動
            rb.isKinematic = true;
            //}
        }
        else
        {
            Debug.LogWarning("displace object");
            /*for (int i = 0; i < 5; i++)
                specialSubObjects[i].SetActive(false);*/
        }
    }
}