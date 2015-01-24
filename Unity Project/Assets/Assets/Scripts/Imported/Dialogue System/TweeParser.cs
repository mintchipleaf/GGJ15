using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class TweeParser {
	
	// The passage class, which is just title, tags, and body
	[System.Serializable]
	public class TweePassage {
		public string title;
		public string[] tags;
		public string body;
	}
	
	// The TextAsset resource we'll be parsing
	//public TextAsset tweeSourceAsset;
	public TextAsset tweeSourceAsset;
	
	// Dictionary to hold the passages, keyed by their titles
	public Dictionary<string, TweePassage> passages =
		new Dictionary<string, TweePassage>();
	
	// I found it useful during development to have an inspectable
	// array of the titles of the loaded passages.
	public string[] titles = new string[0];
	
	// The one big string that will hold the twee source file
	protected string tweeSource;
	
	
	void Awake() {
		
	}
	
	public void Register (FileInfo twineAsset) {
		#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		//tweeSourceAsset = twineAsset;
		// Load the twee source from the asset
		//tweeSource = tweeSourceAsset.text;
		tweeSource = ReadFile(twineAsset);
		
		// Parse it
		Parse();
		
		// Populate the reference array
		titles = new string[passages.Count];
		passages.Keys.CopyTo(titles, 0);
		Debug.Log("Loaded " + titles.Length + " Twine passages.");
		#endif
	}
	
	public void Register (TextAsset twineAsset) {
	
		tweeSourceAsset = twineAsset;
		// Load the twee source from the asset
		tweeSource = tweeSourceAsset.text;
		//tweeSource = ReadFile(twineAsset);
		
		// Parse it
		Parse();
		
		// Populate the reference array
		titles = new string[passages.Count];
		passages.Keys.CopyTo(titles, 0);
		Debug.Log("Loaded " + titles.Length + " Twine passages.");
		
	}
	
	protected StreamReader reader = null;
	
	protected string text = " "; // assigned to allow first line to be read below
	
	string ReadFile (FileInfo asset) {
		#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		
		reader = asset.OpenText();
		return reader.ReadToEnd();
		
		#endif
		return "";
	}
	
	
	// Where the magic happens
	private void Parse() {
		
		// A reference to the passage we're currently building from the source
		TweePassage currentPassage = null;
		
		// Buffer to hold the content of the current passage while we build it
		StringBuilder buffer = new StringBuilder();
		
		// Array that will hold all of the individual lines in the twee source
		string[] lines; 
		
		// Utility array used in various instances where a string needs to be split up
		string[] chunks;
		
		// Split the twee source into lines so we can make sense of it while parsing
		lines = tweeSource.Split(new string[] {"\n"}, System.StringSplitOptions.None);
		
		// Just iterating through the whole file here
		for (long i = 0; i < lines.LongLength; i++) {
			
			// If a line begins with "::" that means a new passage has started
			if (lines[i].StartsWith("::")) {
				
				// If we were already building a passage, that one is done.
				// Wrap it up and add it to the dictionary of passages. 
				if (currentPassage != null) {
					currentPassage.body = buffer.ToString();
					passages.Add(currentPassage.title, currentPassage);                 
					buffer = new StringBuilder();
				}
				
				/* I know, I know, a magic number and chained function calls and it's
                 * ugly, but it's not that complicated. A new passage in a twee file
                 * starts with a line like this:
                 *
                 * :: The Passage Begins Here [someTag anotherTag heyThere]
                 *               
                 * What's happening here is when a new passage starts, we ignore the
                 * :: prefix, strip off the ] at the end of the tags, and split the
                 * line on [ into two strings, one of which will be the passage title
                 * while the other has all of the passage's tags, if any are found.
                 */
				chunks = lines[i].Substring(2).Replace ("]", "").Split ('[');
				
				// We should always have at least a passage title, so we can
				// start a new passage here with that title.
				currentPassage = new TweePassage();
				currentPassage.title = chunks[0].Trim();
				
				// If there was anything after the [, the passage has tags, so just
				// split them up and attach them to the passage.
				if (chunks.Length > 1) {
					currentPassage.tags = chunks[1].Trim().Split(' ');  
				}
				
			} else if (currentPassage != null) {
				
				// If we didn't start a new passage, we're still in the previous one,
				// so just append this line to the current passage's buffer.
				buffer.AppendLine(lines[i]);    
			}
		}
		
		// When we hit the end of the file, we should still have the last passage in
		// the file in the buffer. Wrap it up and end it as well.
		if (currentPassage != null) {           
			currentPassage.body = buffer.ToString();
			passages.Add(currentPassage.title, currentPassage);
		}
	}

	public void ClearAll() {

		tweeSourceAsset = null;
		passages.Clear();
		titles = new string[0];
		tweeSource = "";

	}
}