using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

[RequireComponent(typeof( TextControl))]
public class ScenarioManager : MonoBehaviour {

	public string LoadFileName;
	public Sprite[] Images;

	private string[] m_scenarios;	//シナリオを格納する
	private int m_currentLine =0;
	private bool m_isCallPreload = false;

	public GameObject playerImage;
	public GameObject hukidasi;

	private TextControl m_textControl;
	private	StageSelect stageSelect;
	//private CommandController m_commandController;
	// Use this for initialization
	void Start () {
		m_textControl = this.GetComponent<TextControl> ();
		stageSelect = GameObject.Find ("Main Camera").GetComponent<StageSelect> ();

		UpdateLines (LoadFileName);
		StartScenario ();
	}
	
	// Update is called once per frame
	void Update () {
		//すべて表示したら
		if (m_textControl.IsCompleteDisplayText) {
			//まだ次の行があったら
			if (m_currentLine < m_scenarios.Length) {
				//次の行を読む
				if (!m_isCallPreload) {
					m_isCallPreload = true;
				}
				if (Input.GetMouseButtonDown (0)) {
					RequestNextLine ();
				}
			} else {
				//終わり
				m_textControl.isScenario = false;
				FinishScenario ();
			}
		} else {
			//すべて表示していなかったら
			if(Input.GetMouseButtonDown(0)){
				m_textControl.ForceCompleteDisplaytext();
			}
		}
	}
	//次の行を読む
	void RequestNextLine(){
		if (m_textControl.isScenario == true) {
			var currentText = m_scenarios [m_currentLine];

			m_textControl.SetNextLine (CommandProcess (currentText));
			m_currentLine++;
			m_isCallPreload = false;
		}
	}

	//新しいラインを取得する
	public void UpdateLines(string fileName){
		var scenarioText = Resources.Load<TextAsset> ("Scenario/" + fileName);

		if (scenarioText == null) {
			Debug.LogError("Scenario file not found");
			Debug.LogError("ScenarioManger not active");
			enabled = false;
			return;
		}
		m_scenarios = scenarioText.text.Split (new string[]{"@br"}, System.StringSplitOptions.None);
		m_currentLine = 0;

		Resources.UnloadAsset (scenarioText);
	}
	//Lineにより、プロセスを執行する
	private string CommandProcess(string line){
		var lineReader = new StringReader (line);
		var lineBulider = new StringBuilder ();
		var text = string.Empty;
		while ((text = lineReader.ReadLine()) !=null) {
			var commentCharacterCount = text.IndexOf("/");
			if(commentCharacterCount != -1){
				text = text.Substring(0, commentCharacterCount);
			}

			if(!string.IsNullOrEmpty(text)){
				if(text[0]=='@'){
					int imageNum = int.Parse(GetImageNum(text));
					stageSelect.playerImage.sprite = Images[imageNum];
					m_currentLine++;
					var currentText = m_scenarios [m_currentLine]; 
					text = CommandProcess(currentText);
				}
				lineBulider.AppendLine(text);
			}
		}
		return lineBulider.ToString ();
	}
	/// <summary>
	/// Get the image number.
	/// </summary>
	/// <returns>The image number.</returns>
	/// <param name="line">Line.</param>
	private string GetImageNum(string line){
		var tag = Regex.Match(line, "@(\\S+)");
		return tag.Groups [1].ToString ();
	}
	/// <summary>
	/// Starts the scenario.
	/// </summary>
	public void StartScenario(){
		iTween.MoveTo (hukidasi, iTween.Hash ("x", -71,
			"islocal", true,
			"easeTupe", "easeOutExpo",
			"time", 1,
			"oncomplete", "RequestNextLine",
			"oncompletetarget", this.gameObject
		));
		iTween.MoveTo (playerImage, iTween.Hash ("x", 700,
			"islocal", true,
			"easeTupe", "easeOutExpo",
			"time", 1
			));
	}

	public void FinishScenario(){
		iTween.MoveTo (hukidasi, iTween.Hash ("x", -1700,
			"islocal", true,
			"easeTupe", "easeInExpo",
			"time", 1
		));
		iTween.MoveTo (playerImage, iTween.Hash ("x", 2000,
			"islocal", true,
			"easeTupe", "easeInExpo",
			"time", 1
		));
	}
}
