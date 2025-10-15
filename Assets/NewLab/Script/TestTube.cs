using UnityEngine;
using System.Collections.Generic;
using System.Collections;       

public class TestTube : MonoBehaviour, Interactable, TagTutorial
{
    [SerializeField] private string tagTutorial;
    
    [Header("Visuals")]
    [Tooltip("Renderer của phần chất lỏng trong ống nghiệm.")]
    public Renderer liquidRenderer;
    [Tooltip("Hiệu ứng sủi bọt khi phản ứng.")]
    public ParticleSystem bubblingEffect;
    
    public bool isLiquid = false;
    public bool isItem = false;
    [SerializeField] bool canInteractMySelf;
    private float delayEffect = 0;

    public List<Transform> posMove;
    
    [Header("Metal-Mesh-SO")]
    public List<MetalMeshPair> metalPairs; 
    
    public Dictionary<Metal, int> metalDict;
    
    [Header("Mesh")]
    public MeshRenderer[] MetalRenderers;

    private List<string> contents = new List<string>();

    void Start()
    {
        ConvertListToDictionary();
        if (bubblingEffect != null)
        {
            bubblingEffect.Stop();
            foreach (var meshRen in MetalRenderers)
            {
                meshRen.enabled = false;
            }
        }
        UpdateVisuals();
    }

    public void AddLiquid(string liquidName, Color liquidColor)
    {
        if (!contents.Contains(liquidName))
        {
            contents.Add(liquidName);
            isLiquid = true;
            Debug.Log(gameObject.name + " giờ chứa: " + liquidName);

            UpdateVisuals();
            CheckForReaction(delayEffect);
        }
    }

    private void ConvertListToDictionary()
    {
        metalDict = new Dictionary<Metal, int>();
        foreach (var pair in metalPairs)
        {
            if (pair != null && !metalDict.ContainsKey(pair.metal))
            {
                metalDict.Add(pair.metal, pair.meshRendereIndexs);
            }
        }
    }
    public void AddItem(Metal metal, float delay)
    {
        delayEffect = delay;
        int metalMeshIndex;
        if (metalDict.TryGetValue(metal, out metalMeshIndex) && !contents.Contains(metal.ToString()))
        {
            contents.Add(metal.ToString());
            MetalRenderers[metalMeshIndex].enabled = true;
            isItem = true;
            Debug.Log(gameObject.name + " giờ chứa: " + metal.ToString());
            CheckForReaction(delay);
        }
    }

    private void CheckForReaction(float delay)
    {
        if (isItem && isLiquid)
        {
            if (contents.Contains("Cu"))
            {
                return;
            }
            StartCoroutine(DelayRunReactionEffect(delay));
        }
    }

    IEnumerator DelayRunReactionEffect(float delay)
    {
        yield return new WaitForSeconds(delay);
        RunReactionEffect();

    }

    private void RunReactionEffect()
    {
        if (bubblingEffect != null)
        {
            bubblingEffect.Play();
        }

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (liquidRenderer != null)
        {
            liquidRenderer.enabled = isLiquid;
        }
    }

    #region Interactable Interface
    public void Interact(Interactor interactor) { }
    public bool canInteract()
    {
       return canInteractMySelf;
    }

    public void Use(RaycastHit hitInfo) { }
    public void OnPickup() { }
    public void OnDrop() { }
    public bool IsValidPlacement(RaycastHit hitInfo)
    {
        throw new System.NotImplementedException();
    }

    #endregion

    public string GetTextTutorial()
    {
      return tagTutorial;
    }
}

public enum Metal : byte
{
    Fe = 0,
    Mg = 1,
    Al = 2,
    Cu = 3,
}