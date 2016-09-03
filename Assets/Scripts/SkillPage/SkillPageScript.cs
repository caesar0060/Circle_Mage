using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SkillPageScript : MonoBehaviour {

	public GameObject[] magePage;	//魔法用のページ
	public GameObject[] circlePage;	//サークル用のページ
	public GameObject windowLeft;
	public GameObject windowRight;

	private MageStatus mageStatus;	
	private CircleStatus circleStatus;

	// Use this for initialization
	void Start () {
		mageStatus = this.gameObject.GetComponent<MageStatus> ();
		circleStatus = this.gameObject.GetComponent<CircleStatus> ();
		StartMenu ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/// <summary>
	/// ステージ選択画面に戻る
	/// </summary>
	public void Back(){
		SceneManager.LoadScene (1);
		//Application.LoadLevel (1);
	}
	/// <summary>
	/// 魔法のメーニュを表し、ccircleのメーニュを閉じる 
	/// </summary>
	public void MageButton(){
		//Mageのページを表示する
		for (int i = 0; i < magePage.Length; i++) {
			magePage [i].SetActive(true);
		}
		//circleのページを初期化する
		for (int i = 0; i < circlePage.Length; i++) {
			circlePage [i].SetActive (false);
		}
		for(int i= 0; i < circleStatus.circleText.Length; i++){
			circleStatus.circleText [i].text = "";
		}
		circleStatus.tempButton = null;
		circleStatus.buttonNum = -1;
		circleStatus.level = -1;
	}
	/// <summary>
	/// 魔法のメーニュを閉じ、ccircleのメーニュを表す
	/// </summary>
	public void CircleButton(){
		//Circleのページを表示する
		for (int i = 0; i < circlePage.Length; i++) {
			circlePage [i].SetActive (true);
		}
		//Mageのページを初期化する
		for (int i = 0; i < magePage.Length; i++) {
			magePage [i].SetActive (false);
		}
		for(int i= 0; i < mageStatus.mageText.Length; i++){
			mageStatus.mageText [i].text = "";
		}
		mageStatus.temp = null;
		mageStatus.mageObject = null;
		circleStatus.LoadCircleStatus ();
	}

	void StartMenu(){
		iTween.MoveTo (windowLeft, iTween.Hash ("x", -425,
			"islocal", true,
			"easeTupe", "easeInOutExpo",
			"time", 2
		));
		iTween.MoveTo (windowRight, iTween.Hash ("x", 593,
			"islocal", true,
			"easeTupe", "easeInOutExpo",
			"time", 2
		));
	}
}
