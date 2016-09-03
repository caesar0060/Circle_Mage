using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextControl : MonoBehaviour {


	[SerializeField] 
	private Text _uiText;		//UIテキストの参照を保つ

	[SerializeField][Range(0.001f, 0.5f)]
	public float intervalForCharacterDisplay = 0.05f;	//1文字の表示にかかる時間

	public bool isScenario = true;	//シナリオ中かどうかを判断
		
	private string currentText = string.Empty;	//現在の文字列
	private float timeUntilDisplay = 0;			//表示にかかる時間
	private float timeElapsed = 1;				//文字列の表示を開始した時間
	private int lastUpdateCharacter = -1;		//表示中の文字数


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//クリックから経過時間が想定表示時間の何％か確認し、表示文字数を出す
		int displayCharacterCount = (int)(Mathf.Clamp01 ((Time.time - timeElapsed) / timeUntilDisplay) * currentText.Length);
		//表示文字数が前回の表示文字数と異なるならテキストを更新
		if (displayCharacterCount != lastUpdateCharacter) {
			_uiText.text = currentText.Substring (0, displayCharacterCount);
			lastUpdateCharacter = displayCharacterCount;
		}
	}
	/// <summary>
	/// 次に表示する文字列をセットする
	/// </summary>
	public void SetNextLine(string text){
		currentText = text;

		//想定表示時間と現在の時刻をキャッシュ
		timeUntilDisplay = currentText.Length * intervalForCharacterDisplay;
		timeElapsed = Time.time;
		//時間カウントを初期化
		lastUpdateCharacter = -1;
	}
	/// <summary>
	/// 文字の表示が完了しているかどうか
	/// </summary>
	/// <value><c>true</c> if this instance is complete display text; otherwise, <c>false</c>.</value>
	public bool IsCompleteDisplayText{
		get { return Time.time > timeElapsed + timeUntilDisplay;}
	}

	/// <summary>
	/// 強制的に全文表示する
	/// </summary>
	public void ForceCompleteDisplaytext(){
		timeUntilDisplay = 0;
	}
}
