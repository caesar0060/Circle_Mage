using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerStatus : MonoBehaviour {
	public Text mesekiText;		//魔石の数を表示するテキスト
	public float p_HP;	//プレイヤーのHP
	public float p_MP;	//プレイヤーのMP

	public Text p_HP_text;		//HPを表示するテキスト
	public Text p_MP_text;		//MPを表示するテキスト
	public Slider p_HP_slider;	//HPゲージ
	public Slider p_MP_slider;	//MPゲージ
	public float powerRate;		//攻撃力の増加率
	public float costDownRate;	//コストの減少率
	public float CriticalRate;	//会心の一撃の攻撃力の増加率
	public GameObject shield;	//シールドのオブジェクト
    //[HideInInspector]
    public bool isWin = false;

	private float time = 10;
    private bool hpAlarm = false;
	private float nowMP;
	private NaviControl naviControl;


	// Use this for initialization
	void Start () {
		p_HP_slider.maxValue = p_HP;
		p_HP_slider.value = p_HP;
		p_MP_slider.maxValue = p_MP;
		p_MP_slider.value = p_MP;
		if (LoadLevel.Instance != null) {
			LoadLevel.Instance.LoadStatus ();
		}
		nowMP = p_MP;
		naviControl = GameObject.Find ("GameRoot").GetComponent<NaviControl> ();
        naviControl.ShowNavi("ゲーム開始", 3, "");
        StartCoroutine(CheckMP());
	}
	
	// Update is called once per frame
	void Update () {
		Player_HP_Update ();
		Player_MP_Update ();
		if(LoadLevel.Instance !=null)
		mesekiText.text = LoadLevel.Instance.maseki.ToString();
		if (p_HP <= 0)
			GameOver ();
        CheckHP();
	}

	//表示するHPの更新
	void Player_HP_Update(){
		p_HP_text.text = Mathf.Round (p_HP).ToString ();
		p_HP_slider.value = Mathf.Round (p_HP);
	}

	//表示するMPの更新
	void Player_MP_Update(){
		p_MP_text.text = Mathf.Round (p_MP).ToString ();
		p_MP_slider.value = Mathf.Round (p_MP);
	}

	/// <summary>
	/// Game over.
	/// </summary>
	public void GameOver(){
		GameObject.FindGameObjectWithTag ("GameOver").GetComponent<Image> ().enabled = true;
        naviControl.ShowNavi("ゲームオーバー", 3, "");
		Destroy (shield.gameObject);
		StartCoroutine (MoveScene (1));
	}
	/// <summary>
	/// Moves the scene.
	/// </summary>
	/// <returns>The scene.</returns>
	/// <param name="num">Scene Number.</param>
	public IEnumerator MoveScene(int num){
		yield return new WaitForSeconds (3);
		SceneManager.LoadScene (num);
		//Application.LoadLevel (num);
	}
    /// <summary>
    /// MPが激しい使うと警告します
    /// </summary>
    IEnumerator CheckMP()
    {
        while (true)
        {
            if (p_MP / nowMP * 100 <= 70)
            {
                naviControl.ShowNavi("使いすぎ", 3, "");
            }
            nowMP = p_MP;
            yield return new WaitForSeconds(time);
        }
    }
    /// <summary>
    /// Hpが少ない時、警告します
    /// </summary>
    void CheckHP()
    {
        if (!hpAlarm)
        {
            if (p_HP / p_HP_slider.maxValue * 100 <= 30)
            {
                naviControl.ShowNavi("HP不足", 3, "");
                hpAlarm = true;
            }
        }
    }
}
