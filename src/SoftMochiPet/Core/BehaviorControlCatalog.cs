namespace SoftMochiPet.Core;

public enum BehaviorControlAction
{
    Stop,
    Stand,
    Walk,
    Run,
    Curious,
    Explore,
    Patrol,
    Hop,
    Climb,
    SlideDown,
    JumpDown,
    Roll,
    Sleep,
    Wake,
    FreeFall,
    Toss,
    Hungry,
    LickIcon,
    EatIcon,
    Satisfied,
    Kick,
    Punch,
    Charge,
    HatStore,
    Shatter,
    Tear,
    Tease,
    FillMischief,
    ReturnWindows,
    ThrowAndRecoverHat,
}

[Flags]
public enum BehaviorControlCharacters
{
    None = 0,
    Nuonuo = 1,
    FeibiJiubi = 2,
    Both = Nuonuo | FeibiJiubi,
}

[Flags]
public enum BehaviorControlRequirements
{
    None = 0,
    SessionAvailable = 1 << 0,
    NotDragging = 1 << 1,
    Awake = 1 << 2,
    GroundSupport = 1 << 3,
    OutsideEnclosure = 1 << 4,
    ReachableClimbWindow = 1 << 5,
    WindowPlatform = 1 << 6,
    FreeAirSpace = 1 << 7,
    Sleeping = 1 << 8,
    FoodEnabled = 1 << 9,
    DesktopIcon = 1 << 10,
    EligibleWindow = 1 << 11,
    EmptyHat = 1 << 12,
    HeldWindow = 1 << 13,
    MischiefReady = 1 << 14,
}

public enum BehaviorControlKind
{
    Behavior,
    StateChange,
    AnimationPreview,
}

public sealed record BehaviorControlItem(
    BehaviorControlAction Action,
    string Group,
    string Title,
    BehaviorControlCharacters Characters,
    BehaviorControlRequirements Requirements,
    BehaviorControlKind Kind,
    string Description)
{
    public bool Supports(PetCharacterProfile character)
    {
        ArgumentNullException.ThrowIfNull(character);
        var flag = character.SupportsFood ? BehaviorControlCharacters.Nuonuo
            : character.UsesMischief ? BehaviorControlCharacters.FeibiJiubi : BehaviorControlCharacters.None;
        return (Characters & flag) != 0;
    }
}

/// <summary>
/// Menu metadata for the behavior-control build. Conditions describe the real
/// action route; the dispatcher must recheck live targets before execution.
/// </summary>
public static class BehaviorControlCatalog
{
    private const BehaviorControlCharacters Both = BehaviorControlCharacters.Both;
    private const BehaviorControlCharacters Nuonuo = BehaviorControlCharacters.Nuonuo;
    private const BehaviorControlCharacters Feibi = BehaviorControlCharacters.FeibiJiubi;
    private const BehaviorControlKind Behavior = BehaviorControlKind.Behavior;
    private const BehaviorControlKind StateChange = BehaviorControlKind.StateChange;
    private const BehaviorControlRequirements Ready = BehaviorControlRequirements.SessionAvailable |
        BehaviorControlRequirements.NotDragging;
    private const BehaviorControlRequirements Awake = Ready | BehaviorControlRequirements.Awake;
    private const BehaviorControlRequirements Ground = Awake | BehaviorControlRequirements.GroundSupport;
    private const BehaviorControlRequirements Travel = Ground | BehaviorControlRequirements.OutsideEnclosure;
    private const BehaviorControlRequirements WindowAction = Awake | BehaviorControlRequirements.OutsideEnclosure |
        BehaviorControlRequirements.EligibleWindow;
    private const BehaviorControlRequirements IconAction = Awake | BehaviorControlRequirements.OutsideEnclosure |
        BehaviorControlRequirements.FoodEnabled | BehaviorControlRequirements.DesktopIcon;

    public static IReadOnlyList<BehaviorControlItem> All { get; } = Array.AsReadOnly(new[]
    {
        new BehaviorControlItem(BehaviorControlAction.Stop, "行为控制", "Stop current action", Both,
            BehaviorControlRequirements.None, StateChange, "Stop current action并安全收场；已收进帽子的窗口仍可主动归还。"),
        new BehaviorControlItem(BehaviorControlAction.Stand, "Posture and mood", "Stand", Both,
            Ground, Behavior, "回到有支撑的位置，保持Stand待机。"),
        new BehaviorControlItem(BehaviorControlAction.Curious, "Posture and mood", "Look around curiously", Both,
            Ground, Behavior, "在原地好奇地张望。"),
        new BehaviorControlItem(BehaviorControlAction.Walk, "Walking and exploration", "Walk", Both,
            Travel, Behavior, "沿当前支撑面走到一个附近位置。"),
        new BehaviorControlItem(BehaviorControlAction.Run, "Walking and exploration", "Run", Both,
            Travel, Behavior, "沿支撑面真实跑动，不只是原地播放跑步帧。"),
        new BehaviorControlItem(BehaviorControlAction.Explore, "Walking and exploration", "Explore", Both,
            Travel, Behavior, "走到附近探索，抵达后Look around curiously。"),
        new BehaviorControlItem(BehaviorControlAction.Patrol, "Walking and exploration", "Patrol", Both,
            Travel, Behavior, "持续沿支撑面巡游，到边缘时按地形决定后续动作。"),
        new BehaviorControlItem(BehaviorControlAction.Hop, "Jumps and terrain", "Hop", Both,
            Travel | BehaviorControlRequirements.FreeAirSpace, Behavior, "从支撑面蹦起，再按重力落下。"),
        new BehaviorControlItem(BehaviorControlAction.Climb, "Jumps and terrain", "Climb window", Both,
            Travel | BehaviorControlRequirements.ReachableClimbWindow, Behavior, "走向可攀爬的真实窗口边缘，再尝试爬上去；有可能失手。"),
        new BehaviorControlItem(BehaviorControlAction.SlideDown, "Jumps and terrain", "Slide down window", Both,
            Travel | BehaviorControlRequirements.WindowPlatform, Behavior, "在真实窗口顶边走到边缘，沿侧面滑下后落地。"),
        new BehaviorControlItem(BehaviorControlAction.JumpDown, "Jumps and terrain", "Jump down from window", Both,
            Travel | BehaviorControlRequirements.WindowPlatform | BehaviorControlRequirements.FreeAirSpace,
            Behavior, "从当前窗口顶边跳下，按真实落地规则结束。"),
        new BehaviorControlItem(BehaviorControlAction.Roll, "Jumps and terrain", "Roll", Both,
            Ground, Behavior, "在支撑面上翻滚；菲比也可手动体验，不附加Hunger状态。"),
        new BehaviorControlItem(BehaviorControlAction.FreeFall, "Jumps and terrain", "Free fall", Both,
            Awake | BehaviorControlRequirements.FreeAirSpace, Behavior, "从有下落空间的位置松开角色，按重力落地，不操作系统鼠标。"),
        new BehaviorControlItem(BehaviorControlAction.Toss, "Jumps and terrain", "Toss", Both,
            Awake | BehaviorControlRequirements.FreeAirSpace, Behavior, "给角色一个向上的初速度，体验被抛起与落地的物理效果。"),
        new BehaviorControlItem(BehaviorControlAction.Sleep, "Sleep and wake", "Go to sleep", Both,
            Ready | BehaviorControlRequirements.GroundSupport, Behavior, "在支撑面上入睡，播放入睡与呼吸循环。"),
        new BehaviorControlItem(BehaviorControlAction.Wake, "Sleep and wake", "Wake up", Both,
            Ready | BehaviorControlRequirements.Sleeping | BehaviorControlRequirements.GroundSupport,
            Behavior, "从睡眠状态醒来，再回到日常站姿。"),
        new BehaviorControlItem(BehaviorControlAction.Hungry, "Nuonuo互动", "Hungry", Nuonuo,
            Ground | BehaviorControlRequirements.FoodEnabled, StateChange, "进入Hunger状态并表达Hungry；辟谷模式下不可用。"),
        new BehaviorControlItem(BehaviorControlAction.LickIcon, "Nuonuo互动", "Lick an icon", Nuonuo,
            IconAction, Behavior, "跑到可见桌面图标旁舔一舔，不修改图标或文件。"),
        new BehaviorControlItem(BehaviorControlAction.EatIcon, "Nuonuo互动", "Inhale icon copy", Nuonuo,
            IconAction, Behavior, "使用真实图标外观的副本，跑过去吸入并满足收场；不删除真实文件。"),
        new BehaviorControlItem(BehaviorControlAction.Satisfied, "Animation preview", "满足（Animation preview）", Nuonuo,
            Ground, BehaviorControlKind.AnimationPreview, "单独欣赏吃饱后的满足动作，不假装已经吃掉文件，也不增加进食记录。"),
        new BehaviorControlItem(BehaviorControlAction.Kick, "菲比Mischief", "Kick window", Feibi,
            WindowAction, Behavior, "踢动符合安全条件的真实窗口；帽中已有存货时仍可执行。"),
        new BehaviorControlItem(BehaviorControlAction.Punch, "菲比Mischief", "Punch window", Feibi,
            WindowAction, Behavior, "打动符合安全条件的真实窗口；帽中已有存货时仍可执行。"),
        new BehaviorControlItem(BehaviorControlAction.Charge, "菲比Mischief", "Charge into window", Feibi,
            WindowAction, Behavior, "冲向符合安全条件的真实窗口，命中后让窗口滑动减速。"),
        new BehaviorControlItem(BehaviorControlAction.HatStore, "菲比Mischief", "Store window in hat", Feibi,
            WindowAction | BehaviorControlRequirements.EmptyHat, Behavior, "把窗口快照收进帽子，真实窗口最小化，等待主动归还。"),
        new BehaviorControlItem(BehaviorControlAction.Shatter, "菲比Mischief", "Shatter window", Feibi,
            WindowAction | BehaviorControlRequirements.EmptyHat, Behavior, "Shatter window快照，再把碎片收进帽子；不破坏应用或文件。"),
        new BehaviorControlItem(BehaviorControlAction.Tear, "菲比Mischief", "Tear window vertically", Feibi,
            WindowAction | BehaviorControlRequirements.EmptyHat, Behavior, "变大后上下扯开窗口快照，再收进帽子；原窗口可完整归还。"),
        new BehaviorControlItem(BehaviorControlAction.Tease, "Feibi interactions", "Tease her", Feibi,
            Awake | BehaviorControlRequirements.MischiefReady, StateChange, "按现有逗弄规则增加Mischief值；连续逗弄和安全冷却仍有间隔。"),
        new BehaviorControlItem(BehaviorControlAction.FillMischief, "Feibi interactions", "Mischief槽加满", Feibi,
            Awake, StateChange, "把Mischief值设为满槽；不绕过真实窗口安全条件，也不自动归还已有存货。"),
        new BehaviorControlItem(BehaviorControlAction.ReturnWindows, "Hat interactions", "Return window", Feibi,
            Ready | BehaviorControlRequirements.HeldWindow, Behavior, "播放取回与拼合过程，再确认原窗口实际恢复。"),
        new BehaviorControlItem(BehaviorControlAction.ThrowAndRecoverHat, "Hat interactions", "Throw and recover hat", Feibi,
            Travel | BehaviorControlRequirements.EmptyHat | BehaviorControlRequirements.FreeAirSpace,
            Behavior, "完整抛帽、跑过去捡回并重新戴好，不孤立播放无帽中间帧。"),
    });

    private static readonly IReadOnlyList<BehaviorControlItem> NuonuoItems =
        Array.AsReadOnly(All.Where(item => item.Supports(PetCharacterProfile.Nuonuo)).ToArray());
    private static readonly IReadOnlyList<BehaviorControlItem> FeibiItems =
        Array.AsReadOnly(All.Where(item => item.Supports(PetCharacterProfile.FeibiJiubi)).ToArray());

    public static IReadOnlyList<BehaviorControlItem> ForCharacter(PetCharacterProfile character)
    {
        ArgumentNullException.ThrowIfNull(character);
        return character.UsesMischief ? FeibiItems : NuonuoItems;
    }

    public static BehaviorControlItem Get(BehaviorControlAction action) =>
        All.FirstOrDefault(item => item.Action == action) ?? throw new ArgumentOutOfRangeException(nameof(action), action, null);
}
