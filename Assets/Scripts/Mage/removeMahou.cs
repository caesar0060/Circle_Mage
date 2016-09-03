using UnityEngine;
using System.Collections;

public class removeMahou : MonoBehaviour {

	private Mage mage = null;

	// Use this for initialization
	void Start () {
		mage = this.transform.parent.gameObject.GetComponent<Mage> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void remove(){
		mage.remove();
	}
}
