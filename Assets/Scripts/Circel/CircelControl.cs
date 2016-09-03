using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CircelControl : MonoBehaviour {

	//クリスタルの状態
	public enum STATE{
		NONE = -1,		//未使用
		UNCHOOSE = 0,	//選択されていない
		CHOOSED,		//選択されている
		USING,			//魔法を使っている
		DENFENCE,		//防御中
		COMPOUNDED		//合成中
	};

	public struct CircelInfo{
		//クリスタルの状態
		public STATE circel_state;
		//クリスタルのUIのオジェブトを保存
		public GameObject u_crystal;					//クリスタルのオジェブトを保存
	};
	//クリスタルの数
	public int circleNum = 1;						

	public List<GameObject> circels = new List<GameObject>();	//クリスタルのUIを保存する配列
	public GameObject crystal;									//クリスタルのプレハブ
	public List<GameObject> crystals = new List<GameObject>();	//クリスタルを保存する配列
	public GameObject UI_crystal;					//クリスタルUIのプレハブ
	public Transform gameUI;						//gameUiのtransform
	[HideInInspector] public bool isUsing = false;	//魔法をつかっているかどうか

	public Text debugText =null;					//デブグ用のテキスト

	public int nowChoosedLevel = 0;				//選択したクリスタルのレベル
	public GameObject[] ButtonUI;					//ボタンの配列

	//魔力を回復するの間隔
	public float manaRecoverTime;
	//一秒の回復の量
	public float manaRecoverPencent;			//0.1秒の回復の量
	public float crystalRecoverPencent;			//クリスタルによる回復の増量
	private PlayerStatus playerStatus;
	public Text recoveryText;

	// Use this for initialization
	void Start () {
		if (LoadLevel.Instance != null) {
			circleNum = LoadLevel.Instance.GetCircleNum ();
		}
		playerStatus = GameObject.Find ("Player").GetComponent<PlayerStatus> ();

		// クリスタルを生成
		for (int i = 0; i < circleNum; i++) {
			Vector3 pos = new Vector3 (-350 + i * 40, 180, 0);
			GameObject u_crystal = Instantiate (UI_crystal, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			u_crystal.transform.SetParent (gameUI);
			u_crystal.transform.localPosition = pos;
			u_crystal.transform.localScale = new Vector3 (1, 1, 1);
			u_crystal.transform.localRotation = Quaternion.Euler (0, 0, 0);
			//配列に入れる
			circels.Add (u_crystal);
			//クリスタル一個につき、回復量を増加する
			manaRecoverPencent += crystalRecoverPencent;
		}
		StartCoroutine (ManaRecovery ());
	}
	
	// Update is called once per frame
	void Update () {
		//Debug
		//-------------------------------------------------------

		/*debugText.text = "";
		for(int i =0; i<circels.Count;i++){
			debugText.text += ("Circel"+ i.ToString() + ": " + circels[i].circel_state.ToString() + 
			                   " Level: " + circels[i].u_crystal.GetComponentInChildren<Circel>().circelLevel +"\n");
		}*/
		//-------------------------------------------------------

		//待機しているクリスタルがあれば、魔力を回復する
	/*	for (int i =0; i < circels.Count; i++) {
			if(circels[i].circel_state == STATE.UNCHOOSE){
				set_Recover_Time(circels[i].u_crystal, Time.deltaTime);
			}
			else
				set_Recover_Time(circels[i].u_crystal, 0);
		}*/
	}

	/// <summary>
	/// 一秒ごとに魔力を回復する関数
	/// </summary>
	/// <returns>The recovery.</returns>
	public IEnumerator ManaRecovery(){
		while (true) {
			int count = 0;
			for (int i = 0; i < circels.Count; i++) {
				if (!circels [i].transform.GetChild (0).gameObject.activeInHierarchy) {
					count++;
				}
			}
			if (playerStatus.p_MP < playerStatus.p_MP_slider.maxValue) {
				playerStatus.p_MP += playerStatus.p_MP_slider.maxValue * 
					((manaRecoverPencent - crystalRecoverPencent * count) / 100);
				recoveryText.text = (playerStatus.p_MP_slider.maxValue *
					((manaRecoverPencent - crystalRecoverPencent * count) / 100) * 10).ToString ();
				if (playerStatus.p_MP > playerStatus.p_MP_slider.maxValue)
					playerStatus.p_MP = playerStatus.p_MP_slider.maxValue;
			}
			yield return new WaitForSeconds(manaRecoverTime);
		}
	}

	/// <summary>
	/// 合成
	/// </summary>
	/// <param name="C1">クリスタル</param>
	/// <param name="C2">クリスタル</param>
	public void compound(GameObject C1, GameObject C2){
		//setUnChooseAll ();
		Circel C1Circel = C1.GetComponent<Circel> ();
		//Levelにより、クリスタルの数が変わる
		C1.transform.GetChild (C1Circel.circelLevel).gameObject.SetActive (false);
		C1Circel.circelLevel += C2.GetComponent<Circel> ().circelLevel;
		C1.transform.GetChild (C1Circel.circelLevel).gameObject.SetActive (true);
		//光る
		C1.transform.GetChild (0).gameObject.SetActive (true);
		//setCircelState (C1, STATE.CHOOSED);
		nowChoosedLevel = C1Circel.circelLevel;
		//魔法のボタンを隠す
		HideButtonUI();
		ShowButtonUI ();
		//合成されたクリスタルを隠す
		if (C2.GetComponent<Circel> ().circelLevel > 1) {
			C2.transform.GetChild (C2.GetComponent<Circel> ().circelLevel).gameObject.SetActive (false);
			C2.GetComponent<Circel> ().circelLevel = 1;
			C2.transform.GetChild (C2.GetComponent<Circel> ().circelLevel).gameObject.SetActive (true);
			int childNum = C2.transform.childCount;
			for (int j =7; j < childNum; j++) {
				crystals.Remove (C2.transform.GetChild(7).gameObject);
				Destroy (C2.transform.GetChild(7).gameObject);
			}
		}
		crystals.Remove (C2.gameObject);
		Destroy (C2.gameObject);
		//setCircelState (C2, STATE.COMPOUNDED);
		C2.SetActive (false);
	}
	/// <summary>
	/// 合成したクリスたルを分解する
	/// </summary>
	/*public void divide(){
		int count = 0;
		Vector3 pos = new Vector3(0,0,0);
		//選択しているクリスタルを探し、分解させる
		for (int i =0; i< circels.Count; i++) {
			if (circels [i].circel_state == STATE.CHOOSED) {
				pos = circels[i].Object.transform.position;
				circels [i].Object.transform.GetChild (circels [i].Object.GetComponent<Circel> ().circelLevel).gameObject.SetActive (false);
				circels [i].Object.GetComponent<Circel> ().circelLevel = 1;
				circels [i].Object.transform.GetChild (circels [i].Object.GetComponent<Circel> ().circelLevel).gameObject.SetActive (true);
				nowChoosedLevel = 1;
				int childNum = circels[i].Object.transform.childCount;
				//合成されたクリスタルを表す
				for (int j =7; j < childNum; j++) {
					GameObject tempCircle = circels[i].Object.transform.GetChild(7).gameObject;
					tempCircle.SetActive(true);
					tempCircle.transform.position = new Vector3(pos.x + Mathf.Sin(72 *count) * 3 , 0, pos.z + Mathf.Cos(72*count) * 3);
					setCircelState(tempCircle, STATE.UNCHOOSE);
					tempCircle.transform.parent = null;
					StartCoroutine( tempCircle.GetComponent<Circel>().ManaRecovery());
					count++;
				}
			}
		}
		HideButtonUI ();

	}*/
	/// <summary>
	/// Removes the cirstal.
	/// </summary>
	public void RemoveCirstal(GameObject obj){
		int count = obj.GetComponent<Circel> ().circelLevel;
		for (int j = 0; j < count; j++) {
			for (int i = 0; i < circels.Count; i++) {
				if (!circels [i].transform.GetChild (0).gameObject.activeInHierarchy) {
					circels [i].transform.GetChild (0).gameObject.SetActive (true);
					break;
				}
			}
		}
		crystals.Remove (obj);
		Destroy (obj.gameObject);
	}

	//クリスタルを生成する
	public GameObject crystalGenerat(Vector3 pos){
		for (int i = circels.Count - 1; i >= 0; i--) {
			if (circels [i].transform.GetChild (0).gameObject.activeInHierarchy) {
				circels [i].transform.GetChild (0).gameObject.SetActive (false);
				GameObject crystalClone = Instantiate (crystal, pos, Quaternion.identity) as GameObject;
				nowChoosedLevel = crystalClone.GetComponent<Circel> ().circelLevel;
				crystalClone.GetComponent<Circel> ().c_State = Circel.STATE.CHOOSED;
				crystalClone.transform.GetChild(0).gameObject.SetActive(true);
				crystalClone.GetComponent<Circel> ().circelID = i;
				crystals.Add (crystalClone);
				ShowButtonUI ();
				return crystalClone;
			}
		}
		return null;
	}
	/// <summary>
	/// クリスタル存在するかどうか
	/// </summary>
	public void CheckExist(){		
		for (int i = 0; i < crystals.Count; i++) {
			if (crystals [i].GetComponent<Circel> ().c_State == Circel.STATE.CHOOSED) {
				HideButtonUI ();
				RemoveCirstal (crystals [i]);
				break;
			}
		}
	}
		

	//クリスタルの状態を選択されるに変更する
	/*public void setChoosed(GameObject circel){
		for(int i =0; i<circels.Count;i++){
			if(i == circel.GetComponent<Circel>().circelID){
				//今選択いているクリスタルと同じレベルだったら
				if(circel.GetComponent<Circel>().circelLevel == nowChoosedLevel){
					CircelInfo tempCircel = circels[i];
					tempCircel.circel_state = STATE.CHOOSED;
					circels[i] = tempCircel;
					circels[i].u_crystal.transform.GetChild(0).gameObject.SetActive(true);
				}
			}
		}
	}*/

	/// <summary>
	/// すべてのクリスタルの状態を選択されていないに変更する
	/// </summary>
	/*public void setUnChooseAll(){
		for(int i =0; i<circels.Count;i++){
			switch (circels[i].circel_state) {
			case STATE.CHOOSED:
				CircelInfo tempCircel = circels [i];
				tempCircel.circel_state = STATE.UNCHOOSE;
				circels [i] = tempCircel;
				circels [i].Object.transform.GetChild (0).gameObject.SetActive (false);
				break;
			case STATE.USING:
				circels [i].Object.GetComponent<Circel> ().DivideAfterUse = true;
				circels [i].Object.transform.GetChild (6).gameObject.SetActive (false);
				break;
			}
		}
		nowChoosedLevel = 0;
		//魔法ボタンを隠す
		HideButtonUI();

	}*/

	/// <summary>
	/// クリスタルの状態は選択されているなら正しいを返す
	/// </summary>
	/// <returns><c>true</c>, if choosed was ised, <c>false</c> otherwise.</returns>
	/// <param name="circel">Circel.</param>
	/*public bool isChoosed(GameObject circel){
		for (int i =0; i<circels.Count; i++) {
			if (i == circel.GetComponent<Circel> ().circelID) {
				if(circels[i].circel_state == STATE.CHOOSED){
					return true;
						}
				else return false;

			}
		}
		return false;
	}
	/// <summary>
	/// クリスタルの状態を取得する
	/// </summary>
	/// <returns>The circel state.</returns>
	/// <param name="obj">Object.</param>
	public STATE getCircelState(GameObject obj){
		for(int i =0; i<circels.Count;i++){
			if(i == obj.GetComponent<Circel>().circelID){
				return circels[i].circel_state;
			}
		}
		return STATE.NONE;
	}
	/// <summary>
	/// クリスタルの状態を変更する
	/// </summary>
	/// <param name="obj">クリスタル</param>
	/// <param name="state">状態</param>
	public void setCircelState(GameObject obj, STATE state){
		for(int i =0; i<circels.Count;i++){
			if(i == obj.GetComponent<Circel>().circelID){
				CircelInfo tempCircel = circels[i];
				tempCircel.circel_state = state;
				circels[i] = tempCircel;
				break;
			}
		}
	}*/
	/// <summary>
	/// 選択しているクリスタルのレベルにより、ボタンを表す
	/// </summary>
	void ShowButtonUI(){
		switch(nowChoosedLevel){
		case 1:
			//ButtonUI[0].SetActive(true);
            ButtonUI[0].GetComponent<Canvas>().enabled = true;
			break;
		case 2:
			//ButtonUI[1].SetActive(true);
            ButtonUI[1].GetComponent<Canvas>().enabled = true;
			break;
		case 3:
			//ButtonUI[2].SetActive(true);
            ButtonUI[2].GetComponent<Canvas>().enabled = true;
			break;
		case 4:
			//ButtonUI[3].SetActive(true);
            ButtonUI[3].GetComponent<Canvas>().enabled = true;
			break;
		case 5:
			//ButtonUI[4].SetActive(true);
            ButtonUI[4].GetComponent<Canvas>().enabled = true;
			break;
		}
	}/// <summary>
	/// UIのボタンを隠す
	/// </summary>
	public void HideButtonUI(){
		for(int i =0;i <ButtonUI.Length;i++){
			//ButtonUI[i].SetActive(false);
            ButtonUI[i].GetComponent<Canvas>().enabled = false;
		}
	}
	/// <summary>
	/// ダブルクリックしたら、一番近いクリスタルと合成する
	/// </summary>
	/// <param name="obj">Object.</param>
	/*public void QuickCompound(GameObject obj){
		//仮の配列を作る
		CircelInfo[] tempCircles = new CircelInfo[circels.Count];
		circels.CopyTo (tempCircles);
		for (int i = 0; i<tempCircles.Length;i++) {
			if(tempCircles[i].Object.GetComponent<Circel>().circelID == obj.GetComponent<Circel>().circelID){
				//ダブルクリックしたクリスタルを先頭い置く
				CircelInfo temp = tempCircles [0];
				tempCircles [0] = tempCircles [i];
				tempCircles [i] = temp;
				//距離に沿って順番を帰る
				for (int j = 1; j < tempCircles.Length; j++) {
					if (tempCircles [j].circel_state == STATE.CHOOSED || tempCircles [j].circel_state == STATE.UNCHOOSE) {
						if (tempCircles [j].Object.GetComponent<Circel> ().circelLevel != 1)
							continue;
						float dis = Vector3.Distance (tempCircles [0].Object.transform.position,
							            tempCircles [j].Object.transform.position);
						if (dis < distance) {
							temp = tempCircles [1];
							tempCircles [1] = tempCircles [j];
							tempCircles [j] = temp;
						}
						distance = dis;
					}
				}
				break;
			}
		}
		if (tempCircles [1].Object.GetComponent<Circel> ().circelLevel == 1 &&  (tempCircles [1].circel_state == STATE.CHOOSED || tempCircles [1].circel_state == STATE.UNCHOOSE))
			compound (tempCircles [0].Object, tempCircles [1].Object);
		distance = 10000;
	}*/
}
