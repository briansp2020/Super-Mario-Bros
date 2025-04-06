using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIScriptWorld: MonoBehaviour
{
    public TextMeshProUGUI worldUI;
    void Update()
    {
        
        worldUI.text= (GameManager.Instance.world.ToString()+"-"+GameManager.Instance.stage.ToString());
    }
}
