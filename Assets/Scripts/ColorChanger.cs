using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public void ChangeColor(MeshRenderer meshRenderer) =>
        meshRenderer.material.color = Random.ColorHSV();
}