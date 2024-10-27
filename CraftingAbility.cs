namespace Roguelike;

using Godot;
using System.Collections.Generic;

internal class CraftingAbility : Ability
{
    private string _name;
    private Dictionary<ReagentType, int> _reagents;
    private Dictionary<Entity, int> _producedItems;

    public CraftingAbility(string name, Dictionary<ReagentType, int> reagents, Dictionary<Entity, int> producedItems)
    {
        _name = name;
        _reagents = reagents;
        _producedItems = producedItems;
    }
}