using UnityEngine;

public class FloatingDialog : MonoBehaviour
{
    [SerializeField] float distanceFromCamera = 1.5f;
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
        transform.position = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
