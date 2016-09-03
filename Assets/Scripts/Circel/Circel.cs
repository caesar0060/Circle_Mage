using UnityEngine;
using System.Collections;

public class Circel : MonoBehaviour {

	//クリスタルの状態
	public enum STATE{
		NONE = -1,		//未使用
		CHOOSED = 0,	//選択されている
		USING,			//魔法を使っている
		DENFENCE,		//防御中
		COMPOUNDED		//合成中
	};

	public int circelLevel;			//クリスタルのレベル
	public int circelID;			//クリスタルのID
	public float circelRadius;		//クリスタルの半径
	public STATE c_State;			//クリスタルの状態

	//private float compoundTimer;	//重ねる時間をカウント用の変数
	//public float compoundTime;		//合成する必要な時間

	//[HideInInspector] public float manaRecoverTimer= 0;//回復になるまでの時間をカウント用の変数

	//[HideInInspector]public bool onClick = false;	//ダブルクリックスタート
	//private float clickTimer = 0;					//時間内にダブルクリックしたら成立する
	
	//[HideInInspector] public GameObject compoundObject;	//どのクリスたつと合成
	[HideInInspector] public bool isMove = false;		//移動中かどうか
	//[HideInInspector] public bool compoundOK = false;	//合成できるかどうか
	//[HideInInspector] public bool DivideAfterUse = false;//分解する予定	

	// Use this for initialization
	void Start () {
		//StartCoroutine (ManaRecovery ());
	}

	// Update is called once per frame
	void Update () {
		/*CheckDoubleClick();
		if (circelLevel != circelControl.nowChoosedLevel && circelControl.getCircelState (this.gameObject) == MageCircel.STATE.CHOOSED) {
			circelControl.setCircelState (this.gameObject, MageCircel.STATE.UNCHOOSE);
			this.transform.GetChild (0).gameObject.SetActive (false);
		}
		if (DivideAfterUse && circelControl.getCircelState (this.gameObject) == MageCircel.STATE.CHOOSED) {
			circelControl.setCircelState (this.gameObject, MageCircel.STATE.UNCHOOSE);
			this.transform.GetChild (0).gameObject.SetActive (false);
			circelControl.HideButtonUI ();
			DivideAfterUse = false;
		}*/

		if (this.transform.position.x > 25.05f)
			this.transform.position = new Vector3 (25.05f, this.transform.position.y, this.transform.position.z);
		if (this.transform.position.x < -37.06f)
			this.transform.position = new Vector3 (-37.06f, this.transform.position.y, this.transform.position.z);
		if (this.transform.position.z > 12.09f)
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, 12.09f);
		if (this.transform.position.z < -18.5f)
			this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, -18.5f);

	}
	/// <summary>
	/// 一秒ごとに魔力を回復する関数
	/// </summary>
	/// <returns>The recovery.</returns>
	/*public IEnumerator ManaRecovery(){
		while (true) {
			if (manaRecoverTimer >= manaRecoverTime) {
				playerStatus.p_MP += playerStatus.p_MP_slider.maxValue * (manaRecoverPencent / 100) * circelLevel;
				if (playerStatus.p_MP > playerStatus.p_MP_slider.maxValue)
					playerStatus.p_MP = playerStatus.p_MP_slider.maxValue;
			}
			yield return new WaitForSeconds(1);
		}
	}

	/// <summary>
	/// クリスタルの重ねる時間を計算する
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerStay(Collider other){
		if (other.gameObject.CompareTag ("Circle")) {
			if (other.gameObject.GetComponent<Circel> ().c_State == STATE.CHOOSED) {
				compoundObject = other.gameObject;
			}
		}
	}
	/// <summary>
	/// クリスタルが離れる
	/// </summary>
	/// <param name="other">Other.</param>
	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("Circle")) {
			compoundTimer = 0;
			compoundOK = false;
			this.transform.GetChild (circelLevel).gameObject.SetActive (true);
			this.transform.GetChild (circelLevel + other.GetComponent<Circel>().circelLevel).gameObject.SetActive (false);
		}
	}

	void CheckDoubleClick(){
		if(onClick == true)
			clickTimer += Time.deltaTime;
		if(clickTimer >=0.3){
			onClick = false;
			clickTimer = 0;
		}
	}*/
	/// <summary>
	/// クリスタルの状態を取得する
	/// </summary>
	/// <returns>The circel state.</returns>
	/// <param name="obj">Object.</param>
	public STATE getCircelState(GameObject obj){
		return c_State;
	}
	/// <summary>
	/// クリスタルの状態を変更する
	/// </summary>
	/// <param name="obj">クリスタル</param>
	/// <param name="state">状態</param>
	public void setCircelState(STATE state){
		c_State = state;
	}
}
