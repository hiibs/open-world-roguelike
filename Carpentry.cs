namespace Roguelike;

using System.Collections.Generic;

internal class Carpentry : Skill
{
    public Carpentry()
    {
        _unlockedAbilities = new()
        {
            CreateBowRecipe()
        };

    }

    private Ability CreateBowRecipe()
    {
        var bowReagents = new Dictionary<ReagentType, int>();
        bowReagents.Add(ReagentType.Sapling, 1);
        bowReagents.Add(ReagentType.String, 1);

        var bowResult = new Dictionary<Entity, int>();
        bowResult.Add(new Bow(), 1);

        return new CraftingAbility("Craft a bow", bowReagents, bowResult);
    }
}