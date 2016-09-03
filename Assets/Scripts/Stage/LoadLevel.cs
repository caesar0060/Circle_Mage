using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class LoadLevel : MonoBehaviour {

	//LoadLevelのインタンス
	public static LoadLevel Instance{
		get; set;
		}
	//魔石の数
	[HideInInspector]public int maseki;
	//Mageのprefabsを保存する配列
	public GameObject[] mages;	
	public GameObject playerStatusControl;
	private StatusControl statusControl;
	//データのテキスト用
	public TextAsset statusLevelText = null;
	public TextAsset mahou_level_text = null;
    [HideInInspector]  public int stageNum;
	void Awake(){
		
		if (Instance != null) {
			Destroy (gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}

	// Use this for initialization
	void Start () {
		statusControl = playerStatusControl.GetComponent<StatusControl> ();
		LoadMahouData ();
		LoadStatusLevel ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/// <summary>
	/// プレイヤーのステータスを取得
	/// </summary>
	public void LoadStatus(){
		PlayerStatus playerStatus = GameObject.Find ("Player").GetComponent<PlayerStatus> ();
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
				case 0:
					if (statusControl.status[i] == int.Parse (words [0])) {
						playerStatus.p_MP_slider.maxValue = int.Parse (words [1]);
						playerStatus.p_MP = float.Parse (words [1]);
					}
					break;
				case 1:
					if (statusControl.status[i] == int.Parse (words [0])) {
						playerStatus.powerRate = float.Parse (words [1]);
					}
					break;
				case 2:
					if (statusControl.status[i] == int.Parse (words [0])) {
						playerStatus.CriticalRate = float.Parse (words [1]);
					}
					break;
				case 3:
					if (statusControl.status[i] == int.Parse (words [0])) {
						playerStatus.costDownRate= float.Parse (words [1]);
					}
					break;
				case 4:
					if (statusControl.status[i] == int.Parse (words [0])) {
						playerStatus.p_HP_slider.maxValue = int.Parse (words [1]);
						playerStatus.p_HP = float.Parse (words [1]);
					}
					break;
				default:
					break;
				}
			}
		}
	}
	/// <summary>
	/// プレイヤーのステータスのレベルを取得
	/// </summary>
	void LoadStatusLevel(){
		//テキストダータお、文字列として取り込む
		string level_texts = statusLevelText.text;
		//改行ごとに分割し、文字列の配列に入れる
		string[] lines = level_texts.Split('\n');
		for (int i = 0; i < lines.Length; i++) {
			string[] words = lines [i].Split ();
			switch (i) {
			case 0:
				statusControl.status [i] = int.Parse (words [0]);
				break;
			case 1:
				statusControl.status[i] =int.Parse(words [0]);
				break;
			case 2:
				statusControl.status[i] =int.Parse(words [0]);
				break;
			case 3:
				statusControl.status[i] =int.Parse(words [0]);
				break;
			case 4:
				statusControl.status[i] =int.Parse(words [0]);
				break;
			default:
				break;
			}
		}
	}
	/// <summary>
	/// Get the circle number.
	/// </summary>
	/// <returns>The circle number.</returns>
	public int GetCircleNum(){
		int num = 0;
		for (int i = 0; i < statusControl.status.Length; i++) {
			if (statusControl.status [i] > 0)
				num++;
		}
		return num;
	}

	/// <summary>
	/// すべての魔法のレベルを取得
	/// </summary>
	void LoadMahouData(){
		//テキストダータお、文字列として取り込む
		string level_texts = mahou_level_text.text;

		//改行ごとに分割し、文字列の配列に入れる
		string[] lines = level_texts.Split('\n');


		for(int i=0;i<=mages.Length;i++){
			string[] words = lines [i].Split ();
			//魔法石を数を更新する
			if (i == mages.Length)
				maseki = int.Parse(words [0]);
			//プレハブのレベルを更新する
			else
				mages [i].gameObject.GetComponent<Mage> ().level = int.Parse(words [0]);
		}
	}
	/// <summary>
	/// すべて魔法のレベルのデータを保存するテキストを更新
	/// </summary>
	public void LevelUpdate(){
		//テキストを取得
		FileStream textFile = new FileStream ("Assets/Resources/Mage/MahouLevel.txt", FileMode.Open, FileAccess.Write);
		StreamWriter sw = new StreamWriter (textFile);
		for (int i = 0; i <= mages.Length; i++) {
			if (i == mages.Length) {
				sw.WriteLine (maseki + "\t" + "Maseki");
				Debug.Log (maseki + "\t" + "Maseki");
			}
			else {
				Mage mage = mages [i].GetComponent<Mage> ();
				sw.WriteLine (mage.level + "\t" + mages[i].name);
				Debug.Log (mage.level + "\t" + mages[i].name);
			}
		}
		sw.Close ();
	}
	/// <summary>
	/// Playerのステータスを更新する
	/// </summary>
	public void StatusUpdate(){
		//テキストを取得
		FileStream textFile = new FileStream ("Assets/Resources/Player/StatusLevel.txt", FileMode.Open, FileAccess.Write);
		StreamWriter sw = new StreamWriter (textFile);
		for (int i = 0; i < statusControl.status.Length; i++) {
			sw.WriteLine (statusControl.status[i] + "\t" + statusControl.statusTexts[i].name);
			Debug.Log (statusControl.status[i] + "\t" + statusControl.statusTexts[i].name);
		}
		sw.Close ();
	}
}
