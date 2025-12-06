using Godot;
using System.Collections.Generic;
using System.Linq;

namespace DREditor.Characters
{

    [GlobalClass]
    public partial class CharacterDatabase : Resource
    {
        [Export]
        public Godot.Collections.Array<Character> Characters { get; set; } = new();

        public List<string> GetNames()
        {
            var names = new List<string>();

            foreach (var cha in Characters)
            {
                var name = $"{cha.LastName} {cha.FirstName}";
                switch (cha)
                {
                    case Protagonist _:
                        names.Add(name + " (Protagonist)");
                        break;
                    case Headmaster _:
                        names.Add(name + " (Headmaster)");
                        break;
                    default:
                        names.Add(name);
                        break;
                }
            }
            return names;
        }

        public Character GetCharacter(string firstName)
        {
            return Characters.FirstOrDefault(c => c.FirstName == firstName, null);
        }
        public Character GetCharacterByContaining(string name)
        {
            return Characters.FirstOrDefault(c => name.Contains(c.FirstName), null);
        }
        public int GetActorPrefabCount()
        {
            return Characters.Count(c => c.ActorPrefab != null);
        }
    }
}