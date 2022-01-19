using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShirtManager : MonoBehaviour
{
    public GameObject[] ExistingShirts, ExistingLogos, ColorButtons, ObjectsForDeactive;
    public Shirt CurrentShirt;
    public GameObject ShirtGO;
    public int CurrentShirtNumber, CurrentLogoNumber,CurrentLayer;
    public string TeamName;
    public LayerButton[] LayerButtons;
    public SpriteRenderer[] ShirtLayers;
    public Animator BGAnimator;
    public Transform ShirtPosAtLogoStage, LogoPosAtLogoStage, ShirtPosAtTeamStage, LogoPosAtTeamStage;
    public Text UpText;
    public GameObject TeamNextButton;
    public InputField TeamNameText;
    public Transform WhiteBox;
    public enum Stage {Shirts,Logos,TeamName }

    public Stage CurrentStage = Stage.Shirts;

    public class TeamSave
    {
        public int ShirtNumber, LogoNumber;
        public string TeamName;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        TeamSave save = new TeamSave();
        if (PlayerPrefs.HasKey("TeamSave"))
        {
            save = JsonUtility.FromJson<TeamSave>(PlayerPrefs.GetString("TeamSave"));
        }
        else
        {
            save.ShirtNumber = 0;
            save.LogoNumber = 0;
            save.TeamName = "";
            PlayerPrefs.SetString("TeamSave", JsonUtility.ToJson(save));
        }
        CurrentShirtNumber = save.ShirtNumber;
        CurrentLogoNumber = save.LogoNumber;
        TeamName = save.TeamName;
        TeamNameText.text = TeamName;
        SwitchShirt(0);
        LayerButtons[0].SetActive();
    }

    public void SetTeamName()
    {
        Text text = TeamNameText.transform.FindChild("Text").GetComponent<Text>();
        if (TeamNameText.text.Length <= 10)
        {
            text.color = Color.white;
            TeamNextButton.GetComponent<Image>().color = Color.white;
            TeamNextButton.GetComponent<Button>().interactable = true;
            TeamName = TeamNameText.text;
            
        }
        else
        {
            text.color = new Color(182f/255f, 35f/255f, 23f/255f, 1f);
            TeamNextButton.GetComponent<Image>().color = Color.gray;
            TeamNextButton.GetComponent<Button>().interactable = false;
        }
    }
    
    public void SetColor(Color color)
    {
        CurrentShirt.colors[CurrentLayer] = color;
        LayerButtons[CurrentLayer].InnerColor.color = color;
        ShirtLayers[CurrentLayer].color = color;
    }
    public void SetColor(Color color, int LayerNumber)
    {
        CurrentShirt.colors[LayerNumber] = color;
        LayerButtons[LayerNumber].InnerColor.color = color;
        ShirtLayers[LayerNumber].color = color;
    }
    public void SwitchShirt(int direction)
    {
        int SubSH(int CurrentNumber, GameObject[] ExistingTable)
        {
            CurrentNumber += direction;
            if (CurrentNumber < 0)
                CurrentNumber = ExistingTable.Length - 1;
            if (CurrentNumber == ExistingTable.Length)
                CurrentNumber = 0;
            CurrentShirt = ExistingTable[CurrentNumber].GetComponent<Shirt>();
            Vector2 shirtPos = ShirtGO.transform.position;
            Destroy(ShirtGO);
            ShirtGO = Instantiate(ExistingTable[CurrentNumber], shirtPos, Quaternion.identity,BGAnimator.transform);
            return CurrentNumber;
        }
        if (CurrentStage == Stage.Shirts)
        {
            CurrentShirtNumber = SubSH(CurrentShirtNumber, ExistingShirts);
        }

        if (CurrentStage == Stage.Logos)
        {
            CurrentLogoNumber = SubSH(CurrentLogoNumber, ExistingLogos);
        }
    }

    public void RandomColors()
    {
        for (int i = 0; i < ShirtLayers.Length; i++)
        {
            ShirtLayers[i].color = Color.cyan; //Костыль
            int k;
            while (true)
            {
                Color RandomColor = ColorButtons[Random.Range(0, ColorButtons.Length)].GetComponent<Image>().color;
                bool NotDublicate = true;
                for (int j = 0; j <= i; j++)
                {
                    if (ShirtLayers[j].color == RandomColor)
                        NotDublicate = false;
                }

                if (NotDublicate)
                {
                    CurrentShirt.colors[i] = RandomColor;
                    ShirtLayers[i].color = RandomColor;
                    LayerButtons[i].InnerColor.color = RandomColor;
                    break;
                }
            }
        }
    }

    public void SaveTeam()
    {
        TeamSave save = new TeamSave();
        save.ShirtNumber = CurrentShirtNumber;
        save.LogoNumber = CurrentLogoNumber;
        save.TeamName = TeamName;
        PlayerPrefs.SetString("TeamSave", JsonUtility.ToJson(save));
    }

    public void NextStage()
    {
        switch (CurrentStage)
        {
            case Stage.Shirts:
                CurrentShirt.SaveColors();
                TeamSave save = new TeamSave();
                GameObject BackShirt = Instantiate(ExistingShirts[CurrentShirtNumber], ShirtPosAtLogoStage.position, Quaternion.identity,ShirtPosAtLogoStage.parent);
                BackShirt.transform.localScale = ShirtPosAtLogoStage.transform.localScale;
                BackShirt.GetComponent<Shirt>().hanger.SetActive(false);
                ShirtGO = Instantiate(ExistingLogos[CurrentLogoNumber], LogoPosAtLogoStage.position, Quaternion.identity,LogoPosAtLogoStage.parent);
                ShirtGO.transform.localScale = LogoPosAtLogoStage.transform.localScale;
                CurrentShirt = ShirtGO.GetComponent<Shirt>();
                BGAnimator.Play("ToLogo");
                UpText.text = "Выбери лого";
                CurrentStage = Stage.Logos;
                break;
            case Stage.Logos:
                CurrentShirt.SaveColors();
                GameObject Shirt = Instantiate(ExistingShirts[CurrentShirtNumber], ShirtPosAtTeamStage.position, Quaternion.identity,ShirtPosAtTeamStage.parent);
                Shirt.transform.localScale = ShirtPosAtTeamStage.transform.localScale;
                Shirt.GetComponent<Shirt>().hanger.SetActive(false);
                GameObject Logo = Instantiate(ExistingLogos[CurrentLogoNumber], LogoPosAtTeamStage.position, Quaternion.identity,LogoPosAtTeamStage.parent);
                Logo.transform.localScale = LogoPosAtTeamStage.transform.localScale;
                BGAnimator.Play("ToTeamName");
                for (int i = 0; i < ObjectsForDeactive.Length; i++)
                    ObjectsForDeactive[i].SetActive(!ObjectsForDeactive[i].active);
                UpText.text = "Название команды";
                CurrentStage = Stage.TeamName;
                break;
            case Stage.TeamName:
                Application.Quit();
                break;
        }

        SaveTeam();
    }
    private void OnApplicationQuit()
    {
        SaveTeam();
    }
}
