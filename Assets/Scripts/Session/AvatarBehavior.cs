using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AvatarBehavior : MonoBehaviour
{
    Animator animator;
    bool isPlayingAnimation;
    public double boredom = 0;
    [SerializeField] string[] randomAnimations = {"Talking right"};
    [SerializeField] double boredomTickAmount = 0.0001;
    [SerializeField] double boredomMultiplier = 0.005;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        boredom += boredomTickAmount * boredomMultiplier;

        if (boredom > 0.5 && Random.value < boredom)
        {
            PlayAnimation(randomAnimations[Random.Range(0, randomAnimations.Length)]);
        }
    }

    public void PlayAnimation(string animation)
    {
        if (isPlayingAnimation) return;

        StartCoroutine(PlayAnimationCoroutine(animation));
    }
    
    IEnumerator PlayAnimationCoroutine(string animation)
    {
        isPlayingAnimation = true;
        
        animator.Play(animation);
        AnimatorStateInfo animationState = animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(animationState.length);

        boredom = 0;
        isPlayingAnimation = false;
    }
}
