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

    // all values  in the method below are being adjusted based on trial and error method
    void PlaceAvatars(int numberOfAvatarsToPlace)
    {
        if (numberOfAvatarsToPlace > _allSittingPositions.Count)
        {
            throw new Exception("Not enough sitting spaces for given number of avatars");
        }

        HashSet<Vector3> selectedSeats = SelectSeats(numberOfAvatarsToPlace);

        Vector3 avatarOffset = new Vector3(0, -41, -8);    // still needs some adjustments
        int idx = 0;
        
        foreach (var seatPosition in selectedSeats.ToList())
        {
            Debug.Log(seatPosition.x + " " + seatPosition.y + " " + seatPosition.z);
            GameObject avatarInstance = Instantiate(avatars[idx], 
                seatPosition + avatarOffset, 
                Quaternion.Euler(0f, 180f, 0f));
            avatarInstance.transform.localScale = new Vector3(52,52,52);

            avatarInstance.AddComponent<Animator>();
            avatarInstance.GetComponent<Animator>().runtimeAnimatorController = avatarAnimators[idx];
            
            idx = (idx + 1) % avatars.Length;
        }
    }

    // selecting given number of UNIQUE sitting places for avatars
    HashSet<Vector3> SelectSeats(int numberOfSeats)
    {
        HashSet<Vector3> selectedSeats = new HashSet<Vector3>();
        
        while (selectedSeats.Count < numberOfSeats)
        {   
            int randomIndex = Random.Range(0, _allSittingPositions.Count);
            selectedSeats.Add(_allSittingPositions[randomIndex]);
        }

        return selectedSeats;
    }

    void Update()
    {
        
    }
}
