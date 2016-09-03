using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CoolDown : MonoBehaviour {

	public float coolDown;		//魔法のクールダウンタイム
	Button btn;
	private float timer = 0;
    private NaviControl naviControl;
    private CircelControl circleControl;

    public bool isCoolDown = false;    //クールダウンしているかどうか
    public GameObject mage;

	// Use this for initialization
	void Start () {
		btn = this.GetComponent<Button> ();
        naviControl = GameObject.Find("GameRoot").GetComponent<NaviControl>();
        circleControl = GameObject.Find("GameRoot").GetComponent<CircelControl>();
        if (mage.GetComponent<Mage>().level == 0)
        {
            btn.interactable = false;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (coolDown != 0)
        {
            if (isCoolDown)
                timer += Time.deltaTime;
            if (timer >= coolDown)
            {
                naviControl.ShowNavi("準備完了", 2, this.gameObject.name);
                btn.interactable = true;
                isCoolDown = false;
                timer = 0;
            }
        }
	}
	public void StartCoolDown(){
		if (btn.interactable) {
			btn.interactable = false;
		}
	}
}
