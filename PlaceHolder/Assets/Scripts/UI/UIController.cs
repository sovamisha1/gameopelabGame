using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI Elements")]
    public Text chargeText;
    public Slider staminaSlider;
    public Slider hpSlider;
    public PlayerController player;

    private int charges;
    private float stamina;
    private float hp;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        chargeText = GameObject.FindWithTag("ChargesText").GetComponent<Text>();
        staminaSlider = GameObject.FindWithTag("StaminaSlider").GetComponent<Slider>();
        hpSlider = GameObject.FindWithTag("HpSlider").GetComponent<Slider>();
    }

    void Update()
    {
        stamina = player.GetParametrs("stamina");
        hp = player.GetParametrs("hp");
        UpdateUI();
    }

    public void SetCharges(int newCharges)
    {
        charges = newCharges;
        chargeText.text = "Осталось зарядов: " + charges;
    }

    private void UpdateUI()
    {
        staminaSlider.value = stamina;
        hpSlider.value = hp;
    }
}
