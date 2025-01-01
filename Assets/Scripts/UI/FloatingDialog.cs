using UnityEngine;


public class FloatingDialog : MonoBehaviour
{
    [SerializeField] float distanceFromCamera = 1.5f;
    [SerializeField] DarknessOverlay darknessOverlay;
    Transform cameraTransform;

    void Awake()
    {
        cameraTransform = FindObjectOfType<Camera>().transform;
    }

    void Update()
    {
        FollowCamera();
    }

    void FollowCamera()
    {
        transform.position = Vector3.Lerp(transform.position, cameraTransform.position + cameraTransform.forward * distanceFromCamera, 0.1f);
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0);
    }



    public void Show()
    {
        if (isActiveAndEnabled) return;
        gameObject.SetActive(true);
        darknessOverlay.On();
    }

    public void Hide()
    {
        if (!isActiveAndEnabled) return;
        darknessOverlay.Off();
        gameObject.SetActive(false);
    }
}
