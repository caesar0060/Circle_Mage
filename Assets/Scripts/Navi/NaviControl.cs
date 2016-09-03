using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NaviControl : MonoBehaviour {

	public Sprite[] images;
	public GameObject navi;
	public Image playerImage;
    public Image hukidasiImage;
	public Text hukidasi;
	public TextAsset naviData;
	private float timer = 0;
	public float removeTime;	//naviを消す時間
    private bool onNavi = false;    //ナビしているかどうか

	// Use this for initialization
	void Start () {
        hukidasi.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (onNavi)
            timer += Time.deltaTime;
		if (timer >= removeTime)
			StopNavi ();
	}
    /// <summary>
    /// ナビスタート
    /// </summary>
    /// <param name="key">対応するキー</param>
    /// <param name="time">ナビを消す時間</param>
    ///  /// <param name="name">魔法の名前</param>
	public void ShowNavi(string key, float time, string name){
		timer = 0;
        removeTime = time;
        if (!onNavi)
            StartNavi();
        if (name != "")
            hukidasi.text += name + ShowText(key);
        else
          hukidasi.text += ShowText(key);
        string[] h_lines = hukidasi.text.Split('\n');
        if (h_lines.Length >= 4)
        {
            h_lines[1] = h_lines[2];
            h_lines[2] = h_lines[3];
            hukidasi.text = h_lines[1] + "\n" + h_lines[2] + "\n";
        }
		
	}
    /// <summary>
    /// イメージを表す
    /// </summary>
	void StartNavi(){
        onNavi = true;
            iTween.MoveTo(navi, iTween.Hash("x", 300,
                "islocal", true,
                "easeTupe", "easeOutExpo",
                "time", 0.5
            ));
	}
    /// <summary>
    /// イメージを消す
    /// </summary>
    void StopNavi(){
		iTween.MoveTo (navi, iTween.Hash ("x", 900,
			"islocal", true,
			"easeTupe", "easeInExpo",
			"time", 2
		));
        hukidasi.text = "";
        onNavi = false;
	}
    /// <summary>
    /// 内容を表示
    /// </summary>
    string ShowText(string key)
    {
       
        //テキストダータお、文字列として取り込む
        string level_texts = naviData.text;

        //改行ごとに分割し、文字列の配列に入れる
        string[] lines = level_texts.Split('\n');

        //lines内お各行に対して、順番に処理していくループ
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i] == "")
            {	//行が空っぽなら
                continue;		//以下の処理はせずいループの先頭にジャンプ
            }
            string[] words = lines[i].Split();
            if (words[0].StartsWith("@"))
            {	//ワードの先頭文字が@なら
                if (words[1] == key)
                {
                    playerImage.sprite = images[int.Parse(words[2])];
                    return  lines[i + 1] + "\n";
                }
            }
            if (words[0].StartsWith("#"))
            {	//ワードの先頭文字が＃なら
                continue;						//ループを脱出
            }
        }
        return "Error";
    }	
}
