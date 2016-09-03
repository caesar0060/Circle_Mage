using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {
	GameObject player ;
	public float y_speed;
	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		Vector3 ya = this.transform.position;
		Vector3 p = player.transform.position;
		Vector3 move = (p - ya).normalized;
		this.GetComponent<Rigidbody> ().AddForce (move * y_speed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision other){
		//playerと接触したら
		if (other.gameObject.tag == "MonsterAttack") {
			GetComponentInParent<MonsterAnimatorScript> ().PlayerDamage ();
			Destroy (this.gameObject);
		}
	}
}
