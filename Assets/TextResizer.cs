using UnityEngine;
using TMPro;

[ExecuteAlways] // Makes this run in both Play and Edit Mode
public class TextScaler : MonoBehaviour
{
    public TMP_FontAsset customFont; // Drag the font into the editor here

    void Update()
    {
        UpdateTextScale();
    }

    private void UpdateTextScale()
    {
        // Step 1: Get parent scales
        Transform parent1 = transform.parent;
        Transform parent2 = parent1 != null ? parent1.parent : null;
        Transform parent3 = parent2 != null ? parent2.parent : null;
        Transform parent4 = parent3 != null ? parent3.parent : null;

        if (parent1 == null || parent2 == null || parent3 == null || parent4 == null)
        {
            Debug.LogWarning("TextScaler: Not enough parents in hierarchy!");
            return;
        }

        // Step 2: Calculate total parent scale (inverse of the combined scale of all parents)
        Vector3 totalParentScale = Vector3.one;

        totalParentScale = Vector3.Scale(totalParentScale, parent1.localScale);
        totalParentScale = Vector3.Scale(totalParentScale, parent2.localScale);
        totalParentScale = Vector3.Scale(totalParentScale, parent3.localScale);
        totalParentScale = Vector3.Scale(totalParentScale, parent4.localScale);

        // Step 3: Set inverse scale to the TextMeshPro
        Vector3 inverseScale = new Vector3(
            1f / totalParentScale.x,
            1f / totalParentScale.y,
            1f / totalParentScale.z
        );

        transform.localScale = inverseScale;

        // Step 4: Set font size, specific font, and text alignment
        TMP_Text textComponent = GetComponent<TMP_Text>();
        if (textComponent != null)
        {
            textComponent.fontSize = 16; // Set font size to 16
            textComponent.font = customFont; // Set the specific font (drag in the editor)
            textComponent.alignment = TextAlignmentOptions.Center; // Ensure the text is centered
        }

        // Step 5: Set the position to (0, 0) in the RectTransform
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero; // Set position to (0, 0)
        }
    }
}
