//All Right Receved © TPN GAMES.


using System.Collections;
using UnityEngine;

public class TextureScrollAndFlash : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // Speed of scrolling
    public float flashDuration = 5.0f; // Duration of flashing
    public float scrollDuration = 3.0f; // Duration of each scroll phase
    private string nameMy = "TPNGAMES";
    private Renderer objectRenderer;
    private bool isFlashing = false;
    private Color originalColor;
    private Color flashColor = Color.red; // Color to flash

    void Start()
    {
        if (nameMy == "TPNGAMES")
        {
        
            objectRenderer = GetComponent<Renderer>();
            originalColor = objectRenderer.material.color;
            StartCoroutine(ScrollAndFlashCoroutine());
        }
    }

    private IEnumerator ScrollAndFlashCoroutine()
    {
        while (true)
        {
            // Scroll from right to left
            yield return StartCoroutine(ScrollTexture(Vector2.left));
            // Scroll from left to right
            yield return StartCoroutine(ScrollTexture(Vector2.right));
            // Flash the texture
            yield return StartCoroutine(FlashTexture());
        }
    }

    private IEnumerator ScrollTexture(Vector2 direction)
    {
        float elapsedTime = 0f;
        Vector2 originalOffset = objectRenderer.material.mainTextureOffset;

        while (elapsedTime < scrollDuration)
        {
            Vector2 offset = originalOffset + direction * (elapsedTime / scrollDuration) * scrollSpeed;
            objectRenderer.material.mainTextureOffset = offset;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Reset to original position after scrolling
        objectRenderer.material.mainTextureOffset = originalOffset;
    }

    private IEnumerator FlashTexture()
    {
        float elapsedTime = 0f;

        while (elapsedTime < flashDuration)
        {
            isFlashing = !isFlashing;
            objectRenderer.material.color = isFlashing ? flashColor : originalColor;
            elapsedTime += 0.1f; // Flash every 0.1 seconds
            yield return new WaitForSeconds(0.1f);
        }

        // Reset color after flashing
        objectRenderer.material.color = originalColor;
    }
}
