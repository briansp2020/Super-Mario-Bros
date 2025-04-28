using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
	public TextMeshProUGUI timerText;
	public float time = 400;
	public bool stopTime;

	private Music music;

	public int hurryTime { get; private set; } = 100;
	public bool hurry { get; private set; } = false;

	void Start()
	{
		music = GetComponent<Music>();
		StartCoroutine(CountTimer());
	}

	private IEnumerator CountTimer()
	{
		while (time >= 1)
		{
			if (!stopTime)
			{
                time -= Time.deltaTime;
				timerText.text = $"{(int)time}";
			}

			if (!hurry && time <= hurryTime)
			{
				StartCoroutine(music.ActivateHurryMusic());
			}

			hurry = time <= hurryTime;
			yield return null;
		}

		GameObject.FindWithTag("Player").GetComponent<Player>().Death();
	}
}
