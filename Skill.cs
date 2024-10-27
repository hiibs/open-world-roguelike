namespace Roguelike;

using Godot;
using System.Collections.Generic;

internal abstract class Skill
{
    protected List<Ability> _unlockedAbilities;
    protected List<Ability> _lockedAbilities;

    public int CurrentXP { get; private set; }
    public int CurrentLevel { get; private set; }

    private int GetXPRequirement(int level)
    {
        return (Mathf.RoundToInt(Mathf.Pow((float)level, 1.414f) * 100));
    }
}