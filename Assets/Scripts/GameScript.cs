using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Vuforia;
using UnityEngine.UI;

public class GameScript : MonoBehaviour {

	List<GameObject> spiders;
	public GameObject spiderPrefab;
	public int numberSpiders = 4;
	int spiderCount = 0;

	public GameObject fighter;
	PlayerController playerController;
	public NavMeshAgent fighterAgent;
	DeathScript deathScript;
    
	float maxHeight;
    float minHeight;
    float maxWidth;
    float minWidth;
    float margin = 0;

	Text playerHealth;

	DefaultTrackableEventHandler trackableHandler;
    TrackableBehaviour trackable;
    TrackableBehaviour.Status trackingStatus;
    

	// Use this for initialization
	void Start () {

		//setup spider list
		spiders = new List<GameObject>();

		//setup the player
		playerController = fighter.GetComponent<PlayerController>();
		playerController.SetArsenal("Rifle");
		deathScript = fighter.GetComponent<DeathScript>();
								  

        //get the bounds for the playing field
		Renderer mesh = this.GetComponent<Renderer>();

        maxHeight = mesh.bounds.max.z - margin;
        minHeight = mesh.bounds.min.z + margin;
        maxWidth = mesh.bounds.max.x - margin;
        minWidth = mesh.bounds.min.x + margin;

		playerHealth = this.GetComponentInChildren<Text>();
		playerHealth.text = "Health: 100%";

		//vuforia setup
		trackableHandler = this.GetComponent<DefaultTrackableEventHandler>();


	}
	
	// Update is called once per frame
	void Update () {

		trackable = trackableHandler.getTrackableBehaviour();
        trackingStatus = trackable.CurrentStatus;

		if(trackingStatus == TrackableBehaviour.Status.TRACKED)
		{
			if(Input.GetMouseButtonDown(0))
			{
				Debug.Log("Button pressed");

				StartCoroutine(mouseShooting());
			}
			else if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
			{
				Debug.Log("Screen touched");

				StartCoroutine(touchShooting());
			}


		}


		if (spiderCount < numberSpiders && trackingStatus == TrackableBehaviour.Status.TRACKED)
        {
			StartCoroutine(generateSpiders());
        }
		else if(spiderCount > 0 &&  trackingStatus != TrackableBehaviour.Status.TRACKED)
		{
			//Debug.Log("Killing spiders");
			StartCoroutine(destroySpiders());
		}

		float health = deathScript.GetPlayerHealth();
		if(health > 0)
		{
			playerHealth.text = "Health: " + health +"%";	
		}
		else
		{
			StartCoroutine(PlayerDead());

		}


	}



	IEnumerator PlayerDead()
	{

		playerHealth.text = "Game Over";
		fighter.GetComponent<Animator>().SetTrigger("Death");
		StartCoroutine(destroySpiders());

		yield return new WaitForSeconds(2f);
		spiderCount = numberSpiders + 1;
		//Destroy(fighter);


	}



	IEnumerator mouseShooting()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.transform.name);

            if (hit.collider != null)
            {
                Debug.Log("Spider Hit!");


                //get object that was touched
                GameObject touchedObject = hit.transform.gameObject;

                //make the shooter face the object
                fighter.transform.LookAt(touchedObject.transform);

                //make the shooter fire at the object
                fighter.GetComponent<Animator>().SetTrigger("Aiming");
                fighter.GetComponent<Animator>().SetTrigger("Attack");

                //stop the object that is shot and kill it
                touchedObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                touchedObject.GetComponent<NavMeshAgent>().isStopped = true;
                touchedObject.GetComponent<Animator>().SetTrigger("Death");
				yield return new WaitForSeconds(1f);
				spiders.Remove(touchedObject);
				Destroy(touchedObject);
				spiderCount--;


            }
        }
		yield return null;
	}

	IEnumerator touchShooting()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;
        //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.transform.name);



            if (hit.collider != null)
            {

				//get object that was touched
                GameObject touchedObject = hit.transform.gameObject;

                //make the shooter face the object
                fighter.transform.LookAt(touchedObject.transform);

                //make the shooter fire at the object
                fighter.GetComponent<Animator>().SetTrigger("Aiming");
                fighter.GetComponent<Animator>().SetTrigger("Attack");

                //stop the object that is shot and kill it
                touchedObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                touchedObject.GetComponent<NavMeshAgent>().isStopped = true;
                touchedObject.GetComponent<Animator>().SetTrigger("Death");
                yield return new WaitForSeconds(1f);
                spiders.Remove(touchedObject);
                Destroy(touchedObject);
                spiderCount--;            

            }
        }
		yield return null;
	}



	IEnumerator destroySpiders()
	{

		foreach(GameObject spider in spiders)
		{
			Destroy(spider);
			spiderCount = 0;
		}
		yield return null;
	}

	IEnumerator generateSpiders()
	{

        float x;
        float y;
        float z;

		y = fighter.transform.position.y;
        //instantiate number of spiders place them randomly
		for (; spiderCount < numberSpiders; spiderCount++)
        {

            float topSides = Random.Range(0, 10);

            Debug.Log(topSides);

            if (topSides < 5)
            {
                int topBottom = Random.Range(0, 10);

                if (topBottom < 5)
                {
                    x = Random.Range(minWidth, maxWidth);
                    z = maxHeight;
                }
                else
                {
                    x = Random.Range(minWidth, maxWidth);
                    z = minHeight;
                }

            }
            else
            {
                float leftRight = Random.Range(0, 10);

                if (leftRight < 5)
                {
                    x = maxWidth;
                    z = Random.Range(minHeight, maxHeight);
                }
                else
                {
                    x = minWidth;
                    z = Random.Range(minHeight, maxHeight);
                }
            }


            Vector3 startDestination = new Vector3(x, y, z);

            Debug.Log(startDestination);
            
			GameObject newSpider = Instantiate(spiderPrefab, startDestination, Quaternion.identity) as GameObject;

			spiders.Add(newSpider);
			newSpider.transform.LookAt(fighter.transform);
			newSpider.GetComponent<NavMeshAgent>().SetDestination(fighter.transform.position);
           
        }

		yield return null;
	}
    

}
