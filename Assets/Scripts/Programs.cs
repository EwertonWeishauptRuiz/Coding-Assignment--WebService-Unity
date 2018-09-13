using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Programs : MonoBehaviour {

    // Constant and unchangeble strings for the query
    const string URL = "https://external.api.yle.fi/v1/programs/items.json?";
    string limit = "limit=10";
    const string id = "app_id=2a8e56be";
    const string offset = "offset=";
    const string key = "app_key=7a1ab27c5b94f3f78fd20ae6a121c17c";   
    // The input sent by the user.
    string inputRequested;
    // Number of the page the request should be allocated.
    int page;
    // List of JSON objects created from the resulting query.
    List<ShowInformation> showInformation = new List<ShowInformation>();

    // Handles the user request
    public void Request(string userInput, int userPage) {
        // Handles if the user input is null
        if (userInput == "") {
            // Sends a message to the CanvasManager script that creates the program list on the UI. 
            SendMessage("InvalidRequest");
            return;
        }
        // Assign the default values from the query 
        page = userPage;
        inputRequested = userInput;
        //Pass the input from the user to the query
        StartCoroutine(OnResponse());       
    }

    IEnumerator OnResponse() {
        // Handles the HTTP request
        WWW www = new WWW(SanitizeAndRequestQuery());
        
        //Wait for a response
        while (!www.isDone) {
            yield return null;
        } 
        // Assigns the JSON response to a string
        string jsonString = www.text;     
        // Creates an ShowInformation Object from the JSON Response.    
        ShowInformation query = JsonUtility.FromJson<ShowInformation>(jsonString);
        // Sends a message to the CanvasManager script that creates the program list on the UI.
        SendMessage("ReceiveShowInfo", query.data); 
        yield return query;
    }

    // Returns a list of ShowInformation Objects with the page counter.
    public List<ShowInformation.Datum> ListData() {
        return showInformation[page].data;
    }

    // Handles the URL and Sanitizes the query.
    string SanitizeAndRequestQuery() {
        // Sanitizes the query, so it is URL-Friendly
        string sanitizedQuery = "q=" + WWW.EscapeURL(inputRequested);
        // Returns the full 
        return URL + sanitizedQuery + "&" + limit + "&" + offset + page * 10 + "&" + id + "&" + key;
    }
}
