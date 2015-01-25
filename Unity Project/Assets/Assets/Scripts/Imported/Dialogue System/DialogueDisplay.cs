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

	public GameObject wordPrefab;

	List<GameObject> WordObjects = new List<GameObject> ();

	void Awake () {
		//wordPrefab = (GameObject)Resources.Load("Text.prefab");
		Instance = this; 

	} 

	void Start () {
		TweeFunctions.Instance.Register (LevelFile);

	}

	public void Display (string passage, Transform canvas = null) { 

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
			CreateWord (wordList [i], i, canvas);
			i++;
		}

	}

	void CreateWord (TweeWord word, int i, Transform canvas) {

		//string fullWord = "";

		foreach (string w in word.Word) {
			w.Replace (" ", string.Empty);
			//fullWord += w + " ";
			if (w == "")
				return;

			GameObject wordObj = (GameObject)Instantiate (wordPrefab, Vector3.zero, Quaternion.identity);

			Text text = wordObj.GetComponent<Text> ();
			text.text = w;
			if (word.LinkTo != null && word.LinkTo != "" && word.Responses == false) {
				DialogueButton button = wordObj.AddComponent<DialogueButton> ();
				button.linkTo = word.LinkTo;
				BoxCollider collide = wordObj.AddComponent<BoxCollider> ();
				//collide.size = new Vector3 (500, 200, 1);
			}
			wordObj.transform.SetParent (canvas);
			text.font = font;
			text.fontSize = 200;
			text.rectTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
			//text.rectTransform.
			//text.color = Color.black;
			//text.rectTransform.sizeDelta = new Vector2 (500, text.preferredHeight);
			//text.rectTransform.anchoredPosition = new Vector2 (-text.preferredWidth, -text.preferredHeight);


			wordObj.transform.position = canvas.position;
			//wordObj.transform.position = DisplayStart;

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
