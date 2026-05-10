using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WoolDropping : MonoBehaviour
{
    public Transform WoolAsset;

    private void OnEnable()
    {
        bool randomBool = UnityEngine.Random.value > 0.5f;
        
        transform.localScale = new Vector3((randomBool ?- 1:1)*transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoToGoat()
    {
        GetComponent<Animator>().enabled = false;
        WoolAsset.DOMove(Goat.Instance.transform.position,0.3f).SetEase(Ease.InSine).OnComplete(() => {
            Goat.Instance.AddWool();
            Destroy(gameObject);
        });
    }
}
