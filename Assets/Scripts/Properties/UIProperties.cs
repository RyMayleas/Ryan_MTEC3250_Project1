using UnityEngine;
using TMPro;
using UnityEngine.TextCore.Text;

public class UIProperties : MonoBehaviour
{
    public static UIProperties inst;
    public TMP_FontAsset font;
    public TMP_FontAsset titleFont;
    [Space(10)]

    public bool enableGameTitle = true;
    public string gameTitle;
    [Space(10)]
    public string crateLabel;
    public string stepsLabel;
    [Space(10)]

    public Color timerColor;
    public Color titleColor;
    public Color textColor;
    [Space(10)]
    public Color restartButtonColor;
    public Color restartTextColor;
    [Space(10)]
    public Color backPanelColor;
    public Sprite backPanelImage;
    public bool spriteColorOverride;

    private void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(gameObject);
    }


}
