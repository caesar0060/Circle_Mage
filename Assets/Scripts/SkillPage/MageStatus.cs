using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MageStatus: MonoBehaviour {
	
	public Text[] mageText;			//魔法用テキスト
	public Text[] LevelUpText;		//レベルアップ用テキスト
	public Text masekiText;			//魔法石用テキスト
	public GameObject levelUpMenu;
	[HideInInspector] public GameObject mageObject = null;		//レベルをアップする魔法
	[HideInInspector] public string cost = null;						//必要な魔法石
	[HideInInspector] public int totalCost = 0;					//全部必要あ魔法石 
	[HideInInspector] public GameObject temp;					//選択したボタンの情報を保存

	// Use this for initialization
	void Start () {
		LoadLevel loadLevel = LoadLevel.Instance;
		if (LoadLevel.Instance != null)
			masekiText.text = loadLevel.maseki.ToString ();
		else
			masekiText.text = "5000";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// 魔法のステータスを読み込み
	/// </summary>
	/// <param name="button">Button.</param>
	public void LoadLevelData(GameObject button){
		TextAsset level_data_text;
		//押したボタンを保存する
		temp = button;
		mageObject = button.GetComponent<ButtonSprit> ().Mage;					//魔法のオブジェクト
		mageText[0].text = button.name;			//名前
		level_data_text = button.GetComponent<ButtonSprit> ().level_data_text;	//レベルのデータ	
		Mage mage = mageObject.GetComponent<Mage> ();	//scriptを取得
		mageText [1].text = mage.level.ToString ();	//レベル

		//テキストダータお、文字列として取り込む
		string level_texts = level_data_text.text;

		//改行ごとに分割し、文字列の配列に入れる
		string[] lines = level_texts.Split('\n');

		//lines内お各行に対して、順番に処理していくループ
		foreach (var line in lines) {
			if (line == "") {	//行が空っぽなら
				continue;		//以下の処理はせずいループの先頭にジャンプ
			}
			string[] words = line.Split ();
			if(words[0].StartsWith ("@")){	//ワードの先頭文字が@なら
				string[] des = line.Split('@');
				mageText [6].text = des [1];
				continue;
			}
			if (words[0].StartsWith ("#")) {	//ワードの先頭文字が＃なら
				continue;						//ループを脱出
			}
			if (mage.level == int.Parse (words [0])) {//レベルが違うなら

				int n = 0;

				//words内の各ワードに対して、順番に処理していくループ
				foreach (var word in words) {

					if (word == "") {				//ワードが空っぽなら
						continue;					//ループの先頭にジャンプ
					}


					//「n」の値を0,1,2,...と変化させていく
					//各ワードを数値に変換し、それぞれ格納する
					switch (n) {
					case 1:
						mageText[4].text = word;	//MP
						break;
					case 3:
						mageText[2].text = word;	//パーワ
						break;
					case 7:
						mageText[5].text = word;	//ヒット数
						break;
					case 8:
						mageText[3].text = word;	//距離
						break;
					case 9:
						cost = word; 				//コスト
						break;
					default:
						break;
					}
					n++;
				}
				break;
			}
		}
	}
	/// <summary>
	/// Loads the level up menu.
	/// </summary>
	public void LoadLeveLUpMenu(){
		if (mageObject != null) {
			if (temp.GetComponent<ButtonSprit> ().needCircle <= LoadLevel.Instance.GetCircleNum ()) {
				levelUpMenu.SetActive (true);
				//レベルやコストを表示
				LevelUpText [0].text = mageObject.GetComponent<Mage> ().level.ToString ();
				if (cost != "ー")
					LevelUpText [1].text = (mageObject.GetComponent<Mage> ().level + 1).ToString ();
				else
					LevelUpText [1].text = cost;
				LevelUpText [2].text = cost;
			}
		}
	}
	/// <summary>
	/// level upのウィンドウを閉じる
	/// </summary>
	public void Close(){
		levelUpMenu.SetActive (false);
	}
	/// <summary>
	/// レベルアップする
	/// </summary>
	public void LevelUP(){
		if (cost != "ー" && mageObject != null) {
			if (LoadLevel.Instance.maseki >= int.Parse (cost)) {
				LoadLevel.Instance.maseki -= int.Parse (cost);
				mageObject.GetComponent<Mage> ().level += 1;
				temp.GetComponent<ButtonSprit> ().ShowButtonLevel ();
				LoadLevelData (temp);
				masekiText.text = LoadLevel.Instance.maseki.ToString ();
			}
		}
	}
}
