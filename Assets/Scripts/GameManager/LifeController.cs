using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LifeController : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int currentHp;
    [SerializeField] private bool fullHpOnAwake = true;

    [Header("Shield Settings")]
    [SerializeField] private int maxShield = 0;
    [SerializeField] private int currentShield;

    [Header("Death Settings")]
    [SerializeField] private DeathAction death = DeathAction.Destroy;

    public UnityEvent OnDeath;

    public enum DeathAction { None, Destroy, Disable, Die, SceneReload, Animation }

    private AnimationState currentState = AnimationState.RUN;

    public AnimationState GetState() => currentState;
    public int GetMaxHp() => maxHp;
    public int GetHp() => currentHp;
    public int GetShield() => currentShield;
    public int GetMaxShield() => maxShield;

    private void Awake()
    {
        if (fullHpOnAwake)
        {
            currentHp = maxHp;
            currentShield = maxShield;
        }
    }


    public void TakeDamage(int amount)
    {
        if (amount <= 0) return;

        if (currentShield > 0)
        {
            int shieldDamage = Mathf.Min(currentShield, amount);
            currentShield -= shieldDamage;
            amount -= shieldDamage;
        }

    
        if (amount > 0)
        {
            SetHp(currentHp - amount);
        }
    }

  
    public void AddHp(int amount)
    {
        if (amount <= 0) return;
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHp);
    }

    public void AddShield(int amount)
    {
        if (amount <= 0) return;
        currentShield += amount;
        Debug.Log("Scudi attivi:" + currentShield);
    }

 
    public void SetHp(int hp)
    {
        int oldHp = currentHp;
        currentHp = Mathf.Clamp(hp, 0, maxHp);

        if (oldHp > 0 && currentHp == 0)
        {
            OnDeath?.Invoke();
            HandleDeath();
        }
    }

    public void HandleDeath()
    {
        switch (death)
        {
            case DeathAction.None:
                break;
            case DeathAction.Animation:
                currentState = AnimationState.DEATH; 
                break; 
            case DeathAction.Destroy:
                Destroy(gameObject);
                break;
            case DeathAction.Die:
                RespawnManager.Instance.PlayerDied();
                break;
            case DeathAction.Disable:
                gameObject.SetActive(false);
                break;
            case DeathAction.SceneReload:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }
}

