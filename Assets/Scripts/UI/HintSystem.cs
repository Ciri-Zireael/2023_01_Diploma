using System.Collections;
using TMPro;
using UnityEngine;

public class HintSystem : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI text;
	Coroutine currentCoroutine;
	[SerializeField] float showDuration = 5f;

	public void ShowHint(string message)
	{
		if (currentCoroutine != null)
		{
			StopCoroutine(currentCoroutine);
		}

		currentCoroutine = StartCoroutine(HintCoroutine(message));
	}

	IEnumerator HintCoroutine(string message)
	{
		text.text = message;

		yield return FadeTextAlpha(0, 1, 0.5f);

		yield return new WaitForSeconds(showDuration);

		yield return FadeTextAlpha(1, 0, 0.5f);

		currentCoroutine = null;
	}
	
	IEnumerator FadeTextAlpha(float startAlpha, float endAlpha, float duration)
	{
		float elapsed = 0f;
		Color currentColor = text.color;

		while (elapsed < duration)
		{
			elapsed += Time.deltaTime;
			float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
			text.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
			yield return null;
		}

		text.color = new Color(currentColor.r, currentColor.g, currentColor.b, endAlpha);
	}
}
