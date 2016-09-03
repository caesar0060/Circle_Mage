using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CircleStatus : MonoBehaviour {

	//クリスタル用テキスト
	public Text[] circleText;
	//クリスタル用button
	public Button[] circleButtons;	
	public GameObject playerStatusControl;
	private StatusControl statusControl;
	//選択したボタンの情報を保存
	[HideInInspector] public GameObject tempButton;
	//レベルアップのコスト
	private string cost = null;
	//ボタンの配列のどのボタンを示す
	[HideInInspector]public  int buttonNum = -1;
	public Text masekiText;			//魔法石用テキスト
	public GameObject levelUpMenu;
	public Text[] LevelUpText;		//レベルアップ用テキスト
	[HideInInspector]public int level = -1;

	// Use this for initialization
	void Start () {
		statusControl = playerStatusControl.GetComponent<StatusControl> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/// <summary>
	/// すべてのcircleのレベルを取得する
	/// </summary>
	public void LoadCircleStatus(){
		for (int i = 0; i < statusControl.statusTexts.Length; i++) {
			//テキストダータお、文字列として取り込む
			string level_texts = statusControl.statusTexts[i].text;
			//改行ごとに分割し、文字列の配列に入れる
			string[] lines = level_texts.Split('\n');
			foreach (var line in lines) {
				if (line == "") {	//行が空っぽなら
					continue;		//以下の処理はせずいループの先頭にジャンプ
				}
				string[] words = line.Split ();
				if (words[0].StartsWith ("#")) {	//ワードの先頭文字が＃なら
					continue;						//ループを脱出
				}
				switch (i) {
				case 0:	//MP
					if (statusControl.status[i] == int.Parse (words [0])) {
						ButtonSprit buttonSprit = circleButtons [0].GetComponent<ButtonSprit> ();
						buttonSprit.level = int.Parse (words [0]);
						if (buttonSprit.level > 0)
							buttonSprit.ChangeImage ();
						circleText [4].text = words [1].ToString ();
					}
					break;
				case 1:	//Power
					if (statusControl.status[i] == int.Parse (words [0])) {
						ButtonSprit buttonSprit = circleButtons [1].GetComponent<ButtonSprit> ();
						buttonSprit.level = int.Parse (words [0]);
						if (buttonSprit.level > 0)
							buttonSprit.ChangeImage ();
						circleText [6].text = words [1].ToString ();
					}
					break;
				case 2:	//Critical
					if (statusControl.status[i] == int.Parse (words [0])) {
						ButtonSprit buttonSprit = circleButtons [2].GetComponent<ButtonSprit> ();
						buttonSprit.level = int.Parse (words [0]);
						if (buttonSprit.level > 0)
							buttonSprit.ChangeImage ();
						circleText [10].text = words [1].ToString ();
					}
					break;
				case 3:	//CostDown
					if (statusControl.status[i] == int.Parse (words [0])) {
						ButtonSprit buttonSprit = circleButtons [3].GetComponent<ButtonSprit> ();
						buttonSprit.level = int.Parse (words [0]);
						if (buttonSprit.level > 0)
							buttonSprit.ChangeImage ();
						circleText [8].text = words [1].ToString ();
					}
					break;
				case 4:	//HP
					if (statusControl.status[i] == int.Parse (words [0])) {
						ButtonSprit buttonSprit = circleButtons [4].GetComponent<ButtonSprit> ();
						buttonSprit.level = int.Parse (words [0]);
						if (buttonSprit.level > 0)
							buttonSprit.ChangeImage ();
						circleText [2].text = words [1].ToString ();
					}
					break;
				default:
					break;
				}
			}
		}
	}
	/// <summary>
	/// 押したボタンのcircleの値を取得する
	/// </summary>
	/// <param name="button">Button.</param>
	public void LoadLevelStatus(GameObject button){
		TextAsset level_data_text;
		//押したボタンを保存する
		tempButton = button;
		string[] data;
		circleText [0].text = button.name;		//名前
		level_data_text = button.GetComponent<ButtonSprit> ().level_data_text;	//レベルのデータ	
		//ボタンにより、違う情報を表示する
		switch (button.name) {
		case "Omnipotence":
			RemovePlusText ();
			level = circleButtons [2].GetComponent<ButtonSprit> ().level;
			circleText [1].text = level.ToString ();
			data = TextReader (level_data_text, level);
			circleText [11].text = data [1];
			cost = data [2];
			buttonNum = 2;
			break;
		case "Sprit":
			RemovePlusText ();
			level = circleButtons [0].GetComponent<ButtonSprit> ().level;
			circleText [1].text = level.ToString ();
			data = TextReader (level_data_text, level);
			circleText [5].text = data [1];
			cost = data [2];
			buttonNum = 0;
			break;
		case "Death":
			RemovePlusText ();
			level = circleButtons [1].GetComponent<ButtonSprit> ().level;
			circleText [1].text = level.ToString ();
			data = TextReader (level_data_text, level);
			circleText [7].text = data [1];
			cost = data [2];
			buttonNum = 1;
			break;
		case "Birth":
			RemovePlusText ();
			level = circleButtons [3].GetComponent<ButtonSprit> ().level;
			circleText [1].text = level.ToString ();
			data = TextReader (level_data_text, level);
			circleText [9].text = data [1];
			cost = data [2];
			buttonNum = 3;
			break;
		case "Life":
			RemovePlusText ();
			level = circleButtons [4].GetComponent<ButtonSprit> ().level;
			circleText [1].text = level.ToString ();
			data = TextReader (level_data_text, level);
			circleText [3].text = data [1];
			cost = data [2];
			buttonNum = 4;
			break;
		default:
			break;
		}
	}
	/// <summary>
	/// テキストをと読込む
	/// </summary>
	/// <returns>The reader.</returns>
	/// <param name="level_data_text">Level data text.</param>
	/// <param name="level">Level.</param>
	string[] TextReader(TextAsset level_data_text, int level){
		//return用変数
		string[] data = new string[3];
		//テキストダータお、文字列として取り込む
		string level_texts = level_data_text.text;

		//改行ごとに分割し、文字列の配列に入れる
		string[] lines = level_texts.Split ('\n');

		//lines内お各行に対して、順番に処理していくループ
		foreach (var line in lines) {
			if (line == "") {	//行が空っぽなら
				continue;		//以下の処理はせずいループの先頭にジャンプ
			}
			string[] words = line.Split ();
			if (words [0].StartsWith ("#")) {	//ワードの先頭文字が＃なら
				continue;						//ループを脱出
			}
			if (level == int.Parse (words [0])) {//レベルが違うなら

				int n = 0;

				//words内の各ワードに対して、順番に処理していくループ
				foreach (var word in words) {

					if (word == "") 			//ワードが空っぽなら
						continue;				//ループの先頭にジャンプ
					//「n」の値を0,1,2,...と変化させていく
					//各ワードを数値に変換し、それぞれ格納する
					switch (n) {
					case 1:
						data [0] = word;		//基本値
						break;
					case 2:
						data [1] = word;		//増加値
						break;
					case 3:
						data [2] = word;		//コスト
						break;
					default:
						break;
					}
					n++;
				}
				break;
			}
		}
		return data;
	}
	/// <summary>
	/// すべての増加値を消す
	/// </summary>
	void RemovePlusText(){
		circleText [3].text = "";
		circleText [5].text = "";
		circleText [7].text = "";
		circleText [9].text = "";
		circleText [11].text = "";
		buttonNum = -1;
	}
	/// <summary>
	/// レベルアップする
	/// </summary>
	public void CircleLevelUp(){
		if (cost != "ー" && buttonNum!=-1) {
			if (LoadLevel.Instance.maseki >= int.Parse (cost)) {
				LoadLevel.Instance.maseki -= int.Parse (cost);
				statusControl.status [buttonNum] += 1;
				tempButton.GetComponent<ButtonSprit> ().ChangeImage ();
				LoadCircleStatus ();
				LoadLevelStatus (tempButton);
				masekiText.text = LoadLevel.Instance.maseki.ToString ();
			}
		}
	}
	/// <summary>
	/// Loads the level up menu.
	/// </summary>
	public void LoadLeveLUpMenu(){
		if (buttonNum != -1) {
			levelUpMenu.SetActive (true);
			//レベルやコストを表示
			LevelUpText [0].text = level.ToString ();
			if (cost != "ー")
				LevelUpText [1].text = (level + 1).ToString ();
			else
				LevelUpText [1].text = cost;
			LevelUpText [2].text = cost;
		}

	}
}
