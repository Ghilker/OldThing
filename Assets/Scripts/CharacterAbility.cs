using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviour
{
    public List<AbilityBase> availableAbilities;
    public AbilityBase currentAbility;
    public Transform abilityHolder;

    bool onCooldown = false;
    float currentCooldownTime;

    private void Start()
    {
        InitializeAbility(abilityHolder, currentAbility);
    }

    void InitializeAbility(Transform holder, AbilityBase ability)
    {
        currentAbility.abilityHolder = holder;
        currentAbility.player = gameObject;
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
        if (Input.GetButton("Fire1") && !onCooldown)
        {
            ButtonTriggered();
        }
    }

    void ButtonTriggered()
    {
        onCooldown = true;
        currentAbility.TriggerAbility();
        StartCoroutine(AbilityCooldown());
    }

    IEnumerator AbilityCooldown()
    {
        yield return new WaitForSeconds(currentAbility.abilityCooldown);
        onCooldown = false;
    }

}