using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class SetNameButton : MonoBehaviour {
    void Start() {
        Button button = this.GetComponent<Button>();
        UnityAction ac = new UnityAction(this.OnClickListener);
        button.onClick.AddListener(ac);
    }

    public void OnClickListener() {
        string str = GameObject.Find("Game").transform.Find("UICamera").transform.Find("FistGameCanvas").transform.Find("NameInputField").transform.Find("Text").GetComponent<Text>().text;
        GameObject.Find("Game").GetComponent<MainGame>().setNewPlayer(str);
    }

}
