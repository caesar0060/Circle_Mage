using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbyssEyeScript : MonoBehaviour {

	public float castTime;		//呪文を唱える時間
	public float finishTime;	//魔法の終わる時間
	private bool onEffect = false;	//魔法が作用中かどうか
	private float timer = 0;
	public int attraction;	//引力
	private Collider[] mahouCollider;

	// Use this for initialization
	void Start () {
		mahouCollider = this.GetComponentsInChildren<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer >= castTime && onEffect == false && timer < finishTime) {
			mahouCollider [1].enabled = true;
			onEffect = true;
		} else if (timer >= finishTime && onEffect == true) {
			mahouCollider [1].enabled = false;
		}
	}
	void OnTriggerStay(Collider other){
		if (other.tag == "Monster") {
			Vector3 move = (this.transform.position - other.transform.position).normalized;
			move.y = 0;
			other.transform.position += move * attraction * Time.deltaTime;
		}
	}
}
