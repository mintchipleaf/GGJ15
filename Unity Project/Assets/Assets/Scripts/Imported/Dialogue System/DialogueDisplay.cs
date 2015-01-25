using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(TweeFunctions))]
public class DialogueDisplay : MonoBehaviour {

	public GameObject wordPrefab;
	public static DialogueDisplay Instance { get; private set; }

	public TextAsset LevelFile;
	public Font font;

	public Vector2 DisplayStart = new Vector2 (0, 0);

	public float spaceSize;

	private Renderer lastWord;
	private RectTransform wordStart;

	List<GameObject> WordObjects = new List<GameObject> ();

	void Awake () {
		//wordPrefab = (GameObject)Resources.Load("Text.prefab");
		Instance = this; 
		lastWord = new Renderer();
	} 

	void Start () {
		TweeFunctions.Instance.Register (LevelFile);

	}

	public void Display (string passage, Canvas canvas = null) { 

		//Cleanup ();

		if (TweeFunctions.Instance.currentAsset != LevelFile)
			TweeFunctions.Instance.Register (LevelFile);

		TweeFunctions.Instance.SwitchPassage (passage);

		if (TweeFunctions.Instance.CurrentTweePassage != passage) {
			Debug.LogError ("Problem loading passage!");
			return;
		}

		int i = 0;
		//List<TweeWord> wordList = TweeFunctions.Instance.CurrentTweeBody;
		string body = TweeFunctions.Instance.CurrentCrunchedBody;

		GameObject wordObj = (GameObject)Instantiate (wordPrefab, Vector3.zero, Quaternion.Euler(0,180,0));
		
		Text text = wordObj.GetComponent<Text> ();
		text.text = body; //fullWord;

		wordObj.transform.SetParent(canvas.transform);
		wordObj.transform.position = new Vector3(canvas.transform.position.x - 1.5f, canvas.transform.position.y, canvas.transform.position.z);
		text.font = font;
		text.fontSize = 150;
		text.rectTransform.localScale = new Vector3(0.1f,0.1f,0.1f);

		lastWord = null;
		wordStart = canvas.GetComponentInChildren<RectTransform>();
		/*while (i < wordList.Count) {
			CreateWord (wordList [i], i, canvas);
			i++;
		}*/

	}

	void CreateWord (TweeWord word, int i, Canvas canvas) {

		//string fullWord = "";

		foreach (string w in word.Word) {
			w.Replace (" ", string.Empty);
			//fullWord += w + " ";
			if (w == "")
				return;
		
			GameObject wordObj = (GameObject)Instantiate (wordPrefab, Vector3.zero, Quaternion.identity);

			Text text = wordObj.GetComponent<Text> ();
			text.text = w; //fullWord;
			if (word.LinkTo != null && word.LinkTo != "" && word.Responses == false) {
				DialogueButton button = wordObj.AddComponent<DialogueButton> ();
				button.linkTo = word.LinkTo;
				BoxCollider collide = wordObj.AddComponent<BoxCollider> ();
				//collide.size = new Vector3 (500, 200, 1);
			}
			wordObj.transform.SetParent(canvas.transform);
			text.font = font;
			text.fontSize = 200;
			text.rectTransform.localScale = new Vector3(0.1f,0.1f,0.1f);
			//text.rectTransform.
			//text.color = Color.black;
			//text.rectTransform.sizeDelta = new Vector2 (500, text.preferredHeight);
			//text.rectTransform.anchoredPosition = new Vector2 (-text.preferredWidth, -text.preferredHeight);

			//Positioning
			/*if(lastWord == null){
				wordObj.transform.position = wordStart.position + new Vector3(wordObj.renderer.bounds.extents.x, 0, 0);
				Debug.Log(wordObj.transform.position);
			}
			else {
				float wordPosition = lastWord.bounds.max.x + spaceSize + wordObj.renderer.bounds.extents.x;
				wordObj.transform.position = new Vector3(wordPosition, wordStart.position.y, wordStart.position.z);
				Debug.Log (wordObj.transform.position);
			}*/
			wordObj.transform.position = canvas.transform.position;
			//wordObj.transform.position = DisplayStart;

			WordObjects.Add (wordObj);
			//lastWord = wordObj.renderer;
		}
		//Debug.Log (fullWord);
	}

	public void Cleanup () {

		if (WordObjects.Count == 0)
			return;

		foreach (GameObject obj in WordObjects) {
			Destroy (obj.gameObject);
		}

	}
}
