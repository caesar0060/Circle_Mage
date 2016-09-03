using UnityEngine;
using System.Collections;

public class IceRiver : MonoBehaviour {
	private GameObject[] rivers = new GameObject[12];
	//つける順番
	private int e_num =1;	
	//消す順番
	private int d_num =1;
	//タイマー
	private float timer = 0;
	//カウンター
	private float enbleCounter = 0.1f;
	//カウンター
	private float disableCounter = 1.1f;
	public Transform river;
	// Use this for initialization
	void Start () {
		/*rivers =GameObject.FindGameObjectsWithTag ("IceRiver");
		for (int i = 0; i < rivers.Length; i++) {
			for (int j = i+1; j < rivers.Length ; j++) {
				if (int.Parse(rivers [i].name) > int.Parse(rivers [j].name)) {
					GameObject temp = rivers [i];
					rivers [i] = rivers [j];
					rivers [j] = temp;
				}
			}
		}*/
		Debug.Log (river.childCount);
		for (int i = 0; i < 12; i++) {
			rivers [i] = this.gameObject.transform.GetChild (0).GetChild (i).gameObject;
			Debug.Log (rivers[i].name);
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= enbleCounter) {
			if (e_num < rivers.Length) {
				rivers [e_num].GetComponent<Collider> ().enabled = true;
				e_num++;
				enbleCounter += 0.1f;
			}
		}

		if (timer >= disableCounter) {
			if (d_num < rivers.Length) {
				rivers [d_num].GetComponent<Collider> ().enabled = false;
				d_num++;
				enbleCounter += 0.1f;
			}
		}
	}
}
