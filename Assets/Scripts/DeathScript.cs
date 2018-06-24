using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeathScript : MonoBehaviour {

    private float playerHealth=100;
	bool rerun = false;
	bool runOnce = true;

	int newVarTest;

	private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision detected");

    }


	private void OnTriggerStay(Collider other)
	{
		if(runOnce)
		{
			runOnce = false;
			StartCoroutine(KillingDetection(other));
		}

		StartCoroutine(checkForDPS());
	}

	public float GetPlayerHealth()
	{
		return playerHealth;
	}

	IEnumerator KillingDetection(Collider other)
	{
		GameObject spider = other.gameObject;
        spider.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        spider.GetComponent<NavMeshAgent>().isStopped = true;
        spider.GetComponent<Animator>().SetTrigger("Attack");
		//yield return new WaitForSeconds(1f);
		playerHealth -= 10;
		rerun = true;
		yield return null;
	}

	IEnumerator checkForDPS()
	{
		if (rerun)
        {
            rerun = false;
            StartCoroutine(HealthDecrease());
        }
		yield return null;
	}

	IEnumerator HealthDecrease()
	{
		yield return new WaitForSeconds(1f);
        playerHealth -= 10;
		rerun = true;
	}


}
