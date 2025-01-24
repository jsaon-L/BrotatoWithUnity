# GameplayTag Source Generator

The `GameplayTagSourceGenerator` is a code generator that simplifies using hierarchical tags in your C# project. It automates the creation of static classes for each tag, enabling you to use tags more intuitively while maintaining an organized and easily accessible tag structure.

## How the Source Generator Works

1. **Tag Identification**: The generator looks for `GameplayTag` attributes marked at the assembly level, as shown in the example below:

    ```csharp
    [assembly: GameplayTag("Abilities.Fireball")]
    [assembly: GameplayTag("Abilities.Ice.Spike")]
    ```

   Here, the generator captures these attributes and stores the specified tags.

2. **Hierarchical Organization**: Each tag is split into parts by dot notation (`.`). For example, the tag `Abilities.Fireball` is organized with `Abilities` as a parent node and `Fireball` as its child. This creates a hierarchical structure of tags that the generator uses to build nested classes.

3. **Code Generation**: Based on this hierarchical structure, the generator creates nested classes to represent each level of the tag hierarchy. Each generated class includes a static `Get` method to return the corresponding `GameplayTag` instance. This allows you to access tags directly in your code in a strongly-typed way.

## Example of Generated Code

For the tag examples mentioned, the generated code would look like this:

```csharp
namespace BandoWare.GameplayTags
{
    internal partial class AllGameplayTags
    {
        internal partial class Abilities
        {
            private static readonly GameplayTag s_Tag = GameplayTagManager.RequestTag("Abilities");
            public static GameplayTag Get() { return s_Tag; }

            internal partial class Fireball
            {
                private static readonly GameplayTag s_Tag = GameplayTagManager.RequestTag("Abilities.Fireball");
                public static GameplayTag Get() { return s_Tag; }
            }

            internal partial class Ice
            {
                private static readonly GameplayTag s_Tag = GameplayTagManager.RequestTag("Abilities.Ice");
                public static GameplayTag Get() { return s_Tag; }

                internal partial class Spike
                {
                    private static readonly GameplayTag s_Tag = GameplayTagManager.RequestTag("Abilities.Ice.Spike");
                    public static GameplayTag Get() { return s_Tag; }
                }
            }
        }
    }
}
```

### How to Use the Tags in Code

Once the code is generated, you can directly access the tags:

```csharp
GameplayTag fireballTag = AllGameplayTags.Abilities.Fireball.Get();
GameplayTag iceSpikeTag = AllGameplayTags.Abilities.Ice.Spike.Get();
```