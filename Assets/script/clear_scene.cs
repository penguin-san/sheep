using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class clear_scene : MonoBehaviour {

	public Text score_text;
	public AudioClip best_score_audio;
	public Text rank_text;
	public AudioClip score_audio;
	public AudioSource audioSource;

	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		if(Data.Instance.score > Data.Instance.best_score) {
			Data.Instance.best_score = Data.Instance.score;
			audioSource.clip = best_score_audio;
		} else {
			audioSource.clip = score_audio;
		}

		if(Data.Instance.score > 2000) {
			rank_text.text = "寝かしのプロ!";
		} else if(Data.Instance.score > 1500) {
			rank_text.text = "普通の人";
		} else if(Data.Instance.score > 1000) {
			rank_text.text = "もうちょっとがんばれ！";
		} else if(Data.Instance.score > 500) {
			rank_text.text = "ダメダメ男";
		} else {
			rank_text.text = "いないほうが寝れるかも";
		}

		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(int.Parse(score_text.text) < Data.Instance.score) {
			score_text.text = (int.Parse(score_text.text) + 10).ToString();
		}


	}
}
