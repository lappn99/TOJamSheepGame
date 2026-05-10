using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Goat : MonoBehaviour
{
    public static Goat Instance;
    public static UnityAction FirstInfected, CoatReady;
    public int WoolCount, WoolMax, WoolCountFirstInfected;
    public Renderer Coat1, Coat2;
    private void Awake()
    {
        if(Instance == null)
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void AddWool(int add = 1)
    {
        WoolCount+= add;
        Coat1.material.DOKill(true);
        Coat1.material.DOFloat(1, "_Blink", 0.15f).SetLoops(2,LoopType.Yoyo);
        Coat1.material.SetFloat("_Dithering", (float)WoolCount / WoolMax);

        Coat2.material.DOKill(true);
        Coat2.material.DOFloat(1, "_Blink", 0.15f).SetLoops(2, LoopType.Yoyo);
        Coat2.material.SetFloat("_Dithering", (float)WoolCount / WoolMax);

        transform.DOKill(true);
        transform.DOScale(1.5f, 0.15f).SetLoops(2,LoopType.Yoyo);

        if (WoolCount == WoolCountFirstInfected)
        {
            FirstInfected.Invoke();
        }

        if (WoolCount == WoolMax)
        {
            CoatReady.Invoke();
        }
    }
}
