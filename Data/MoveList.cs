public static class MoveList
{
    public static Move jab = new Move(
        name: "jab",
        damage: 2,
        staminaCost: 1,
        type: MoveType.Punch,
        targetBodyparts: new List<BodypartType> 
        {
            BodypartType.Head,
            BodypartType.Torso
        },
        accuracyModifier: 4,
        significant: false
    );

    public static Move cross = new Move(
        name: "cross",
        damage: 4,
        staminaCost: 2,
        type: MoveType.Punch,
        targetBodyparts: new List<BodypartType> 
        {
            BodypartType.Head,
            BodypartType.Torso
        },
        accuracyModifier: 1
    );

    public static Move hook = new Move(
        name: "hook",
        damage: 5,
        staminaCost: 3,
        type: MoveType.Punch,
        targetBodyparts: new List<BodypartType> 
        {
            BodypartType.Head,
            BodypartType.Torso
        },
        accuracyModifier: -1
    );

    public static Move haymaker = new Move(
        name: "haymaker",
        damage: 8,
        staminaCost: 5,
        type: MoveType.Punch,
        targetBodyparts: new List<BodypartType> 
        {
            BodypartType.Head
        },
        accuracyModifier: -5
    );

    public static Move lowKick = new Move(
        name: "low kick",
        damage: 5,
        staminaCost: 3,
        type: MoveType.Kick,
        targetBodyparts: new List<BodypartType> 
        {
            BodypartType.LeftLeg,
            BodypartType.RightLeg
        },
        accuracyModifier: 2
    );

    public static Move bodyKick = new Move(
        name: "body kick",
        damage: 7,
        staminaCost: 4,
        type: MoveType.Kick,
        targetBodyparts: new List<BodypartType> 
        {
            BodypartType.Torso
        },
        accuracyModifier: 0
    );

    public static Move headKick = new Move(
        name: "head kick",
        damage: 10,
        staminaCost: 6,
        type: MoveType.Kick,
        targetBodyparts: new List<BodypartType> 
        {
            BodypartType.Head
        },
        accuracyModifier: -4
    );

    public static Move frontKick = new Move(
        name: "front kick",
        damage: 5,
        staminaCost: 3,
        type: MoveType.Kick,
        targetBodyparts: new List<BodypartType> 
        {
            BodypartType.Torso,
            BodypartType.Head
        },
        accuracyModifier: 1
    );
}