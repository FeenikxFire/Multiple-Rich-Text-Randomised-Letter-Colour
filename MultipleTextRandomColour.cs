using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultipleTextRandomColour : MonoBehaviour
{
    #region UI Text Components
    [Header("UI Text Components")]
    [Tooltip("Manually Filled - UI Text Elements that will random colour per letter.")]
    public Text[] uiText;           //Manually Filled.
    private string[] textToString;  //Array for stored strings converted from UI Text.
    private string textToWrite = "";//Stored specific string for individual character iterations.
    private string[] originalWords; //Original Words in uiText.
    private string[] characters;    //Array for individual letters of each word.
    #endregion

    #region Text Colours and Components
    [Header("Colours")]
    [Tooltip("Manually Set - Amount and Colours the text letters will randomize between.")]
    public Color[] textColours;     //Colours the text letters will randomize between.
    private string[] colours;       //Array of strings that converts the colours in textColours[] to #HexCodes http://htmlcolorcodes.com/.
    private int randTextColour;     //Number which the random colour from the textColours[] get stored.
    #endregion

    #region Hex codes set with HTML Strings
    //Unity Inspector (Rich) Text takes the string format of:
    //      <color=#HEXCODE> Text you want coloured </color>
    private string startHTMLcolour = ("<color=#");  //Stored string for start Hex code formatting. Requires ">" to finish HTML Start.
    private string endHTMLcolour = ("</color>");    //Stored string for end Hex code formatting.
    #endregion

    #region Failsafe
    //Check so awake and start don't conflict in exection order.
    private bool started = false;
    #endregion

    // Use this for initialization
    void Awake()
    {
        //Set the size of the textToString[] to be the same size as the uiText[].
        textToString = new string[uiText.Length];

        //Set the size of the originalWords[] to be the same size as the uiText[].
        originalWords = new string[uiText.Length];

        //Set the size of the colours[] string to be the same size as the textColours[].
        colours = new string[textColours.Length];

        #region Store colours
        //For every Colour in textColours[], convert the #hex colour code into string form and store it.
        for (int j = 0; j < textColours.Length; j++)
        {                                                                                           //This was missing from startHMLColour, because it needed to be inserted after the #Hex colour.
            colours[j] = (startHTMLcolour + ColorUtility.ToHtmlStringRGBA(textColours[j]).ToString() + ">"); //Added > on the end for Unity UI "HTML" to do the conversion in Rich Text.
        }
        #endregion

        //Set new colours for each word.
        InitializeRandomizeTextColour();

        started = true; //Failsafe.
    }

    private void OnEnable()
    {
        if(started)
        {
            //When the object is enabled - Choose new colours.
            RerollRandomTextColour();
        }
    }

    #region Initializing Randomize Text Colour
    private void InitializeRandomizeTextColour()
    {
        //For every Text in uiText - Convert it to a string.
        for (int i = 0; i < uiText.Length; i++)
        {
            textToString[i] = uiText[i].text.ToString();    //Current[i] string = the same text as uiText[i] in the inspector.
            originalWords[i] = uiText[i].text.ToString();   //Current[i] original word = the same as uiText[i] in the inspector.
            uiText[i].text = "";    //Clear the current[i] UI text - It's going to be overwritten. Saves errors of duplicate texts in UI.

            textToWrite = textToString[i];
            characters = new string[textToWrite.Length];
            for (int k = 0; k < textToWrite.Length; k++)    //While index is less than the word length - Write.
            {
                characters[k] = textToWrite[k].ToString();

                //Set the current letter to have a random colour from available colours in textColours[].
                RollRandomTextColour();
                characters[k] = (colours[randTextColour] + characters[k] + endHTMLcolour);

                //Set the uiText to be what it currently is + the new letter. (It's been cleared already).
                uiText[i].text = uiText[i].text + characters[k];
            }
        }
    }
    #endregion

    #region ReRoll Randomized Text Colour
    public void RerollRandomTextColour()    //Call this to re-roll each Letter in every UI text within uiText[].
    {
        //For every Text in uiText - Convert it to a string.
        for (int i = 0; i < uiText.Length; i++)
        {
            textToString[i] = originalWords[i]; //Current[i] string = the same text as uiText[i] in the inspector.
            uiText[i].text = "";    //Clear the current[i] UI text - It's going to be overwritten. Saves errors of duplicate texts in UI.

            textToWrite = textToString[i];      //The current textToWrite is [i].
            characters = new string[textToWrite.Length];    //Set the amount of letters is the same as the word about to be written.

            for (int j = 0; j < textToWrite.Length; j++)    //While index is less than the word length - Write.
            {
                characters[j] = textToWrite[j].ToString();
                
                //Set the current letter to have a random colour from available colours in textColours[].
                RollRandomTextColour();
                characters[j] = (colours[randTextColour] + characters[j] + endHTMLcolour);

                //Set the uiText to be what it currently is + the new letter. (It's been cleared already).
                uiText[i].text = uiText[i].text + characters[j];
            }
        }
    }
    #endregion

    #region Roll Random Text Colour
    //Roll a random number to a max of the amount of colours in the inspector.
    private void RollRandomTextColour()
    {
        randTextColour = Random.Range(0, colours.Length);
    }
    #endregion
}
