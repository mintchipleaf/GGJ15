using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(TweeFunctions))]
public class DialogueDisplay : MonoBehaviour {

	public static DialogueDisplay Instance { get; private set; }

	public TextAsset LevelFile;
	public Font font;

	public Vector2 DisplayStart = new Vector2 (0, 0);

	List<GameObject> WordObjects = new List<GameObject> ();

	void Awake () {

		Instance = this; 

	} 

	void Start () {

		TweeFunctions.Instance.Register (LevelFile);

	}

	public void Display (string passage) { 

		//Cleanup ();

		if (TweeFunctions.Instance.currentAsset != LevelFile)
			TweeFunctions.Instance.Register (LevelFile);

		TweeFunctions.Instance.SwitchPassage (passage);

		if (TweeFunctions.Instance.CurrentTweePassage != passage) {
			Debug.LogError ("Problem loading passage!");
			return;
		}

		int i = 0;
		List<TweeWord> wordList = TweeFunctions.Instance.CurrentTweeBody;
		while (i < wordList.Count) {
			CreateWord (wordList [i], i);
			i++;
		}

	}

	void CreateWord (TweeWord word, int i) {

		//string fullWord = "";

		foreach (string w in word.Word) {
			w.Replace (" ", string.Empty);
			//fullWord += w + " ";
			if (w == "")
				return;

			GameObject wordObj = new GameObject (w);

			Text text = wordObj.AddComponent<Text> ();
			text.text = w;
			if (word.LinkTo != null && word.LinkTo != "" && word.Responses == false) {
				DialogueButton button = wordObj.AddComponent<DialogueButton> ();
				button.linkTo = word.LinkTo;
				BoxCollider collide = wordObj.AddComponent<BoxCollider> ();
				collide.size = new Vector3 (500, 200, 1);
			}
			text.font = font;
			text.fontSize = 50;
			text.color = Color.black;
			text.rectTransform.sizeDelta = new Vector2 (500, text.preferredHeight);
			text.rectTransform.anchoredPosition = new Vector2 (-text.preferredWidth, -text.preferredHeight);

			wordObj.transform.SetParent (CanvasScript.Instance.transform);
			wordObj.transform.position = DisplayStart;

			WordObjects.Add (wordObj);
		}


	}

	void Cleanup () {

		if (WordObjects.Count == 0)
			return;

		foreach (GameObject obj in WordObjects) {
			Destroy (obj.gameObject);
		}

	}
}
