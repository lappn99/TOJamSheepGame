using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Unity.Cinemachine;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [FormerlySerializedAs("firstInfectedChance")] [SerializeField] private float latentInfectionChance = 0.05f;
    [FormerlySerializedAs("firstInfectedTryRate")] [SerializeField] private float latentInfectionTryRate = 1.0f;
    [SerializeField] private UnityAction<Sheep> firstSheepInfected;
    [SerializeField] private Volume PPVolume;
    [SerializeField] private Image noiseVignette;
    [SerializeField] private Animator BackgroundAnimator;

    public CinemachineCamera cmCamera;
    private CinemachineBasicMultiChannelPerlin noiseCamera;

    private List<Sheep> _sheep = new List<Sheep>();

    private bool _firstSheepInfected;

    private static GameManager _instance;

    public int infectedCount;

    public static GameManager Instance => _instance;

    private void Awake()
    {
        PPVolume.weight = 0f;
        noiseVignette.enabled = false;
        noiseCamera = cmCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void OnEnable()
    {
        Goat.FirstInfected += StartLatentInfection;
        firstSheepInfected += StartFirstInfection;
        Goat.CoatReady += CoatReady;
    }

    private void OnDisable()
    {
        Goat.FirstInfected -= StartLatentInfection;
        firstSheepInfected -= StartFirstInfection;
        Goat.CoatReady -= CoatReady;
    }


    void StartLatentInfection()
    {
        Sheep first = _sheep[Random.Range(0,_sheep.Count)];
        first.Infect();
        firstSheepInfected.Invoke(first);
        _firstSheepInfected = true;
        StartCoroutine(LatentInfection());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSheep(Sheep sheep)
    {
        _sheep.Add(sheep);
        sheep.onDestroyed.AddListener((() =>
        {
            _sheep.Remove(sheep);
        }));
    }

    public void UpdateInfected(int add)
    {
        infectedCount += add;
        if(infectedCount <= 0)
        {
            PPVolume.weight = 0f;
            noiseVignette.enabled = false;
        }
        else
        {
            PPVolume.weight = 1f;
            StartCoroutine(ShakeCamera(5, 0.5f));
            noiseVignette.enabled = true;
        }
    }

    private IEnumerator LatentInfection()
    {
        while (true)
        {
            
            foreach (var sheep in _sheep)
            {
                if (Random.value <= latentInfectionChance)
                {
                    if (sheep.Infect())
                    {
                        PPVolume.weight = 1.0f;

                        if (!_firstSheepInfected)
                        {
                            _firstSheepInfected = true;
                            firstSheepInfected.Invoke(sheep);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(latentInfectionTryRate);
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator ShakeCamera(float intensity, float duration)
    {
        noiseCamera.AmplitudeGain = intensity; // On active le tremblement
        yield return new WaitForSeconds(duration);
        noiseCamera.AmplitudeGain = 0f; // On l'arrête
    }


    void StartFirstInfection(Sheep infected)
    {
        StartCoroutine(FirsInfectedEffect(infected.transform));
    }

    private IEnumerator FirsInfectedEffect(Transform focusTransform)
    {
        yield return new WaitForSeconds(5f);

        float orthoSave = cmCamera.Lens.OrthographicSize;
        Vector3 cmPosSave = cmCamera.transform.position;
        float elapsed = 0;
        float duration = 0.3f;

        cmCamera.Follow = focusTransform;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            float smoothPercent = Mathf.SmoothStep(0, 1, percent);

            cmCamera.Lens.OrthographicSize = Mathf.Lerp(orthoSave, 1.5f, smoothPercent);

            yield return null; 
        }
            yield return new WaitForSeconds(3f);

        cmCamera.Follow = null;
        cmCamera.transform.DOMove(cmPosSave, duration);

        while (elapsed > 0)
        {

            elapsed -= Time.deltaTime;
            float percent = elapsed / duration;

            float smoothPercent = Mathf.SmoothStep(0, 1, percent);

            cmCamera.Lens.OrthographicSize = Mathf.Lerp(orthoSave, 1.5f, smoothPercent);

            yield return null; 
        }
        BackgroundAnimator.SetTrigger("Infected");

        Time.timeScale = 1;

    }

    private void CoatReady()
    {
        StartCoroutine(GoatFinalAnimation());
    }

    private IEnumerator GoatFinalAnimation()
    {
        float orthoSave = cmCamera.Lens.OrthographicSize;
        float elapsed = 0;
        float duration = 0.3f;

        cmCamera.Follow = Goat.Instance.transform;
        cmCamera.GetComponent<CinemachineFollow>().FollowOffset += new Vector3(0, 1.9f, -10f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percent = elapsed / duration;

            float smoothPercent = Mathf.SmoothStep(0, 1, percent);

            cmCamera.Lens.OrthographicSize = Mathf.Lerp(orthoSave, 3.5f, smoothPercent);

            yield return null;
        }
    }
}
