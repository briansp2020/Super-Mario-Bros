using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIScriptLives: MonoBehaviour
{
    public TextMeshProUGUI livesUI;
    private int lifeValue;

    void Start(){
        //livesUI=GetComponent<TMP_Text>();
    }
    void Update()
    {
        lifeValue=GameManager.Instance.lives;
        livesUI.text= lifeValue.ToString();
        //lifeValue.ToString();
    }
}
