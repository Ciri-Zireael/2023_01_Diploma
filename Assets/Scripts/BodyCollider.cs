using UnityEngine;

public class BodyCollider : MonoBehaviour
{
	[SerializeField] Transform head;
	[SerializeField] Transform feet;
	void Update()
	{
		transform.position = new Vector3(head.position.x, feet.position.y, head.position.z);
	}
}
