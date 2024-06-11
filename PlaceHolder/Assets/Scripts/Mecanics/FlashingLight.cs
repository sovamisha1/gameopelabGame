using System.Collections;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    private InputManager inputManager;

    public Light lightSource;
    public float maxRangeLightSource = 30.0f;
    public float minRangeLightSource = 5.0f;
    public float maxIntensityLightSource = 10.0f;
    public float minIntensityLightSource = 1.0f;
    public float flashDuration = 0.3f;
    public float fadeOutDuration = 0.2f;

    public int maxUses = 3;
    public float rechargeTime = 10.0f;

    private int currentUses;
    private bool canFlash = true;
    private bool isRecharging = false;

    void Awake()
    {
        currentUses = maxUses;
    }

    void Update()
    {
        if (lightSource.range > minRangeLightSource)
        {
            lightSource.range -= 4 * Time.deltaTime;
        }

        // Ассинхронно выполняем вспышку света
        if (Input.GetKeyDown(inputManager.GetKeyForAction("Flash")))
        {
            if (canFlash && currentUses > 0)
            {
                StartCoroutine(FlashLight());
            }
        }

        if (Input.GetKeyDown(inputManager.GetKeyForAction("Recharge")) && !isRecharging)
        {
            StartCoroutine(Recharge());
        }
    }

    private IEnumerator FlashLight()
    {
        canFlash = false;
        currentUses--;

        float initialRange = lightSource.range;
        float elapsedTime = 0f;
        float initialIntensity = lightSource.intensity;

        // Увеличиваем диапазон света до максимального значения
        lightSource.range = maxRangeLightSource;
        lightSource.intensity = maxIntensityLightSource;

        // Ждем указанное количество времени
        yield return new WaitForSeconds(flashDuration);

        // Плавно уменьшаем диапазон света до минимального значения
        initialRange = lightSource.range;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            lightSource.intensity = Mathf.Lerp(
                initialIntensity,
                minIntensityLightSource,
                elapsedTime / fadeOutDuration
            );
            lightSource.range = Mathf.Lerp(
                initialRange,
                minRangeLightSource,
                elapsedTime / fadeOutDuration
            );
            yield return null;
        }

        // Устанавливаем диапазон света в минимальное значение на всякий
        lightSource.range = minRangeLightSource;
        lightSource.intensity = minIntensityLightSource;

        canFlash = true;

        if (currentUses == 0 && !isRecharging)
        {
            StartCoroutine(Recharge());
        }
    }

    private IEnumerator Recharge()
    {
        canFlash = false;
        isRecharging = true;
        yield return new WaitForSeconds(rechargeTime);
        currentUses = maxUses;
        isRecharging = false;
        canFlash = true;
    }
}
