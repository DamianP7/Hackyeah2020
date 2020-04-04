using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
	public Image redLine1, redLine2;
	public Animator animator;
	public float speed;

	bool coloring1, coloring2;

	private void OnEnable()
	{
		coloring1 = coloring2 = false;
		if (redLine1 != null)
		{
			redLine1.fillAmount = redLine2.fillAmount = 0;
			StartCoroutine(WaitForColor());
		}
		else
			StartCoroutine(WaitForAnimation());
	}

	private void Update()
	{
		if(coloring1)
		{
			redLine1.fillAmount += speed;
			if (redLine1.fillAmount == 1)
			{
				StartCoroutine(WaitForColor());
				coloring1 = false;
			}
		}
		else if (coloring2)
		{
			redLine2.fillAmount += speed;
			if (redLine2.fillAmount == 1)
			{
				coloring2 = false;
			}
		}
	}

	IEnumerator WaitForColor()
	{
		if (!coloring1 && redLine1.fillAmount == 0)
		{
			yield return new WaitForSeconds(1f);
			coloring1 = true;
		}
		else if (!coloring2 && redLine2.fillAmount == 0)
		{
			yield return new WaitForSeconds(0.3f);
			coloring2 = true;
		}
	}

	IEnumerator WaitForAnimation()
	{
		animator.SetTrigger("Wait");
		yield return new WaitForSeconds(1.5f);
		animator.SetTrigger("Start");
	}

}
