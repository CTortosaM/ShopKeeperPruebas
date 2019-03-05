using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> weapons;
    [SerializeField] private float weaponSwitchDelay = .5f;
    private int index = 0;
    private bool isSwitching = false;

    public float WeaponSwitchDelay { get => weaponSwitchDelay; set => weaponSwitchDelay = value; }

    // Start is called before the first frame update
    private void Start()
    {
        InitializeWeapons();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0f && !isSwitching)
        {
            isSwitching = true;

            index++;

            if (index > weapons.Count - 1) index = 0;

            SwitchWeapons(index);
            
           
        }
        else if (Input.GetAxis("Mouse ScrollWheel")<0f && !isSwitching)
        {
            isSwitching = true;

            index--;
            if (index < 0) index = weapons.Count - 1;

            SwitchWeapons(index);
        }
    }

    private void InitializeWeapons()
    {
        for(int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }

        weapons[0].SetActive(true);

    }

    private IEnumerator SwitchCooldown()
    {

        yield return new WaitForSeconds(weaponSwitchDelay);
        isSwitching = false;

    }

    private void SwitchWeapons(int newIndex)
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(false);
        }

        weapons[newIndex].SetActive(true);
        StartCoroutine(SwitchCooldown());
    }
}
