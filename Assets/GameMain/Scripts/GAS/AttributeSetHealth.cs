using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeSetHealth : AttributeSet
{
  
    public GASAttribute HealthAttribute;
    public GASAttribute MaxHealthAttribute;


    public AttributeSetHealth(float health)
    {
        HealthAttribute = new GASAttribute(health);
        MaxHealthAttribute = new GASAttribute(HealthAttribute.GetValue());
    }

}
