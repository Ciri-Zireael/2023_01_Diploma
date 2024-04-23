using UnityEngine;

public class SlideSwitcher : MonoBehaviour, Interactive
{
	[SerializeField] SlideHolder slideHolder;
	
	enum Direction
	{
		Previous,
		Next
	}

	[SerializeField] Direction direction = Direction.Previous;

	public void Interact()
	{
		switch (direction)
		{
			case Direction.Previous:
				slideHolder.PrevSlide();
				break;
			case Direction.Next:
				slideHolder.NextSlide();
				break;
		}
	}
}
