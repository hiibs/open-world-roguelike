namespace Roguelike;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class Character : Entity
{
    private List<Skill> _skills = new();

    internal void AddSkill(Skill skill) => _skills.Add(skill);
}
