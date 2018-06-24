using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(ButtonPressed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ButtonPressed()
	{
		SceneManager.LoadScene("LoadingScene");
	}
}
