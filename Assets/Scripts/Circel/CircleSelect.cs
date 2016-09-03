using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleSelect : MonoBehaviour {

	private List<GameObject> circleList = new List<GameObject>();
	private CircelControl circleControl = null;

	// Use this for initialization
	void Start () {
		circleControl = GameObject.Find ("GameRoot").GetComponent<CircelControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//接触いたクリスタルを、リストに入れる
	void OnTriggerEnter(Collider other){
		circleList.Add (other.gameObject);
	}

	public void SetChooseInRange(){
		/*for (int i = 0; i < circleList.Count; i++) {
			if(circleControl.nowChoosedLevel == 0 || circleList[i].GetComponent<Circel>().circelLevel <= circleControl.nowChoosedLevel)
				circleControl.setChoosed (circleList [i]);
		}
		Destroy (this.gameObject);*/
	}
}
