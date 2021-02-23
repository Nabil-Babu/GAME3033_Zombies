using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Weapons;

public class WeaponInfoUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CurrentAmmoText;
    [SerializeField] private TextMeshProUGUI TotalAmmoText;
    [SerializeField] private TextMeshProUGUI WeaponNameText;

    private WeaponComponent EquippedWeapon; 

    void OnEnable()
    {
        PlayerEvents.OnWeaponEquipped += OnWeaponsEquipped;
    }
    
    void OnDisable()
    {
        PlayerEvents.OnWeaponEquipped -= OnWeaponsEquipped;
    }
    private void OnWeaponsEquipped(WeaponComponent weapon)
    {
        EquippedWeapon = weapon;
        WeaponNameText.text = weapon.WeaponInformation.WeaponName;
    }

    void Update()
    {
        CurrentAmmoText.text = EquippedWeapon.WeaponInformation.BulletsInClip.ToString();
        TotalAmmoText.text = EquippedWeapon.WeaponInformation.BulletsAvailable.ToString();
    }
}
