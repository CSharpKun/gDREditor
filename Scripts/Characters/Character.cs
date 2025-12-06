// Completed edited version by CSharpKun
using Godot;
using System.Linq;
namespace DREditor.Characters;

public partial class Alias : Resource
{
    public string Name { get; set; }
    public Texture2D Nameplate { get; set; }
    public Texture2D TrialNameplate { get; set; }
    public Texture2D TrialPortrait { get; set; }
}

// Any properties marked //* were edits made by Benjamin "Sweden" Jillson : Sweden#6386
// Again, all properties were remade to Godot standarts, but saved og structure.
[GlobalClass]
public partial class Character : Resource
{
    public string TranslationKey { get; set; }
    public string LastName { get; set; } = "";
    public string FirstName { get; set; } = "";
    public Texture2D DefaultSprite { get; set; }
    public Texture2D TrialPortrait { get; set; }
    public Texture2D NSDPortrait { get; set; } //*
    public PackedScene ActorPrefab { get; set; } //*
    #if TOOLS
    public bool showSprites { get; set; } = false; // For editor window's use
    #endif
    public Material BlackExpression { get; set; } //*
    public Material TrialMaterial { get; set; }
    public Material MissingMat { get; set; } //*
    public Texture2D MissingTex { get; set; } //*
    public Godot.Collections.Array<Expression> Expressions { get; set; } = new();
    public Godot.Collections.Array<Unlit> Sprites { get; set; } = new(); // for using sprite renderers
    public Godot.Collections.Array<Alias> Aliases { get; set; } = new();
    public Texture2D Nameplate { get; set; }
    public Texture2D Headshot { get; set; }
    public Texture2D TrialNameplate { get; set; }
    public float TrialHeight { get; set; } = 7.88f;
    public int TrialPosition { get; set; } = 0;
    public bool IsDead = false;
    public FTEData FriendshipData { get; set; } = new() { FriendshipLvl = 1 };

    // (message to Benjamin "Sweden" Jillson) I still can't understand why would you need massive of indexes... 

    public Texture GetSpriteByName(string name)
    {
        var sprites = Sprites.Where(u => u.Name == name).FirstOrDefault(null as Unlit);
        if (sprites == null)
        {
            GD.PushWarning("COULDN'T GET SPRITE BY NAME: " + name);
            return null;
        }
        return sprites.Sprite.texture;
    }
    public string GetSpriteLabelByTexName(string name)
    {
        var sprites = Sprites.Where(s => s.Sprite.texture.Name == name).FirstOrDefault(null as Unlit);
        if (sprites == null)
        {
            GD.PushWarning("COULDN'T GET SPRITE LABEL BY TEX NAME: " + name);
            return null;
        }
        return sprites.Name;
    }
}

public struct FTEData
{
    public int FriendshipLvl;
}
