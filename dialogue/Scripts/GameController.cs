using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
	public TextAsset inkJSON;
	private Story story;
	
	public Animator dialogueSelectionAnimator;
	public Animator fadeAnimator;
	public int selectedOption = 0; // 0 = left, 1 = right;
	public bool canProceed = false;
	public bool makingChoice = false;
	public InputField dialogueBox;
	public GameObject leftChoiceBox;
	public GameObject rightChoiceBox;
	public GameObject[] awakeObjects;
	public GameObject[] awakeObjectsChoice;
	public GameObject[] nonNarratedObjects;
			
	string currentText;
	
    // Start is called before the first frame update
    void Start()
    {
        story = new Story(inkJSON.text);
		currentText = story.Continue();
		StartCoroutine(BuildText());
    }

    // Update is called once per frame
    void Update()
    {
		if (story.currentChoices.Count > 1){
			makingChoice = true;
			leftChoiceBox.GetComponent<InputField>().text = story.currentChoices[0].text;
			rightChoiceBox.GetComponent<InputField>().text = story.currentChoices[1].text;
		}
        if (Input.GetKeyDown("left") || Input.GetKeyDown("a")){
			selectedOption--;
			if (selectedOption<0) { selectedOption = 0; }
			dialogueSelectionAnimator.SetInteger("selected", selectedOption);
		}
		else if (Input.GetKeyDown("right") || Input.GetKeyDown("d")){
			selectedOption++;
			if (selectedOption>1) { selectedOption = 1; }
			dialogueSelectionAnimator.SetInteger("selected", selectedOption);
		}
		if ((Input.GetKeyDown("space") || Input.GetKeyDown("enter")) && canProceed){
			if (makingChoice){
				story.ChooseChoiceIndex(selectedOption);
				story.Continue();
			}
			dialogueBox.text = "";
			leftChoiceBox.GetComponent<InputField>().text = "";
			rightChoiceBox.GetComponent<InputField>().text = "";
			
			currentText = story.Continue();
			
			if (currentText == "!FROM_BLACK\n"){
				StartCoroutine(FadeFromBlack());
				currentText = story.Continue();
			}
			else if (currentText == "!TO_BLACK\n"){
				Debug.Log("test");
				StartCoroutine(FadeToBlack());
				currentText = story.Continue();
			}
			
			if (currentText == "!NARRATOR\n"){
				for (int k=0; k<nonNarratedObjects.Length; k++){
					nonNarratedObjects[k].SetActive(false);
				}
				currentText = story.Continue();
			}
			else if (currentText == "!SAMAEL\n"){
				loadCharacter("SAMAEL");
				for (int k=0; k<nonNarratedObjects.Length; k++){
					nonNarratedObjects[k].SetActive(true);
				}
				currentText = story.Continue();
			}
			else if (currentText == "!WAYNE\n"){
				loadCharacter("WAYNE");
				for (int k=0; k<nonNarratedObjects.Length; k++){
					nonNarratedObjects[k].SetActive(true);
				}
				currentText = story.Continue();
			}
			
			if (currentText == "!CAVE\n"){
				travelTo("CAVE");
				currentText = story.Continue();
			}
			else if (currentText == "!VILLAGE\n"){
				travelTo("VILLAGE");
				currentText = story.Continue();
			}
			
			
			for (int i=0; i<awakeObjects.Length; i++){
				awakeObjects[i].SetActive(false);
			}
			for (int j=0; j<awakeObjectsChoice.Length; j++){
				awakeObjectsChoice[j].SetActive(false);
			}
			selectedOption=0;
			canProceed = false;
			makingChoice = false;
			StartCoroutine(BuildText());
		}
    }
	
	private IEnumerator FadeFromBlack(){
		fadeAnimator.SetBool("faded", false);		
		yield return new WaitForSeconds(0);
	}
	private IEnumerator FadeToBlack(){
		fadeAnimator.SetBool("faded", true);
		yield return new WaitForSeconds(0);
	}
	
	private IEnumerator BuildText(){
		for (int i = 0; i < currentText.Length; i++){
			dialogueBox.text = string.Concat(dialogueBox.text, currentText[i]);
			if (i==(currentText.Length-1)){
				canProceed = true;
				for (int k=0; k<awakeObjects.Length; k++){
					awakeObjects[k].SetActive(true);
				}
				if (makingChoice){
					for (int j=0; j<awakeObjectsChoice.Length; j++){
						awakeObjectsChoice[j].SetActive(true);
					}
				}
			}
			yield return new WaitForSeconds((float)0.03);
		}
	}
	
	public SpriteRenderer bg;
	public Sprite caveSprite;
	public Sprite villageSprite;
	
	public void travelTo(string destination){
		if (destination=="CAVE"){
			bg.sprite = caveSprite;
		}
		else if (destination=="VILLAGE"){
			bg.sprite = villageSprite;
		}
	}
	
	public InputField characterName;
	public SpriteRenderer portrait;
	public Sprite samaelSprite;
	public Sprite wayneSprite;
	
	public void loadCharacter(string character){
		characterName.text = character;
		if (character=="SAMAEL"){
			portrait.sprite = samaelSprite;
		}
		else if (character == "WAYNE"){
			portrait.sprite = wayneSprite;
		}
	}
}
