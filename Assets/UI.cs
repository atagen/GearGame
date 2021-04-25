using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{
    public PlayerItems _playerItems;
    private GameObject _CrystalsUI;
    private GameObject _HealthUI;
    private TMPro.TextMeshProUGUI _crystalsText; 
    private TMPro.TextMeshProUGUI _healthText; 

    // Start is called before the first frame update
    void Start()
    {
        _CrystalsUI = gameObject.transform.Find("Crystals").gameObject;
        _HealthUI = gameObject.transform.Find("Health").gameObject;
        _crystalsText = _CrystalsUI.transform.Find("Cnt").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        _healthText = _HealthUI.transform.Find("Cnt").gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        _playerItems = FindObjectOfType<PlayerItems>();
    }

    // Update is called once per frame
    void Update()
    {
        _crystalsText.text =  _playerItems.mineral_count.ToString();
        _healthText.text =  _playerItems.player_health.ToString();
    }
}
