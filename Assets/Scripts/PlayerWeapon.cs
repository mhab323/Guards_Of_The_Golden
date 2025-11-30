using System.Collections;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [Header("References")]
    public Transform handBone;
    public AudioSource swingAudioSource;   // <--- add this

    [Header("Offsets in Hand")]
    public Vector3 localPositionOffset;
    public Vector3 localRotationOffset;

    [Header("Audio")]
    public AudioClip swingClip;           // <--- optional, if you want a specific clip

    [Header("Runtime")]
    public GameObject currentWeapon;

    bool isSwinging = false;
    Quaternion defaultWeaponRotation;

    void Update()
    {
        if (currentWeapon != null && Input.GetMouseButtonDown(0) && !isSwinging)
        {
            StartCoroutine(SwingWeapon());
        }
    }

    public void Equip(GameObject weapon)
    {
        currentWeapon = weapon;

        weapon.transform.SetParent(handBone);
        weapon.transform.localPosition = localPositionOffset;
        weapon.transform.localRotation = Quaternion.Euler(localRotationOffset);

        defaultWeaponRotation = weapon.transform.localRotation;

        Rigidbody rb = weapon.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        Collider col = weapon.GetComponent<Collider>();
        if (col) col.enabled = false;
    }

    IEnumerator SwingWeapon()
    {
        isSwinging = true;

        // PLAY SOUND HERE
        if (swingAudioSource != null)
        {
            if (swingClip != null)
                swingAudioSource.PlayOneShot(swingClip);
            else
                swingAudioSource.Play(); // uses clip on the AudioSource
        }

        float duration = 0.2f;
        float timer = 0f;

        Quaternion startRot = defaultWeaponRotation;
        Quaternion swingRot = defaultWeaponRotation * Quaternion.Euler(-70f, 0f, 0f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float swingAmount = Mathf.Sin(t * Mathf.PI); // 0 → 1 → 0

            currentWeapon.transform.localRotation = Quaternion.Slerp(startRot, swingRot, swingAmount);

            yield return null;
        }

        currentWeapon.transform.localRotation = defaultWeaponRotation;
        isSwinging = false;
    }
}
