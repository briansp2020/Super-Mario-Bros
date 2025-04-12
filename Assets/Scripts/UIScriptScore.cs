using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIScriptScore : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    

    
    void Update()
    {
        scoreUI.text= GameManager.Instance.score.ToString();
    }
}
