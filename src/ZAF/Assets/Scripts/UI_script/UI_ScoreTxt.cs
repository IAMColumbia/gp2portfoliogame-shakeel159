using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreTxt : MonoBehaviour
{
    public LevelCount lvl;

    public TextMeshProUGUI roundTxt;

    public int level;

    // Start is called before the first frame update
    void Start()
    {
        lvl = GetComponent<LevelCount>();
    }

    // Update is called once per frame
    void Update()
    {
        level = lvl.currentRound;

        roundTxt.text = "Round " + level;
    }
}
