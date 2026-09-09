using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager inst;
    public GameObject background;
    public TextMeshProUGUI gameTitle;
    public Image uiPanel;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI cratesRemainingText;
    public TextMeshProUGUI cratesRemainingLabel;
    public TextMeshProUGUI restartText;
    public Image restartImage;
    private int cratesRemaining;
    public TextMeshProUGUI stepsTakenText;
    public TextMeshProUGUI stepsTakenLabel;
    private int stepsTaken = 0;
    
    void Awake()
    {
        if (inst == null) inst = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitUI();

        
        AudioManager.inst.PlayMusic(Sounds.inst.musicVolume);
        AudioManager.inst.PlayAmbience(Sounds.inst.ambienceVolume);
    }

    private void InitUI()
    {
        uiPanel.color = UIProperties.inst.backPanelColor;
        if (UIProperties.inst.backPanelImage != null)
        {
            uiPanel.sprite = UIProperties.inst.backPanelImage;
            if (!UIProperties.inst.spriteColorOverride)
            {
                uiPanel.color = Color.white;
                
            }
        }

        var titleFont = UIProperties.inst.titleFont !=null? UIProperties.inst.titleFont: UIProperties.inst.font;

        gameTitle.text = UIProperties.inst.gameTitle;
        gameTitle.color = UIProperties.inst.titleColor;
        gameTitle.font = titleFont;

        timerText.color = UIProperties.inst.timerColor;
        timerText.font = UIProperties.inst.font;

        restartText.color = UIProperties.inst.restartTextColor;
        restartImage.color = UIProperties.inst.restartButtonColor;
        restartText.font = UIProperties.inst.font;

        cratesRemaining = GameProperties.inst.crateCount;
        cratesRemainingText.text = cratesRemaining.ToString("00");
        cratesRemainingLabel.text = UIProperties.inst.crateLabel;
        cratesRemainingLabel.color = UIProperties.inst.textColor;
        cratesRemainingText.color = UIProperties.inst.textColor;
        cratesRemainingText.font = UIProperties.inst.font;
        cratesRemainingLabel.font = UIProperties.inst.font;

        stepsTakenLabel.text = UIProperties.inst.stepsLabel;
        stepsTakenLabel.color = UIProperties.inst.textColor;
        stepsTakenText.color = UIProperties.inst.textColor;
        stepsTakenLabel.font = UIProperties.inst.font;
        stepsTakenText.font = UIProperties.inst.font;

        gameTitle.gameObject.SetActive(UIProperties.inst.enableGameTitle);

        if (VisualProperties.inst.backgroundImage != null)
        {
            background.GetComponent<RawImage>().texture = VisualProperties.inst.backgroundImage.texture;
            background.SetActive(true);
        }

    }

    private void OnEnable()
    {
        Tile.CrateDestroyed += UpdateCrateTextUI;
        PlayerControl.PlayerMoved += UpdateStepsTakenUI;
    }
    private void OnDisable()
    {
        Tile.CrateDestroyed -= UpdateCrateTextUI;
        PlayerControl.PlayerMoved -= UpdateStepsTakenUI;
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateCrateTextUI(Tile tile)
    {
        if (cratesRemaining < 1) return;
        cratesRemaining--;
        cratesRemainingText.text = cratesRemaining.ToString("00");
    }

    private void UpdateStepsTakenUI()
    {
        stepsTaken++;
        stepsTakenText.text = stepsTaken.ToString("000");
    }


}
