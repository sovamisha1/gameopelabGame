using System.Collections;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    [Header("Objects")]
    public GameObject lightObject;
    public GameObject candle;
    public Camera mainCamera;
    private SphereCollider lightCollider;
    public UIController uiController;

    [Header("LigthSettings")]
    public Light lightSource;
    public float maxRangeLightSource = 30.0f;
    public float minRangeLightSource = 5.0f;
    public float maxIntensityLightSource = 10.0f;
    public float minIntensityLightSource = 1.0f;
    public float flashDuration = 0.3f;
    public float fadeOutDuration = 0.2f;

    [Header("ChardgesSettings")]
    public int maxUses = 3;
    public float rechargeTime = 10.0f;

    private int currentUses;
    private bool canFlash = true;
    private bool isRecharging = false;

    void Awake()
    {
        if (lightCollider == null)
            lightCollider = lightObject.GetComponent<SphereCollider>();
        if (uiController == null)
            uiController = GameObject.FindWithTag("Canvas").GetComponent<UIController>();
        if (candle == null)
            candle = this.gameObject;
        if (mainCamera == null)
            mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Start()
    {
        candle.transform.SetParent(mainCamera.transform, false);
        currentUses = maxUses;
        uiController.SetCharges(currentUses);
    }

    void Update()
    {
        Positionpotion();

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
        uiController.SetCharges(currentUses);

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
                0.01f,
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
        uiController.SetCharges(currentUses);
        isRecharging = false;
        canFlash = true;
    }

    void Positionpotion()
    {
        Vector3 localPosition = new Vector3(0.95f, -0.6f, 1.4f);
        candle.transform.localPosition = localPosition;
        candle.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
