using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviour
{
    public List<AbilityBase> availableAbilities;
    public AbilityBase currentAbility;
    public Transform abilityHolder;

    private void Start()
    {
        InitializeAbility(abilityHolder, currentAbility);
    }

    void InitializeAbility(Transform holder, AbilityBase ability)
    {
        currentAbility.abilityHolder = holder;
        currentAbility.InitializeAbility(abilityHolder);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int currentAbilityIndex = availableAbilities.IndexOf(currentAbility);
            int previousIndex = currentAbilityIndex - 1 < 0 ? availableAbilities.Count - 1 : currentAbilityIndex - 1;
            currentAbility = availableAbilities[previousIndex];
            Debug.Log(previousIndex);
            InitializeAbility(abilityHolder, currentAbility);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            int currentAbilityIndex = availableAbilities.IndexOf(currentAbility);
            int nextIndex = currentAbilityIndex + 1 > availableAbilities.Count - 1 ? 0 : currentAbilityIndex + 1;
            currentAbility = availableAbilities[nextIndex];
            Debug.Log(nextIndex);
            InitializeAbility(abilityHolder, currentAbility);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            ButtonTriggered();
        }
    }

    void ButtonTriggered()
    {
        currentAbility.TriggerAbility();
    }

}