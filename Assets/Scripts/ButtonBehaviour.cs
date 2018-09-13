using UnityEngine;

public class ButtonBehaviour : MonoBehaviour {

    GameObject moreInformationPanel;
    GameObject moreInformationText;
    
    // Grabs the gameobjects
    void Start(){
        moreInformationPanel = gameObject.transform.GetChild(2).gameObject;
        moreInformationText = gameObject.transform.GetChild(3).gameObject;
    }
    // public function changing the flag of active game objects
    // to show the more information panel.
    public void OnClick(){
        moreInformationPanel.SetActive(!moreInformationPanel.activeInHierarchy); 
        moreInformationText.SetActive(!moreInformationPanel.activeInHierarchy); 
    }
}
