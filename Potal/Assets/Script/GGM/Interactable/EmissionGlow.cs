using UnityEngine;

public class EmissionGlow : MonoBehaviour
{
    [SerializeField] private Material targetMaterial;
    [SerializeField] private Color glowColor = new Color(0f, 1f, 1f); // 시안
    [SerializeField] private float glowSpeed = 2f;
    [SerializeField] private float minIntensity = 0.2f;
    [SerializeField] private float maxIntensity = 2f;

    private void Update()
    {
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, (Mathf.Sin(Time.time * glowSpeed) + 1f) / 2f);
        Color finalColor = glowColor * intensity;

        targetMaterial.SetColor("_EmissionColor", finalColor);
    }
}