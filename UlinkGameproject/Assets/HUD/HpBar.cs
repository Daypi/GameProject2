using UnityEngine;
using System.Collections;

public class HpBar : MonoBehaviour {
    public  float scale = 1f;
    public string pname = "Player";
    private Camera camera;
    public Texture2D maxHealthtex;
    public Texture2D currentHealthtex;
    public bool enabled;
    Font font;

    void Start()
    {
		if (Network.isServer == true)
			this.enabled = false;
        camera = Camera.main;
        font = GameObject.Find("PlayerHUD").GetComponent<FontHolder>().font;
    }

	// Update is called once per frame
	void OnGUI () {
        if (enabled)
        {
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.normal.textColor = Color.black;
            centeredStyle.font = font;
            Vector3 onScreenPosition = camera.WorldToScreenPoint(this.transform.position);
            Rect displayRect = new Rect(onScreenPosition.x - 50, Screen.height - (onScreenPosition.y - 15), 100, 10);
            Rect namePosition = new Rect(onScreenPosition.x - 50, Screen.height - (onScreenPosition.y -25), 100, 100);
            Rect borderNamePosition = new Rect(onScreenPosition.x - 50 + 1, Screen.height - (onScreenPosition.y - 25) + 1, 100, 100);
            GUI.Label(borderNamePosition, pname);
            centeredStyle.normal.textColor = Color.yellow;
            GUI.Label(namePosition, pname);
            GUI.DrawTexture(displayRect, maxHealthtex);
            displayRect.width = scale * 100;
            GUI.DrawTexture(displayRect, currentHealthtex);
        }
    }

    public void AdjustScale(float adj)
    {
        if (adj > 0)
            adj = 0;
        else if (adj > 1)
            adj = 1;

        scale = adj;
    }
}
