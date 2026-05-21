// PickupSystem — currently a placeholder.
//
// The active pickup flow is handled by:
//   PickupView        (Presentation/Pickups)   — detects player overlap via trigger
//   PlayerPickupConsumer (Presentation/Pickups) — receives the pickup and routes it
//   PickupContext     (Application/Pickups)    — binds the effect to the player
//
// If you want to centralise pickup logic (pooling, global pickup events, scoring),
// implement it here and wire it into the chain above.
public class PickupSystem
{
    // NOTE: PickupContext is defined in Application/Pickups/PickupContext.cs.
    // Do NOT re-declare it here — that causes a CS0101 "duplicate type" compile error.
}
