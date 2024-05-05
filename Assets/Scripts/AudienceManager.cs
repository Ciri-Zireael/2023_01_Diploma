using System;
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
    private List<Vector3> _allSittingPositions;

    void Start()
    {
        DetectChairPositions();
        PlaceAvatars(numberOfSeatsToFill);
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

        IEnumerable<Vector3> selectedSeats = SelectSeats(numberOfAvatarsToPlace);
        
        int idx = 0;
        
        foreach (var seatPosition in selectedSeats.ToList())
        {
            Debug.Log(seatPosition.x + " " + seatPosition.y + " " + seatPosition.z);
            GameObject avatarInstance = Instantiate(avatars[idx], 
                new Vector3(seatPosition.x, 0, seatPosition.z) + avatarOffset, 
                Quaternion.Euler(avatarRotation.x, avatarRotation.y, avatarRotation.z));
           avatarInstance.transform.localScale = new Vector3(avatarScale, avatarScale, avatarScale);

            avatarInstance.AddComponent<Animator>();
            avatarInstance.GetComponent<Animator>().runtimeAnimatorController = avatarAnimators[idx];
            
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

    void Update()
    {
        
    }
}
