//DRPlaylist script by SeleniumSoul for DREditor
//Godot version by CSharpKun
using Godot;
using System.Linq;

namespace gDREditor.Audio
{
    [GlobalClass]
    public partial class Playlist : Resource, IPlaylist
    {
        [Export] public Music[] Musics { get; set; }

        public string[] GetAudioTitles()
        {            
            return [..Musics.Select(m => m.Title)];
        }

        public AudioStream GetAudioStream(string title)
        {
            var stream = Musics.Where(m => m.Title == title).FirstOrDefault(null as Music);
            if (stream == null) { GD.PushWarning("Couldn't find Music: " + title); return null; }
            return stream.BGM;
        }
        public string GetTitleFromClip(AudioStream stream)
        {
            var title = Musics.Where(m => m.BGM == stream).FirstOrDefault(null as Music);
            if (title == null) { GD.PushWarning("Couldn't find Title: " + stream); return null; }
            return title.Title;
        }
    }

    [GlobalClass]
    public partial class Music : Resource
    {
        [Export] public string Title { get; set; }
        [Export] public AudioStream BGM { get; set; }
    }

    #region Interfaces
    public interface IPlaylist
    {
        string[] GetAudioTitles();
        AudioStream GetAudioStream(string title);
        string GetTitleFromClip(AudioStream stream);
    }
    #endregion
}