using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

public class JaggedArrayParser {

	private string serialized;
	private JaggedArrayParser(string serialized) {
		this.serialized = serialized;
	}
	
	public static GameObject[][] Parse(string serialized) {
		var j = new JaggedArrayParser(serialized);
		return j.ParseToGameObjectJaggedArray();
	}
	
	private GameObject StringToGameObject(string s) {
		if (s == "_")
			return null;
			
		Debug.Log ("found " + s);
		
		var result = GameObject.Find("t_" + s);
		if (result == null)
			result = GameObject.Find(s);
		if (result == null)
			Debug.LogError("load error, could not find tile " + s);
		
		return result;
	}
	
	private GameObject[][] ParseToGameObjectJaggedArray() {
		string[][] stringJaggedArray = ParseJaggedArray();
		
		return stringJaggedArray.Select(x => x.Select(y => StringToGameObject(y)).ToArray()).ToArray();
	}

	private enum ParseState {BeginOuter, BeginInner, AccumulateWord, EndInner}
	private string[][] ParseJaggedArray() {
		ParseState state = ParseState.BeginOuter;
		
		StringBuilder accumulator = new StringBuilder();
		var innerArrayAccumulator = new List<string>();
		var outerArrayAccumulator = new List<List<string>>();
		
		for (int i = 0; i < serialized.Length; i++) {
			char c = serialized[i];
		
			if (char.IsWhiteSpace(c))
				continue;
			
			if (state == ParseState.BeginOuter) {
				if (c != '[')
					LogError("[", c, i);
				state = ParseState.BeginInner;
			} else if (state == ParseState.BeginInner) {
				if (c == '[') {
					accumulator = new StringBuilder();
					state = ParseState.AccumulateWord;
				} else if (c == ']') {
					if (i < serialized.Length - 1)
						Debug.LogError("Parse error; reached end of outer list before end of string"); 
					
					return outerArrayAccumulator.Select(x => x.ToArray()).ToArray();
				} else 
					LogError("[ or ]", c, i);
				
			} else if (state == ParseState.AccumulateWord) {
				// TODO: check against alphanumeric/underscore/hyphen
				// TODO: doesn't prevent empty string
				
				if (c == ',' || c == ']') {
					innerArrayAccumulator.Add(accumulator.ToString());
					accumulator = new StringBuilder();
					
					if (c == ']') {
						outerArrayAccumulator.Add (innerArrayAccumulator);
						innerArrayAccumulator = new List<string>();
						state = ParseState.EndInner;
					}	
				}  else {
					accumulator.Append(c);
				}
			} else if (state == ParseState.EndInner) {
				if (c == ']') {
					if (i < serialized.Length - 1)
						Debug.LogError("Parse error; reached end of outer list before end of string"); 
					
					return outerArrayAccumulator.Select(x => x.ToArray()).ToArray();
				} else if (c == ',')
					state = ParseState.BeginInner;
				else
					LogError(", or ]", c, i);
			}
		}
		
		throw new UnityException("Parse error, did not find end of list");
	}
	
	private void LogError(string expected, char got, int index) {
		// TODO: illustrate where discrepancy is in string
		Debug.LogError(string.Format(
				"Error while parsing board, expected {0} but got {1} at index {2} of {3}",
				expected,
				got,
				index,
				serialized
		));
	}
}
