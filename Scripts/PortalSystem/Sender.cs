using UnityEngine;
using System.Collections;

public class Sender : MonoBehaviour {

	GameObject player;
	GameObject otherObj;
	[SerializeField] GameObject receiver;

    float prevDot = 0;
    bool playerOverlapping = false;
	bool otherOverlapping = false;
	bool haveCopy=false;
	GameObject cloneObj;

	[SerializeField] Animator anim;
	[SerializeField] GameObject main_obj;
	bool enabled=false;
	[SerializeField] AudioSource _as;
	[SerializeField] AudioClip on_off_sound;
	void Awake()
	{
		player = GameObject.FindGameObjectWithTag ("Player");

		Off_Portal ();
		//_as = GetComponent<AudioSource> ();
	}

    void Start () {
		//Off_Portal ();
    }

	void Off_Portal()
	{
		if (!enabled)
			return;
		enabled = false;
		anim.SetBool ("Enabled", false);
		_as.PlayOneShot (on_off_sound);
		//StartCoroutine (WaitCoroutine (1f));
		//main_obj.SetActive (false);
	}
	void On_Portal()
	{
		if (enabled)
			return;
		//main_obj.SetActive (true);
		anim.SetBool ("Enabled", true);
		//StartCoroutine (WaitCoroutine (0.2f));
		_as.PlayOneShot (on_off_sound);

		enabled = true;
	}

	IEnumerator WaitCoroutine(float time)
	{
		yield return new WaitForSeconds(time);
		//main_obj.SetActive (false);
	}



	void DeleteCopy()
	{
		Destroy (cloneObj);
		haveCopy = false;
	}

	void CreateCopy(GameObject obj)
	{
		haveCopy = true;


		Vector3 positionOffset = obj.transform.position - transform.position;
		var newPosition = receiver.transform.position + positionOffset;

		cloneObj=Instantiate(obj, newPosition, Quaternion.identity) as GameObject;
		cloneObj.GetComponent<Rigidbody> ().isKinematic = true;
		cloneObj.GetComponent<BoxCollider> ().isTrigger = true;
		cloneObj.tag="Clone";
	}


    void Update()
    {
		if (!enabled)
			return;

		if (haveCopy) {
			Vector3 positionOffset = otherObj.transform.position - transform.position;
			var newPosition = receiver.transform.position + positionOffset;
			cloneObj.transform.position = newPosition;
			cloneObj.transform.rotation = otherObj.transform.rotation;
		}

        if (playerOverlapping) {
            var currentDot = Vector3.Dot(transform.up, player.transform.position - transform.position);

            if (currentDot < 0) // only transport the player once he's moved across plane
            {
                // transport him to the equivalent position in the other portal
                float rotDiff = -Quaternion.Angle(transform.rotation, receiver.transform.rotation);
                rotDiff += 180;
                player.transform.Rotate(Vector3.up, rotDiff);

                Vector3 positionOffset = player.transform.position - transform.position;
                positionOffset = Quaternion.Euler(0, rotDiff, 0) * positionOffset;
                var newPosition = receiver.transform.position + positionOffset;
                player.transform.position = newPosition;

                playerOverlapping = false;
            }
               
            prevDot = currentDot;
        }


		if (otherOverlapping) {
			var currentDot = Vector3.Dot(transform.up, otherObj.transform.position - transform.position);

			if (currentDot < 0) // only transport the player once he's moved across plane
			{
				// transport him to the equivalent position in the other portal
				float rotDiff = -Quaternion.Angle(transform.rotation, receiver.transform.rotation);
				rotDiff += 180;
				otherObj.transform.Rotate(Vector3.up, rotDiff);

				Vector3 positionOffset = otherObj.transform.position - transform.position;
				positionOffset = Quaternion.Euler(0, rotDiff, 0) * positionOffset;
				var newPosition = receiver.transform.position + positionOffset;
				//otherObj.GetComponent<Rigidbody> ().position = newPosition;
				otherObj.transform.position = newPosition;

				otherOverlapping = false;

			}

			prevDot = currentDot;

		}				

    }


    void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("Player"))
        {
            playerOverlapping = true;
        }
	/*	if (other.CompareTag("DragObject"))
		{
			Debug.Log ("Enter");
			otherOverlapping = true;
			otherObj = other.gameObject;
			if(!haveCopy)
				CreateCopy (otherObj);
		}*/

    }



    void OnTriggerExit(Collider other)
    {
		if (other.CompareTag("Player"))
        {
            playerOverlapping = false;
        }
		/*if (other.CompareTag("DragObject"))
		{
			otherOverlapping = false;
			DeleteCopy ();
		}*/
    }
}
