using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player pl;
    public LevelCount lvl;

    public TextMeshProUGUI roundTxt;
    public TextMeshProUGUI currancyTxt;

    public GameObject Menu;
    public GameObject MainMenu;
    public GameObject GameLoop;
    public MenuScript MenuScript;


    int mtarPrice = 150;
    int akPrice = 240;

    // Start is called before the first frame update
    void Start()
    {
        lvl = GetComponent<LevelCount>();
    }

    // Update is called once per frame
    void Update()
    {
        currancyTxt.text = "$" + pl.currency;

        if (pl.inMenu == true)
        {
            Menu.gameObject.SetActive(true);
        }
        if (MenuScript.isBuyingMtar)
        {
            if (pl.currency >= mtarPrice)
            {
                pl.currency -= mtarPrice;
                pl.weaponsAvilable.Add(pl.mTar);
            }
            pl.UI_BarreirText.enabled = false;
            MenuScript.isBuyingMtar = false;
        }
        if (MenuScript.isBuyingAK)
        {
            if (pl.currency >= akPrice)
            {
                pl.currency -= akPrice;
                pl.weaponsAvilable.Add(pl.Ak);
            }
            pl.UI_BarreirText.enabled = false;
            MenuScript.isBuyingAK = false;
        }
        if(pl.inMenu == false)
        {
            Menu.gameObject.SetActive(false);
        }
        roundTxt.text = "Round " + lvl.currentRound;
    }
    public void StartGame()
    {
        MainMenu.gameObject.SetActive(false);
        GameLoop.gameObject.SetActive(true);
    }
}

