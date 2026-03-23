using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SkillCheck : MonoBehaviour
{
    public static SkillCheck Instance { get; set; }

    private WeaponBase currentWeapon;
    public AudioClip[] ReloadSounds;
    public TextMeshProUGUI Sequence;
    private List<KeyCode> keySequence = new List<KeyCode>();
    public int currentKey = 0;
    public bool isActive;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    public void StartQTE(WeaponBase weapon)
    {
        currentWeapon = weapon;
        FillKeyCodes();
        DisplaySequence();

        currentKey = 0;
        isActive = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Sequence != null && isActive)
        {
            if (Input.anyKeyDown)
            {
                if (currentKey < keySequence.Count && Input.GetKeyDown(keySequence[currentKey]))
                {
                    // Implement for reload sound per keycode

                    //for (int soundIndex = currentKey; soundIndex < keySequence.Count;  soundIndex++)
                    //{
                    //    currentWeapon.ReloadAudio.PlayOneShot(ReloadSounds[soundIndex]);
                    //}
                    currentKey++;
                    if (currentKey >= keySequence.Count)
                    {
                        CompletedQTE();
                    }
                }
                else
                {
                    FailedQTE();
                    Debug.Log("QTE Failed");
                }
            }
        }
    }

    void FillKeyCodes()
    {
        keySequence.Clear();

        KeyCode[] possibleKeys = 
        {
            KeyCode.L, KeyCode.K, KeyCode.J, KeyCode.I
        };

        for(int i = 0; i < possibleKeys.Length; i++)
        {
            keySequence.Add(possibleKeys[Random.Range(0, possibleKeys.Length)]);
        }
    }

    void DisplaySequence()
    {
        Sequence.gameObject.SetActive(true);
        Sequence.text = string.Join(" ", keySequence);
    }

    void CompletedQTE()
    {
        isActive = false;
        Sequence.gameObject.SetActive(false);

        Debug.Log("QTE Success");
        currentWeapon.Reload();
    }

    void FailedQTE()
    {
        isActive = false;
        Sequence.gameObject.SetActive(false);

        Debug.Log("QTE Failed");
    }
}

