using UnityEngine;
using System.Collections;

public class BossNavi: MonoBehaviour
{
    private NaviControl naviControl;
	public Sprite nameSprite;

    // Use this for initialization
    void Start()
    {
        naviControl = GameObject.Find("GameRoot").GetComponent<NaviControl>();
        naviControl.ShowNavi(this.gameObject.name, 3, "");
    }

    // Update is called once per frame
    void Update()
    {
 
    }
}
