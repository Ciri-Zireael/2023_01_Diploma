using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UserInput : MonoBehaviour, Input.UserInput.ISessionActions
{
    Input.UserInput userInput;
    SlideHolder[] slideHolders;
    FloatingDialog confirmationDialog;
    [SerializeField] AnalyticsCollector analyticsCollector;

    void Awake()
    {
        SceneManager.sceneLoaded += SetActionMap;
    }

    void Start()
    {
        slideHolders = FindObjectsOfType<SlideHolder>();
    }
	
    void SetActionMap(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Lobby") return;

        userInput = new Input.UserInput();
        userInput.Session.SetCallbacks(this);
        userInput.Session.Enable();

        Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        confirmationDialog = canvases.FirstOrDefault(canvas => canvas.gameObject.name == "Confirmation Dialog")?.GetComponent<FloatingDialog>();
    }

    public void OnSlidePrev(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        foreach (var slides in slideHolders)
        {
            slides.PrevSlide();
        }
    }

    public void OnSlideNext(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        foreach (var slides in slideHolders)
        {
            slides.NextSlide();
        }
    }

    public void OnCallMenu(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed) return;
        confirmationDialog.Show();
    }

    public void GoToLobby()
    {
        if (analyticsCollector != null)
        {
            if (PlayerPrefs.GetInt("STT") == 1)
            {
                analyticsCollector.SaveAnalytics();
                Debug.Log("All good");
            }
        }
        SceneManager.LoadScene("Lobby");
    }

    public void ExitDialog()
    {
        confirmationDialog.Hide();
    }
	
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= SetActionMap;
        userInput?.Session.Disable();
    }
}