using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudienceManager : MonoBehaviour
{

	[SerializeField] private GameObject[] avatars;
    [SerializeField] private RuntimeAnimatorController[] avatarAnimators;
    
    [SerializeField] private int numberOfSeatsToFill;
    
    [SerializeField] private Vector3 avatarOffset = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 avatarRotation = new Vector3(0, 0, 0);
    [SerializeField] private float avatarScale = 1;
    
    [SerializeField] private float neighborDetectionRadius = 2.0f;
    
    private List<Vector3> _allSittingPositions;
    private Dictionary<Vector3, GameObject> _seatToAvatarMap;
    private Dictionary<GameObject, List<GameObject>> _neighborMap;

    void Start()
    {
        DetectChairPositions();
        PlaceAvatars(numberOfSeatsToFill);
        DetectNeighbors();
        InvokeRepeating(nameof(TriggerRandomInteractions), 2.0f, 10.0f);
    }
    
    void DetectChairPositions()
    {
        _allSittingPositions = new List<Vector3>();
        GameObject[] chairs = GameObject.FindGameObjectsWithTag("Chair");
        foreach (var chair in chairs)
        {
            _allSittingPositions.Add(chair.transform.position);
        }
    }

    void PlaceAvatars(int numberOfAvatarsToPlace)
    {
        if (numberOfAvatarsToPlace > _allSittingPositions.Count)
        {
            throw new Exception("Not enough sitting spaces for given number of avatars");
        }

        _seatToAvatarMap = new Dictionary<Vector3, GameObject>();
        IEnumerable<Vector3> selectedSeats = SelectSeats(numberOfAvatarsToPlace);
        
        int idx = 0;
        
        foreach (var seatPosition in selectedSeats.ToList())
        {
            GameObject avatarInstance = Instantiate(avatars[idx], 
                new Vector3(seatPosition.x, 0, seatPosition.z) + avatarOffset, 
                Quaternion.Euler(avatarRotation.x, avatarRotation.y, avatarRotation.z));
           avatarInstance.transform.localScale = new Vector3(avatarScale, avatarScale, avatarScale);

            var animator = avatarInstance.AddComponent<Animator>();
            animator.runtimeAnimatorController = avatarAnimators[idx];
            
            _seatToAvatarMap.Add(seatPosition, avatarInstance);
            
            idx = (idx + 1) % avatars.Length;
        }
    }

    // selecting given number of UNIQUE sitting places for avatars
    IEnumerable<Vector3> SelectSeats(int numberOfSeats)
    {
        HashSet<Vector3> availableSeats = new HashSet<Vector3>(_allSittingPositions);
        List<Vector3> selectedSeats = new List<Vector3>();
        System.Random random = new System.Random();
        
        while (selectedSeats.Count < numberOfSeats)
        {   
            Vector3 randomSeat = availableSeats.ElementAt(random.Next(availableSeats.Count));
            selectedSeats.Add(randomSeat);
            availableSeats.Remove(randomSeat);
        }

        return selectedSeats;
    }
    
    void DetectNeighbors()
    {
        // Dictionary to map each avatar to its valid neighbors
        _neighborMap = new Dictionary<GameObject, List<GameObject>>();

        // Get a list of all placed avatars
        var allAvatars = _seatToAvatarMap.Values.ToList();

        foreach (var avatar in allAvatars)
        {
            List<GameObject> neighbors = new List<GameObject>();

            foreach (var otherAvatar in allAvatars)
            {
                if (otherAvatar == avatar) continue;

                // Get the position of the other avatar in the local space of the current avatar
                Vector3 localPosition = avatar.transform.InverseTransformPoint(otherAvatar.transform.position);

                // Check alignment in local space
                if (Mathf.Abs(localPosition.z) > 0.5f) continue; // Ignore if not near the left-right plane
                if (localPosition.x > 0.5f || localPosition.x < -0.5f) // Strictly left or right
                {
                    neighbors.Add(otherAvatar);
                }
            }

            _neighborMap.Add(avatar, neighbors);
        }
    }



    
    void TriggerRandomInteractions()
    {
        // Get a random avatar that has neighbors
        var availableAvatars = _neighborMap.Keys.Where(avatar => _neighborMap[avatar].Count > 0).ToList();
        if (availableAvatars.Count == 0) return;

        GameObject randomAvatar = availableAvatars[UnityEngine.Random.Range(0, availableAvatars.Count)];
        List<GameObject> neighbors = _neighborMap[randomAvatar];

        // Select a random neighbor for interaction
        GameObject randomNeighbor = neighbors[UnityEngine.Random.Range(0, neighbors.Count)];

        // Start interaction coroutine
        StartCoroutine(HandleInteraction(randomAvatar, randomNeighbor));
    }


    IEnumerator HandleInteraction(GameObject avatar, GameObject neighbor)
    {
        // Determine facing direction
        bool faceLeft = neighbor.transform.position.x < avatar.transform.position.x;

        // Play initial animation for the first avatar
        var avatarAnimator = avatar.GetComponent<Animator>();
        avatarAnimator.Play(faceLeft ? "Talking left" : "Talking right");

        // Wait for 2 seconds before the neighbor responds
        yield return new WaitForSeconds(2.0f);

        // Play response animation for the neighbor
        var neighborAnimator = neighbor.GetComponent<Animator>();
        neighborAnimator.Play(faceLeft ? "Talking right" : "Talking left");

        yield return new WaitForSeconds(3.0f); // Wait before transitioning to idle
        avatarAnimator.Play("Idle");
        neighborAnimator.Play("Idle");

    }

    
    void FaceTarget(GameObject source, Transform target)
    {
        Vector3 direction = (target.position - source.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        source.transform.rotation = Quaternion.Slerp(
            source.transform.rotation,
            lookRotation,
            Time.deltaTime * 5.0f); // Adjust rotation speed
    }

    void Update()
    {
        
    }
}
