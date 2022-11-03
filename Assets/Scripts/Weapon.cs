using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] fireSounds;
    private AudioSource source;
    private Animation anim;
    [SerializeField]
    private AnimationClip slideBack;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();
        anim.wrapMode = WrapMode.Once;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire() {
        source.pitch = Random.Range(0.9f, 1.1f) - (1 - Time.timeScale);
        source.PlayOneShot(fireSounds[Random.Range(0, fireSounds.Length - 1)]);
        anim.clip = slideBack;
        anim.Play();
    }
}
