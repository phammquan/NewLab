using UnityEngine;

[CreateAssetMenu(fileName = "NewMetalMeshPair", menuName = "Custom/Metal Mesh Pair")]
public class MetalMeshPair : ScriptableObject
{
    public Metal metal;               // Loại kim loại
    public int meshRendereIndexs; // Renderer tương ứng trong scene
}