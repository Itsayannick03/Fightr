Debug.Enabled = true;
Debug.Detailed = false;
Debug.StepThrough = true;

Fighter fighter1 = new Fighter(
    "Alex",
    "Stone",
    115, // Striking
    105, // StrikingDefense
    90,  // Kicking
    95,  // KickingDefense
    80,  // Wrestling
    75,  // Grappling
    120, // Cardio
    110, // Power
    105, // Chin
    65   // Aggression
);

Fighter fighter2 = new Fighter(
    "Marcus",
    "Reed",
    95,  // Striking
    110, // StrikingDefense
    115, // Kicking
    105, // KickingDefense
    85,  // Wrestling
    90,  // Grappling
    110, // Cardio
    100, // Power
    115, // Chin
    50   // Aggression
);


Fight fight = new Fight(3, fighter1, fighter2);

fight.Run();


