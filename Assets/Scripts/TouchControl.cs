using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TouchControl : MonoBehaviour {
	private CircelControl circelControl = null;		//
	private GameObject tempCircel = null;			//
	//public GameObject rangeObject = null;
	//private GameObject range = null;
	private Vector3 pos;							//
	//private bool isButton = false;

	// Use this for initialization
	void Start () {
		circelControl = GameObject.Find ("GameRoot").GetComponent<CircelControl> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			//床、クリスタル、ボタンをタッチできる
			int layerMask = (1 << 10) | (1 << 5) | (1 << 8);
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
				GameObject obj = hit.collider.gameObject;
				pos = hit.point;
				switch (obj.layer){
				//クリスタル
				case 8:
					Circel circle = obj.transform.parent.gameObject.GetComponent<Circel> ();
					if (circle.c_State == Circel.STATE.CHOOSED && circle.circelLevel < 5) {
						tempCircel = circelControl.crystalGenerat (hit.point);
						if(tempCircel != null)
							circelControl.compound (obj.transform.parent.gameObject, tempCircel);
					}
					
					break;
				//床
				case 10:
					circelControl.CheckExist ();
					tempCircel = circelControl.crystalGenerat (hit.point);
					break;
				//ボタン
				case 5:
					//isButton = true;
					break;
				}
			}
		}

		if (Input.GetMouseButton (0)) {
			//床しか反応がない
			int layerMask = (1 << 10);
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit = new RaycastHit ();
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
				if (tempCircel != null) {/*
						tempCircel.GetComponent<Circel> ().isMove = true;
						pos = hit.point - pos;
						pos.y = tempCircel.transform.position.y;
						tempCircel.transform.position += pos;
						pos = hit.point;*/
					tempCircel.transform.position = hit.point;
				}
				/*else{
					if(!isButton){
						//選択範囲の計算
						float dis = Vector3.Distance (pos, hit.point)/2;
						float x = hit.point.x - pos.x;
						float z = hit.point.z - pos.z;
						float angle = Mathf.Atan2 (z, x) * Mathf.Rad2Deg;
						Vector3 selectPos = new Vector3 (pos.x + (Mathf.Cos (Mathf.Deg2Rad * angle) * dis), 0, pos.z + (Mathf.Sin (Mathf.Deg2Rad * angle) * dis));
						if (range == null) {
							range = Instantiate (rangeObject, selectPos, Quaternion.identity) as GameObject;
						}
						range.transform.localScale = new Vector3 (x, 1, z);
						range.transform.position = selectPos;

					}
				}*/
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			if(tempCircel != null){
				tempCircel.GetComponent<Circel>().isMove = false;
			}
			tempCircel = null;
			//isButton = false;
			/*if (range != null) {
				range.GetComponent<CircleSelect> ().SetChooseInRange ();
				range = null;
			}*/
		}
	}
	/*void DobuleClick(GameObject obj){
		if (obj.GetComponent<Circel> ().onClick == true) {
			if (obj.GetComponent<Circel> ().circelLevel == 1)
				circelControl.QuickCompound (obj);
			else
				circelControl.divide ();
			obj.GetComponent<Circel> ().onClick = false;
		}
		else
			obj.GetComponent<Circel> ().onClick = true;
	}*/
	/// <summary>
	/// Open the option.
	/// </summary>
	/// <param name="obj">Object.</param>
	public void OpenOption(GameObject obj){
		Time.timeScale = 0;
		obj.SetActive (true);
	}
	/// <summary>
	/// Close the option.
	/// </summary>
	/// <param name="obj">Object.</param>
	public void CloseOption(GameObject obj){
		Time.timeScale = 1;
		obj.SetActive (false);
	}
	public void GoToTittle(int num){
		Time.timeScale = 1;
		SceneManager.LoadScene (num);
	}
}
