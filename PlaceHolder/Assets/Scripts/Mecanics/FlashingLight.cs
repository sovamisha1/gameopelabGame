using System.Collections;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    public GameObject lightObject;
    private SphereCollider lightCollider;

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
        lightCollider = lightObject.GetComponent<SphereCollider>();
        if (lightCollider == null)
        {
            Debug.LogError("No SphereCollider attached to the lightObject!");
        }
    }

    void Start()
    {
        currentUses = maxUses;
    }

    void Update()
    {
        if (lightSource.range > minRangeLightSource)
        {
            lightSource.range -= 4 * Time.deltaTime;
        }

        if (
            Input.GetKeyDown(InputManager.instance.GetKeyForAction("Flash"))
            && canFlash
            && currentUses > 0
        )
        {
            StartCoroutine(FlashLight());
        }

        if (Input.GetKeyDown(InputManager.instance.GetKeyForAction("Recharge")) && !isRecharging)
        {
            StartCoroutine(Recharge());
        }
    }

    private IEnumerator FlashLight()
    {
        canFlash = false;
        currentUses--;

        float initialRange = lightSource.range;
        float initialIntensity = lightSource.intensity;
        float elapsedTime = 0f;

        lightSource.range = maxRangeLightSource;
        lightSource.intensity = maxIntensityLightSource;
        lightCollider.radius = maxRangeLightSource / 2;

        yield return new WaitForSeconds(flashDuration);

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            lightCollider.radius = Mathf.Lerp(
                maxRangeLightSource / 2,
                0.1f,
                elapsedTime / fadeOutDuration
            );
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
