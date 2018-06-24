using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour {


	private string character;
	public GameObject fighterModel;
	TextMesh selectedText;

	// Use this for initialization
	void Start () {
		selectedText = fighterModel.GetComponentInChildren<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.name);



                if (hit.collider != null)
                {

                    GameObject touchedObject = hit.transform.gameObject;

					//set current character to chosen avatar
					character = touchedObject.name;
					Debug.Log(character + " Chosen");

					fighterModel.SetActive(true);
					selectedText.text = character + " Character Chosen";

                }
            }
        }

		else if (Input.GetMouseButtonDown(0))
        {
			Debug.Log("Mouise");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
			//Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
			if (Physics.Raycast(ray, out hit))
			{
				Debug.Log(hit.transform.name);

				if (hit.collider != null)
				{

					//get object that was touched
					GameObject touchedObject = hit.transform.gameObject;


					//set current character to chosen avatar
                    character = touchedObject.name;
                    Debug.Log(character + " Chosen");

                    fighterModel.SetActive(true);
                    selectedText.text = character + " Character Chosen";
					selectedText.color = UnityEngine.Color.black;
				}
			}
        }
	}
}
