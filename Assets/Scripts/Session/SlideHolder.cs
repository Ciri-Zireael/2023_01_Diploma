using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SlideHolder : MonoBehaviour
{
	[SerializeField] Sprite[] images;
	public static string folderPath = "Presentation1";
	Image canvas;
	int currentImageIndex;

	void Awake()
	{
		Debug.Log("Hi, I'm awake. folderPath = " + folderPath);
		canvas = GetComponent<Image>();
		LoadSlides();
	}
	
	void LoadSlides()
	{
		// Load images from the Resources folder
		var loadedTextures = Resources.LoadAll<Texture2D>(folderPath);
		images = loadedTextures.Select(tex => Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.one * 0.5f)).ToArray();

		if (images.Length == 0)
		{
			Debug.LogWarning("No slides found in folder: " + folderPath);
		}
		else
		{
			canvas.sprite = images[0];	
		}
		
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
	
	public static void SetPresentationPath(string newPath)
	{
		folderPath = newPath;
	}
}