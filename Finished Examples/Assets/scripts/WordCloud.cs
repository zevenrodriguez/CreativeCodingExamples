using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;


public class WordCloud : MonoBehaviour
{
    // Drag your .txt file into this slot in the Inspector
    [SerializeField] TextAsset textFile;
    [SerializeField] TextAsset partOfSpeechFile;

    // This array will hold all the individual words
    [SerializeField] string[] words;
    [SerializeField] string[] wordsPartOfSpeech;

    //List<string> cleanedWords = new List<string>();
    Dictionary<string, int> wordCounts = new Dictionary<string, int>();

    [SerializeField] GameObject wordPrefab;

    void Start()
    {
        if (textFile != null && partOfSpeechFile != null)
        {
            // 1. Get the entire text content as one string
            string fileContents = textFile.text;
            string partOfSpeechContents = partOfSpeechFile.text;
            Debug.Log("File Contents: " + fileContents);
            Debug.Log("Part of Speech Contents: " + partOfSpeechContents);

            wordsPartOfSpeech = partOfSpeechContents.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Log("--- Found " + wordsPartOfSpeech.Length + " parts of speech: ---");
            // 2. Split the string into an array of words
            //
            // We pass 'null' as the separator to split by all
            // default whitespace characters (space, tab, newline, etc.).
            //
            // 'RemoveEmptyEntries' is important. It ensures that if you
            // have two spaces in a row ("Hello  world"), you don't get
            // an empty "" entry in your array.

            words = fileContents.Split(" ", StringSplitOptions.RemoveEmptyEntries);


            Debug.Log("--- Found " + words.Length + " words: ---");


            foreach (string word in words)
            {
                bool isPartOfSpeech = false;

                foreach (string pos in wordsPartOfSpeech)
                {
                    if (word.Equals(pos, StringComparison.OrdinalIgnoreCase))
                    {
                        isPartOfSpeech = true;
                        break;
                    }
                }

                if (isPartOfSpeech == false)
                {
                    // basic cleanup: trim punctuation and whitespace, lower-case key
                    var key = word.Trim().ToLower().Trim(new char[] { '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')', '[', ']' });
                    if (string.IsNullOrEmpty(key))
                        continue;

                    /*
                    If string.IsNullOrEmpty(key) is true, the continue statement skips the rest of the code inside the if (isPartOfSpeech == false) block, specifically these lines:
                    */

                    if (wordCounts.ContainsKey(key))
                    {
                        wordCounts[key]++;
                    }
                    else
                    {
                        wordCounts[key] = 1;
                    }
                }

            }

            foreach (var kv in wordCounts)
            {
                Debug.Log($"Word: {kv.Key}, Count: {kv.Value}");
                // Instantiate a word prefab for each unique word
                if (kv.Value < 5)
                {
                    Vector3 position = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0.0f);
                    GameObject wordObj = Instantiate(wordPrefab, position, Quaternion.identity);
                    wordObj.GetComponent<TextMeshPro>().text = kv.Key;
                    wordObj.transform.localScale = wordObj.transform.localScale * (kv.Value * 0.1f);
                }

            }
        }
        else
        {
            Debug.LogError("Text file not assigned in the Inspector!");
        }



    }
}