using UnityEngine;
using Vuforia;

public class ShowObjectInFrontOfCamera : MonoBehaviour
{
    public GameObject objectToShow;              // 要顯示的物件
    public float distanceInFront = 500f;           // 距離相機的距離
    public Transform vrCamera;                   // VR 相機（例如 OVRCameraRig/CenterEyeAnchor）

    private ObserverBehaviour observerBehaviour;
    private Rigidbody rb;
    public static ShowObjectInFrontOfCamera Instance
    {
        get; private set;
    }
    public int objectID;
    void Awake()
    {
        // 確保只有一個實例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切換場景時不會被銷毀（視情況而定）
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged += OnTargetStatusChanged;
        }

        if (objectToShow != null)
        {
            objectToShow.SetActive(false); // 預設隱藏
        }
    }

    void OnDestroy()
    {
        if (observerBehaviour != null)
        {
            observerBehaviour.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED ||
                         status.Status == Status.EXTENDED_TRACKED;

        if (isTracked && vrCamera != null && objectToShow != null)
        {
            if (behaviour.TargetName == "ArtworkFrame")
            {
                objectID = 0;
                Debug.LogWarning("第一幅:"+objectID);
            }
            else if (behaviour.TargetName == "qrcode_rgb1")
            {
                objectID = 10;
                Debug.Log("第二幅");
            }
            // 將物件移動到相機正前方 distanceInFront 單位
            Vector3 forwardPosition = vrCamera.position + vrCamera.forward * distanceInFront;
            objectToShow.transform.position = forwardPosition;
            //objectToShow.transform.rotation = Quaternion.LookRotation(vrCamera.forward); // 讓它面對相機前方
            objectToShow.SetActive(true);
            Debug.LogWarning("------place object");
            rb = objectToShow.AddComponent<Rigidbody>();
            // 完全凍結位置和旋轉
            rb.constraints = RigidbodyConstraints.FreezeAll;
            // 禁用物理重力影響
            rb.useGravity = false;

            // 確保物件不會被物理系統移動
            rb.isKinematic = true;
        }
        else if (objectToShow != null)
        {
            objectToShow.SetActive(false);
        }
    }
    void Update()
    {
        // 鎖定物件的旋轉，保持原始方向
        objectToShow.transform.rotation = Quaternion.identity;

        // 或者鎖定特定軸向
        // transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }
}