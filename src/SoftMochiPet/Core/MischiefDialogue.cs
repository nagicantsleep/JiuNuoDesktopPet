namespace SoftMochiPet.Core;

public enum MischiefCue
{
    Teased,
    Sneak,
    Kick,
    Punch,
    Charge,
    Recoil,
    HideWindow,
    HoldingWindow,
    ReturnWindow,
    Burst,
    Smash,
    Tear,
    Finished,
    Interrupted,
    NoTarget,
}

public sealed class MischiefDialogueSelector
{
    private static readonly IReadOnlyDictionary<MischiefCue, IReadOnlyList<string>> Lines =
        new Dictionary<MischiefCue, IReadOnlyList<string>>
        {
            [MischiefCue.Teased] = ["Hehe, again!", "That tickles!", "Teasing me again!", "Got you!"],
            [MischiefCue.Sneak] = ["Shh—", "You didn't see me.", "Sneaking in.", "Hehe, wait for it!"],
            [MischiefCue.Kick] = ["Watch my feet!", "Take that!", "Off you go!", "Just a little kick!"],
            [MischiefCue.Punch] = ["Bonk!", "Tiny punch!", "Hey!", "Catch this!"],
            [MischiefCue.Charge] = ["Make room!", "Here I go—!", "Coming through!", "Hold onto the hat!"],
            [MischiefCue.Recoil] = ["Ouchie.", "Bottom-first landing!", "Pretend you saw nothing.", "I'm steady now!"],
            [MischiefCue.HideWindow] = ["Let me hide this!", "The hat can hold it!", "Hehe, it's mine now.", "No peeking!"],
            [MischiefCue.HoldingWindow] = ["I'm not hiding anything.", "Behave, hat!", "Huh? What window?", "Hehe…"],
            [MischiefCue.ReturnWindow] = ["Here you go!", "Ta-da—!", "It's right here!", "All done hiding."],
            [MischiefCue.Burst] = ["I can't resist!", "Hat, take a break!", "Now I'm serious!", "Hehe, watch this!"],
            [MischiefCue.Smash] = ["Fly away—!", "Big kick!", "Wow, it broke apart!", "I'm amazing!"],
            [MischiefCue.Tear] = ["Heave—ho!", "Why is it so tough?", "Just a little farther!", "Oops, it's sticky!"],
            [MischiefCue.Finished] = ["That was fun!", "The hat is back!", "Hehe, no problem.", "I was good!"],
            [MischiefCue.Interrupted] = ["Caught me!", "Okay, okay.", "I'm letting go.", "Next time!"],
            [MischiefCue.NoTarget] = ["Where did the toy go?", "Maybe later!", "I'll bounce around first.", "Then I'll rest."],
        };

    private readonly Random _random;
    private readonly Dictionary<MischiefCue, int> _previous = [];

    public MischiefDialogueSelector(Random? random = null) => _random = random ?? Random.Shared;

    public static IReadOnlyList<string> GetLines(MischiefCue cue) => Lines[cue];

    public string Choose(MischiefCue cue)
    {
        var lines = GetLines(cue);
        var index = _random.Next(lines.Count);
        if (_previous.TryGetValue(cue, out var previous) && index == previous)
        {
            index = (index + 1) % lines.Count;
        }
        _previous[cue] = index;
        return lines[index];
    }
}
