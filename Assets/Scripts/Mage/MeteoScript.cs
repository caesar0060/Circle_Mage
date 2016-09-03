using UnityEngine;
using System.Collections;

public class MeteoScript : MonoBehaviour {

	//爆発
	public GameObject bakuhatu;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/// <summary>
	/// メテオがチメント接触いたら、爆発する
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerEnter(Collider other){
		if (other.gameObject.layer == 14) {
			Debug.Log ("1");
			Vector3 pos = new Vector3 (this.transform.position.x, 0, this.transform.position.z);
			GameObject cloneObject = Instantiate (bakuhatu, pos, Quaternion.identity) as GameObject;
			cloneObject.transform.parent = this.transform;
		}
	}
}
