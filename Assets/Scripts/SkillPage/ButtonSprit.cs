using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonSprit : MonoBehaviour {

	[HideInInspector]
	public int level;			//レベル
	public int needCircle;		//circle何個学んだら解放
	public GameObject Mage;		//ボタンに対応する魔法
	private Text textCom;		//自分が持ってるテキストを取得
	public Sprite image;		//学んだあとの画像
	public TextAsset level_data_text = null;	//テキストアイル

	// Use this for initialization
	void Start () {
		ShowButtonLevel ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/// <summary>
	/// 学んだ魔法の画像を変える
	/// </summary>
	public void ChangeImage(){
			this.GetComponent<Image> ().sprite = image;
	}

	public void ShowButtonLevel(){
		if (Mage != null) {
			//自分が持ってるテキストを取得
			textCom = this.GetComponentInChildren<Text> ();
			textCom.text = Mage.GetComponent<Mage> ().level.ToString ();
			level = Mage.GetComponent<Mage> ().level;
			if (level >= 1)
				ChangeImage ();
		}
	}
}
