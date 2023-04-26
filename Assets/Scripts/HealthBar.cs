using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Player player;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI healthText;
    public Image background;
    public Color invincibleColor; 


    // Start is called before the first frame update
    void Start()
    {
        
        SetMaxHealth(player.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
      
         SetHealth(player.health);
        
        
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
        UpdateHealthText(health);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        
        if (!player.isInvincible)
        {
            // Evaluate the gradient based on the normalized health value
            float normalizedHealth = (float)health / 100f; 
            
            // Calculate the normalized health value between 0 and 1
            fill.color = gradient.Evaluate(normalizedHealth);
        }

        UpdateHealthText(health);

        // Set the background transparency
        Color backgroundColor = background.color;
        backgroundColor.a = 0.2f;
        background.color = backgroundColor;
    }
 

    private void UpdateHealthText(int health)
    {
        healthText.text = $"{Mathf.RoundToInt(slider.normalizedValue * 100)}";
    }

    public void SetInvincibleColor()
    {
        fill.color = invincibleColor;
    }

    public void ResetColor()
    {
        // Calculate the normalized health value between 0 and 1
        float normalizedHealth = (float)slider.value / 100f;

        // Evaluate the gradient based on the normalized health value
        fill.color = gradient.Evaluate(normalizedHealth);
    }
}