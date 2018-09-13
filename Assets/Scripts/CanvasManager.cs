using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
    // Prefab of the image holder
    public GameObject programPrefab;
    // Object with grid aligment of the programs.
    public GameObject pageObject;

    // UI Elements
    [Header("ScrollBar")]
    public Scrollbar scrollbar;
    [Header("HolderOfPages")]
    public RectTransform holderOfPages;
    [Header("Program Image")]
    RawImage programImage;
    [Header("Invalid Request Text")]
    public Text requestAlert;
    [Header("Input Field")]
    public InputField inputField;
    string currentUserInput;

    Programs programs;
    List<GameObject> resultPrograms = new List<GameObject>();
    // Flag to block requests
    bool requestPagination;
    // Counter of pages
    int page;

    public bool userMadeQuery;

    //Set up default values and grab components.
    void Start() {
        currentUserInput = "";
        requestAlert.text = "";
        requestPagination = true;
        userMadeQuery = false;
        programs = GetComponent<Programs>();
    }

    // Handles the UI when a request is invalid.
    void InvalidRequest() {
        requestAlert.color = Color.red;
        requestAlert.text = "Please insert a search parameter";        
    }

    void ReceiveShowInfo(List<ShowInformation.Datum> info) {
        // Handles the UI if there's no results found from the query.
        if (info.Count == 0 && resultPrograms.Count == 0) {
            requestAlert.color = Color.red;
            requestAlert.text = "No Results found";
            return;
        }
        // Handles the UI if there's no more results found from the query.
        else if (info.Count == 0 && resultPrograms.Count != 0) {
            requestAlert.color = Color.red;
            requestAlert.text = "Found " + resultPrograms.Count + " results. " + "No more results";
            return;
        }
        foreach (ShowInformation.Datum data in info) {
            // Instantiate the prefab
            GameObject program = Instantiate(programPrefab);
            // Set it as a child of the page container
            program.transform.SetParent(pageObject.transform, false);
            resultPrograms.Add(program);
            // Get The Text for the title
            program.GetComponentsInChildren<Text>()[0].text = data.title.ReceiveTitle();
			// Get the texts inside the More information Text and pass it to an array
            Text[] texts = program.transform.GetChild(2).GetComponentsInChildren<Text>();           
            // Apply information to the array text of the More Information Panel
            texts[0].text = "Description: " + data.description.ReceiveDescription();
            texts[1].text = "Duration: " + data.ReceiveDuration();
            texts[2].text = "Type: " + data.ReceiveType();
            texts[3].text = "Original Title: " + data.originalTitle.ReceiveOriginalTitle();
            // Disable more information Panel
            program.transform.GetChild(2).gameObject.SetActive(false);
            // Check if the image is null, if is not, assign the Image url as Image.
            if(data.image.ReceiveImage() != null){                
				StartCoroutine(RenderImage(program.gameObject.transform.GetChild(0).GetComponent<RawImage>(), data.image.ReceiveImage()));            
            }                        
        }
        // Handles the UI showing the quantity of results found from the query.
        requestAlert.text = "Found " + resultPrograms.Count + " results";
        requestAlert.color = Color.green;
        // Adds a colldown period before the next Request
        StartCoroutine(WaitTimeForNextRequest());
    }

    void Update() {        
        // When the user reaches the bottom of the page, load more results
        if(!requestPagination && scrollbar.value == 0) {            
            Paginate();
        }        
        // Get the Rerturn Key, and do a request
        if (Input.GetKeyDown(KeyCode.Return)) {
            SearchPressed();
        }
    }

    void Paginate() {        
        requestPagination = true;
        // Increment the page size
        page++;
        // If the user has changed the Input field value, make a new Request.
        if(inputField.text != currentUserInput) {
            ResetScreen();
        }
        // Calls the request function on the Programs script.
        programs.Request(currentUserInput, page);        
    }

    IEnumerator WaitTimeForNextRequest() {
        //Gives a small delay of oen on each request        
        yield return new WaitForSeconds(1);
        // Set request pagination to false, allowing another Request
        requestPagination = false;        
    }

    // Get Image to an URL
    public IEnumerator RenderImage(RawImage rawImage, string url) {
        using (WWW www = new WWW(url)) {
            yield return www;
            // Grab the image from the prefab Program
            programImage = rawImage;
            // Assing the image to the Raw Image component.
            programImage.texture = www.texture;
        }
    }
    
    // Handles when the search button or the return key are pressed
    public void SearchPressed() {
        // Resets the list so there are no programs on the screen
        ResetScreen();
        // Calls the request function on the Programs script.
        programs.Request(currentUserInput, page);
    }

    void ResetScreen() {
        // Clear the UI destroying all the existing programs.
        foreach (GameObject program in resultPrograms) {
            Destroy(program);            
        }
        // Clear the list, to reset the counter of results.
        resultPrograms.Clear();
        // Blocks any other request that can be made in the meantime 
        requestPagination = true;
        // Assigns to the current Input the value of the Input Field.
        currentUserInput = inputField.text;
        // Resets the page counter
        page = 0;
    }
}
