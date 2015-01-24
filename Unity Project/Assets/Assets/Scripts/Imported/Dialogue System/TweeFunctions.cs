/*
-------------------------
Twee Functions
-------------------------

This script runs through a twine passage and looks for twee
syntax, which then it performs functions accordingly.
 * 
 * Code is copyright Allen-Michael Brower used with permission by Wagh Creations
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

// TWEESYNTAX Holds syntax types.
public enum TweeSyntax {

	NULL,
	LINK_TO

}

// TWEESTATUS Holds the status of the current Twee passage.
public enum TweeStatus {

	NOT_IN_TWEE,
	DISPLAYING,
	DISPLAYED

}


// TWEEWORD holds the text for each word in a passage, along with associated special attributes.
public class TweeWord {

	public string[] Word;
	public string LinkTo;
	public TweeSyntax Effect;
	public bool Responses = false;
	public int ResponseNum;

}

public class TweeFunctions : MonoBehaviour {


	public static TweeFunctions Instance { get; private set; } // SINGLETON!!!!

	// Twee Event related variables.
	public delegate void TweeEvents(string tweeEvent);		// Used for game events. Holds the type of event from the enum.
	public static event TweeEvents TweeEvent;				// Twee Event being called.

	// Twee Event related variables.
	public delegate void FinishEvent();		// Used for game events. Holds the type of event from the enum.
	public static event FinishEvent FinishTwee;				// Twee Event being called.



	// Twee parser and Twee Status related.
	public TweeParser parser = new TweeParser();			// The almighty twine parser!
	public TweeStatus CurrentStatus = TweeStatus.NOT_IN_TWEE;	// Status of the current Twee passage.

	[HideInInspector]
	public TextAsset
		currentAsset;

	// Passage related variables.
	List<TweeWord> currentTweeBody;
	string currentPassage;
	string[] currentTags;
	string currentBody;			// This is passed the current passage body being used.

	public string CurrentTweePassage { get { return currentPassage; } }
	public List<TweeWord> CurrentTweeBody { get { return currentTweeBody; } }
	public string CurrentCrunchedBody {
		get {
			string crunch = "";
			foreach(TweeWord w in CurrentTweeBody) {
				foreach(string word in w.Word)
					crunch += " " + word;
			}
			return crunch;
		}
	}
	public string[] CurrentTweeTags { get { return currentTags; } }

	void Awake() {

		if(Instance == null)
			Instance = this;

	}


	// ************************************
	// * FILE RELATED FUNCTIONS.
	// ************************************

	// Registers the file to the almighty TweeParser.
	// There are two so there's the option to call through a file browser or an event. Most cases will be event.
	public void Register(FileInfo file) {

#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX

		if(file == null) {
			Debug.LogError("The text file is missing!");
			return;
		}

		parser.Register(file);	// Sacrifice to the parser!


		RegisterComplete();		// Finishes registering after grabbing the file.

#endif

	}

	public void Register(TextAsset file) {

		if(file == null) {
			Debug.LogError("The text file is missing!");
			return;
		}

		parser.Register(file);		// Sacrifice to the parser!

		currentAsset = file;

		RegisterComplete();			// Finishes registering after grabbing the file.

	}

	public void Unregister() {

		LeaveTwee();

	}

	void RegisterComplete() {

		// FIXME May make it so you can load the file without displaying the text
		currentPassage = "Start";		// Sets the passage so I can reference it later.


		currentBody = parser.passages[currentPassage].body;		// Changes the body to display.
		currentTweeBody = ParsePassage();						// Crunches everything inside of the text body, including code.


		CrunchTags();

	}

	public void SwitchPassage(string switchTo) {

		CurrentStatus = TweeStatus.DISPLAYING;

		currentPassage = switchTo;

		if(!parser.passages.ContainsKey(currentPassage)) {
			Debug.LogError("Passage '" + currentPassage + "' does not exist!");
			return;
		}

		ClearPassages();
		currentBody = parser.passages[currentPassage].body;
		currentTweeBody = ParsePassage();
		CrunchTags();

	}

	void ClearPassages() {
		currentBody = "";
		currentTweeBody.Clear();
		currentTags = null;
		CurrentStatus = TweeStatus.NOT_IN_TWEE;
	}

	// ************************************
	// * PASSAGE PARSING
	// ************************************

	// **** Parse the passage for code ****
	public List<TweeWord> ParsePassage() {
		string tempBody = currentBody.Replace("[[", "***[[").Replace("]]", "]]***").Replace("<responses>", "***<responses>***");
		string[] newBody = tempBody.Split(new string[] { "***" }, System.StringSplitOptions.RemoveEmptyEntries);

		List<TweeWord> tweeBody = new List<TweeWord>();

		for(int i = 0; i < newBody.Length; i++) {
			tweeBody.Add(CrunchWord(newBody[i]));
		}

		for(int j = 0; j < tweeBody.Count; j++) {
			if(tweeBody[j].Word[0] == "<responses>")
				tweeBody.Remove(tweeBody[j]);
		}

		endResponses = false;

		return tweeBody;
	}

	// **** Deposit each word into a TweeWord with attributes. ****
	private bool endResponses = false;
	TweeWord CrunchWord(string line) {

		TweeWord currentTweeWord = new TweeWord();

		if(line.StartsWith("<responses>")) {
			endResponses = true;
		}
		if(line.StartsWith("[[")) {
			string dialogue = line.Replace("[[", "");
			dialogue = dialogue.Replace(dialogue.Substring(dialogue.LastIndexOf('|')), "");

			if(dialogue.StartsWith("[")) {
				Debug.LogError("ERROR: Duplicate [ detected in line '" + dialogue + "'.");
			}

			string LinkTo = line.Replace("[[", "").Replace("]]", "");
			LinkTo = LinkTo.Substring(LinkTo.IndexOf('|') + 1);
			LinkTo = LinkTo.Trim();

			currentTweeWord.Word = dialogue.Split(' ');
			currentTweeWord.Effect = TweeSyntax.LINK_TO;
			currentTweeWord.LinkTo = LinkTo;
			currentTweeWord.Responses = endResponses;
		}
		else if(line.Contains("***")) {
			Debug.LogError("ERROR: There was an error crunching code! Placeholder string *** still exists in line '" + line + "'. Check to make sure no duplicate code exists.");
		}
		else {
			currentTweeWord.Word = line.Split(' ');
			currentTweeWord.Effect = TweeSyntax.NULL;
			currentTweeWord.LinkTo = "";
		}

		return currentTweeWord;

	}

	// **** Crunch each tag. ****
	void CrunchTags() {

		currentTags = parser.passages[currentPassage].tags;

		if(currentTags != null) {
			foreach(string tag in currentTags) {
				if(tag.StartsWith("EVENT:")) {
					ParseEvent(tag);
				}
				if(tag.ToLower() == "finish") {
					CurrentStatus = TweeStatus.NOT_IN_TWEE;
					LeaveTwee();
				}
			}
		}

	}

	// ************************************
	// * TWEE EVENTS
	// ************************************

	// **** Grab each event from tags. ****
	void ParseEvent(string e) {
		if(e == null) {
			Debug.LogError("Received unnamed event.");
			return;
		}
		string eventClean = e.Replace("EVENT:", "");
		eventClean = eventClean.Trim();

		if(TweeEvent != null)
			TweeEvent(eventClean);
		else {
			Debug.LogWarning("Found a Twee Event but no function to go with it for tag: " + e + ".");
			return;
		}
	}

	void LeaveTwee() {

		ClearPassages();
		currentAsset = null;
		currentPassage = "";
		parser.ClearAll();
		CurrentStatus = TweeStatus.NOT_IN_TWEE;
		if(FinishTwee != null)
			FinishTwee();
		Debug.Log("Leaving Twee");

	}

	public void AddCallback(TweeEvents callback) {

		TweeEvent += callback;

	}

	public void RemoveCallback(TweeEvents callback) {

		TweeEvent -= callback;

	}

	public void ClearCallbacks() {

		TweeEvent = null;

	}

	public bool Contains(string passage) {

		if(parser.passages.ContainsKey(passage))
			return true;
		return false;

	}

}