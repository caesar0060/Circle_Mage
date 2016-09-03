using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Xft;

public class MageLevelControl : MonoBehaviour {

	public string MahouName;
	public TextAsset level_data_text = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// <summary>
	/// Load Mage level data.
	/// </summary>
	/// <param name="mageObject">Mage object.</param>
	public void LoadLevelData(GameObject mageObject){
		Mage mage = mageObject.GetComponent<Mage> ();
        if (mage.level == 0)
            return;
		MageAttack mageAttack = mageObject.GetComponentInChildren<MageAttack> ();

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
			if (words[0].StartsWith ("#") || words[0].StartsWith ("@")) {	//ワードの先頭文字が＃か@なら
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
						mage.cost = int.Parse (word);
						break;
					case 2:
						mage.castTime = float.Parse (word);
						break;
					case 3:
						mage.power = int.Parse (word);
						break;
					case 4:
						mage.XZ_Blowing = float.Parse (word);
						break;
					case 5:
						mage.Y_Blowing = float.Parse (word);
						break;
					case 6: 
						mage.breakPower = float.Parse (word);
						break;
					case 8:
						mageObject.GetComponentInChildren<XffectComponent> ().Scale = float.Parse (word);
						try{
                            mageObject.transform.localScale = new Vector3(float.Parse(word), float.Parse(word), float.Parse(word));
						}catch (MissingComponentException e){
							Debug.LogError (e);
						}
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
}
