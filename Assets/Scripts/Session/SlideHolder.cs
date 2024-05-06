using UnityEngine;
using UnityEngine.UI;

public class SlideHolder : MonoBehaviour
{
	[SerializeField] Sprite[] images;
	Image canvas;
	int currentImageIndex;

	void Awake()
	{
		canvas = GetComponent<Image>();
	}

	public void NextSlide()
	{
		currentImageIndex++;

		if (currentImageIndex == images.Length)
		{
			currentImageIndex = 0;
		}

		canvas.sprite = images[currentImageIndex];
	}

	public void PrevSlide()
	{
		currentImageIndex--;

		if (currentImageIndex < 0)
		{
			currentImageIndex = images.Length - 1;
		}

		canvas.sprite = images[currentImageIndex];
	}
}