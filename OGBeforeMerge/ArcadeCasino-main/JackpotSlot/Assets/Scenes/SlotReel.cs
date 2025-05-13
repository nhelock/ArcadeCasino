using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotReel : MonoBehaviour
{
    public Sprite[] symbols; 
    public Sprite jackpotSymbol;
    public Sprite[] rareSymbols;

    [HideInInspector] public List<Sprite> weightedSymbols = new List<Sprite>();

    private SpriteRenderer spriteRenderer;
    public float spinSpeed = 0.05f;

    private bool spinning = false;
    private Coroutine spinRoutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        foreach (Sprite s in symbols)
        {
            if (s == jackpotSymbol)
            {
                weightedSymbols.Add(s);
            }
            else if (System.Array.Exists(rareSymbols, r => r == s))
            {
                weightedSymbols.Add(s); 
            }
            else
            {
                for (int i = 0; i < 5; i++) 
                {
                    weightedSymbols.Add(s);
                }
            }
        }
    }

    public void StartSpinning()
    {
        spinning = true;
        spinRoutine = StartCoroutine(Spin());
    }

    public void StopSpinning(float delay)
    {
        StartCoroutine(StopWithDelay(delay));
    }

    IEnumerator Spin()
    {
        while (spinning)
        {
            int index = Random.Range(0, weightedSymbols.Count);
            spriteRenderer.sprite = weightedSymbols[index];
            yield return new WaitForSeconds(spinSpeed);
        }
    }

    IEnumerator StopWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        spinning = false;
    }

    public Sprite GetCurrentSymbol()
    {
        return spriteRenderer.sprite;
    }
}
