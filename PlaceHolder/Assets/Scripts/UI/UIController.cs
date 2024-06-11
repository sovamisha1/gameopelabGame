using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text chargeText;
    public Slider staminaSlider;
    public PlayerController player;
    private int charges;

    private float stamina;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update() { }

    public void SetCharges(int newCharges)
    {
        charges = newCharges;
        chargeText.text = "Осталось зарядов: " + charges;
    }

    private void UpdateUI() { }
}
