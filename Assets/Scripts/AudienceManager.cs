using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudienceManager : MonoBehaviour
{
    [SerializeField] private int numberOfSeatsToFill;
    [SerializeField] private float interactionInterval = 10.0f;

    private void Start()
    {
        DetectChairPositions();
        PlaceAvatars(numberOfSeatsToFill);
        DetectNeighbors();
        InvokeRepeating(nameof(TriggerRandomInteractions), 2.0f, interactionInterval);
    }

    private List<Vector3> _allSittingPositions;

    private void DetectChairPositions()
    {
        _allSittingPositions = new List<Vector3>();
        var chairs = GameObject.FindGameObjectsWithTag("Chair");
        foreach (var chair in chairs) _allSittingPositions.Add(chair.transform.position);
    }

    [SerializeField] private GameObject[] avatars;
    [SerializeField] private RuntimeAnimatorController[] avatarAnimators;

    [SerializeField] private Vector3 avatarOffset = new(0, 0, 0);
    [SerializeField] private Vector3 avatarRotation = new(0, 0, 0);
    [SerializeField] private float avatarScale = 1;

    private Dictionary<Vector3, GameObject> _seatToAvatarMap;

    private void PlaceAvatars(int numberOfAvatarsToPlace)
    {
        if (numberOfAvatarsToPlace > _allSittingPositions.Count)
            throw new Exception("Not enough sitting spaces for given number of avatars");

        _seatToAvatarMap = new Dictionary<Vector3, GameObject>();
        var selectedSeats = SelectSeats(numberOfAvatarsToPlace);

        var idx = 0;

        foreach (var seatPosition in selectedSeats.ToList())
        {
            var avatarInstance = Instantiate(avatars[idx],
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
    private IEnumerable<Vector3> SelectSeats(int numberOfSeats)
    {
        var availableSeats = new HashSet<Vector3>(_allSittingPositions);
        var selectedSeats = new List<Vector3>();
        var random = new System.Random();

        while (selectedSeats.Count < numberOfSeats)
        {
            var randomSeat = availableSeats.ElementAt(random.Next(availableSeats.Count));
            selectedSeats.Add(randomSeat);
            availableSeats.Remove(randomSeat);
        }

        return selectedSeats;
    }

    private Dictionary<GameObject, List<GameObject>> _neighborMap;
    [SerializeField] private float neighborDetectionRadius = 2.0f;

    private void DetectNeighbors()
    {
        // Dictionary to map each avatar to its valid neighbors
        _neighborMap = new Dictionary<GameObject, List<GameObject>>();

        var allAvatars = _seatToAvatarMap.Values.ToList();

        foreach (var avatar in allAvatars)
        {
            var neighbors = new List<GameObject>();

            foreach (var otherAvatar in allAvatars)
            {
                if (otherAvatar == avatar) continue;

                var distance = Vector3.Distance(avatar.transform.position, otherAvatar.transform.position);
                if (distance > neighborDetectionRadius) continue; // Only consider avatars within the radius

                // Position of the other avatar in the local space of the current avatar
                var localPosition = avatar.transform.InverseTransformPoint(otherAvatar.transform.position);

                // Check if the other avatar is strictly to the left or right (ignoring front/back)
                if (Mathf.Abs(localPosition.z) > 0.5f) continue; // Ignore if not near the left-right plane
                if (localPosition.x > 0.5f || localPosition.x < -0.5f) // Strictly left or right
                    neighbors.Add(otherAvatar);
            }

            _neighborMap.Add(avatar, neighbors);
        }
    }


    private void TriggerRandomInteractions()
    {
        // Random avatar that has neighbors
        var availableAvatars = _neighborMap.Keys.Where(avatar => _neighborMap[avatar].Count > 0).ToList();
        if (availableAvatars.Count == 0) return;

        var randomAvatar = availableAvatars[Random.Range(0, availableAvatars.Count)];
        var neighbors = _neighborMap[randomAvatar];

        var randomNeighbor = neighbors[Random.Range(0, neighbors.Count)];

        StartCoroutine(HandleInteraction(randomAvatar, randomNeighbor));
    }


    private IEnumerator HandleInteraction(GameObject avatar, GameObject neighbor)
    {
        // Determine facing direction
        var faceLeft = neighbor.transform.position.x < avatar.transform.position.x;

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
}