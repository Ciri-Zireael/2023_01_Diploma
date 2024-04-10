using UnityEngine;
using UnityEngine.InputSystem;

public class TerribleInputController : MonoBehaviour
{
    [SerializeField] InputActionReference prevSlideReference;
    [SerializeField] InputActionReference nextSlideReference;
    [SerializeField] SlideHolder slideHolder;

    // this is so bad but my brain isn't braining I'll do it properly later
    
    private void Awake()
    {
        prevSlideReference.action.started += prevSlide;
        nextSlideReference.action.started += nextSlide;
    }

    void prevSlide(InputAction.CallbackContext context)
    {
        slideHolder.PrevSlide();
    }
    
    void nextSlide(InputAction.CallbackContext context)
    {
        slideHolder.NextSlide();
    }
}
