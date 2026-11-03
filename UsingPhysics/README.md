# Using Physics

A Unity example project for **CIM203 – Intro to Creative Coding**. It introduces Unity's physics engine through two small interactive scenes.

- **Bouncing Balls:** press a key to drop balls that bounce, using a Physics Material.
- **Cannonball:** aim and fire a cannon, watch the ball arc under gravity, and see it splash into water.

Topics covered: Rigidbodies, colliders, Physics Materials, `Instantiate` / `Destroy`, prefabs, the Input System (Input Actions), raycasts, a custom water shader, and VFX Graph.

---

## Requirements

| | Version |
|---|---|
| Unity | **6000.6.0f1** (Unity 6.6) |
| Render pipeline | Universal Render Pipeline (URP) 17.6.0 |
| Input | Input System 1.20.0. **Active Input Handling** is set to *Input System Package (New)*. |
| VFX | Visual Effect Graph 17.6.0 |

Open the project folder in Unity Hub with the version above. Then open a scene from `Assets/Scenes`.

---

## Scenes

### 1. BouncingBalls (`Assets/Scenes/BouncingBalls.unity`)

Press **Space** to drop a ball from 10 units up, at a random spot over the ground. Each ball has a bouncy Physics Material, so it bounces when it hits the ground.

| Object | Purpose |
|---|---|
| **Spawner** | Holds `BallSpawner.cs`, which spawns the `ball` prefab |
| **Ground** | Plane the balls land on |

**Controls:** `Space` drops a ball.

### 2. Cannonball (`Assets/Scenes/cannonball.unity`)

Aim a cannon, change the power and fire. Cannonballs fly in an arc because of gravity. When one reaches the water it plays a splash effect and is destroyed.

| Object | Purpose |
|---|---|
| **cannon** | Cylinder used as the barrel. `LaunchBall` rotates it. |
| **script** | Holds `LaunchBall.cs` |
| **Ground** | A flat cube using the **Water** material. It's tagged `Floor`, so cannonballs can detect it. |

**Controls:** these use the standard **Player** actions in `InputSystem_Actions`.

| Action | Keyboard / Mouse | Gamepad | Does |
|---|---|---|---|
| Move | WASD / Arrow keys | Left stick | Turn (left/right) and tilt (up/down) the cannon |
| Attack | Left mouse / Enter | West button (X) | Fire |
| Next | `2` | D-pad right | More power |
| Previous | `1` | D-pad left | Less power |

Each shot logs its angle and speed to the Console. Try finding the angle that goes farthest.

---

## Project structure

```
Assets/
├── InputSystem_Actions.inputactions   Project-wide input actions (Player + UI maps)
├── Materials/
│   ├── Bouncy.physicMaterial          Physics Material (bounciness) for the balls
│   └── Water.mat                      Material using the SimpleWater shader
├── prefab/
│   ├── ball.prefab                    Bouncing ball (Rigidbody + Bouncy + DestroyBelowGround)
│   ├── cannonball.prefab              Cannonball (Rigidbody + Bouncy + CannonBallDestroy)
│   └── splash.prefab                  Visual Effect + SplashDestroy
├── Scenes/
│   ├── BouncingBalls.unity
│   └── cannonball.unity
├── Scripts/
│   ├── BallSpawner.cs
│   ├── DestroyBelowGround.cs
│   └── Cannonball/
│       ├── LaunchBall.cs
│       ├── CannonBallDestroy.cs
│       └── SplashDestroy.cs
├── Shaders/
│   └── SimpleWater.shader             URP see-through water with world-space ripples
└── vfx/
    └── splash.vfx                     VFX Graph splash burst
Documentation/
└── README.md                          Step-by-step guide to building the splash VFX
```

---

## Scripts

| Script | Goes on | What it does |
|---|---|---|
| `BallSpawner.cs` | Spawner | When `Space` is pressed, instantiates `ballPrefab` at a random X/Z within `spawnRange`, at `spawnHeight`. |
| `DestroyBelowGround.cs` | ball prefab | Destroys the ball when its Y position drops below `destroyHeight`, for example after it rolls off the edge. |
| `LaunchBall.cs` | script object | Reads the Move / Attack / Next / Previous actions to aim the barrel, change `launchSpeed` and fire. Sets the cannonball's `linearVelocity` to point along the barrel. |
| `CannonBallDestroy.cs` | cannonball prefab | Raycasts downward each physics step. When it hits an object tagged `Floor`, it spawns the splash and destroys the cannonball. It also cleans up below Y = −10. |
| `SplashDestroy.cs` | splash prefab | Sends the `OnPlay` event to the Visual Effect, then destroys the splash after 2 seconds. |

---

## Key concepts

### Physics Materials
A Physics Material on a collider controls **friction** and **bounciness** (0 = no bounce, 1 = perfect bounce). When two colliders touch, Unity combines their values using the **Combine** mode. The priority order is Maximum > Multiply > Minimum > Average.

> If the ground has no material, its bounciness is 0. With the default *Average* combine, a ball with 0.8 bounciness only bounces at 0.4. Set **Bounce Combine** to **Maximum** on the ball's material so it uses its own bounciness.

Also note:

- **Bounce Threshold** (*Project Settings → Physics*, default 2) stops slow, tiny bounces. Lower it to keep small bounces going.
- Mass doesn't affect how high something bounces. **Linear Damping** (air resistance) does.

### Colliders
Use simple shapes (Box, Sphere, Capsule) wherever possible. They're faster and more reliable than Mesh Colliders. A Mesh Collider on Unity's built-in Plane shows a warning about *pre-baked triangle collision*. Replacing it with a thin Box Collider removes the warning.

### Input System
The project uses **project-wide actions** (`InputSystem_Actions`). Scripts look actions up by name:

```csharp
InputAction moveAction = InputSystem.actions.FindAction("Move");
Vector2 move = moveAction.ReadValue<Vector2>();
```

You can change the controls by editing the asset. No code changes are needed.

### Water shader (`SimpleWater.shader`)
A transparent URP shader whose ripples use **world position** instead of UVs. The ripples line up across every face of a cube and stay the same size when the object is scaled. Adjustable on the material: Shallow/Deep Color, Opacity, Ripple Size/Speed/Strength, Shininess and Edge Glow.

A cube only has 4 vertices per face, so the shader doesn't move vertices to make waves. If you want real waves, use a subdivided plane.

### Splash VFX
Built with VFX Graph: a **Single Burst** of particles with random upward velocity (**Set Velocity**, Random: *Per Component*), gravity, and color and size fading over life. See **[Documentation/README.md](Documentation/README.md)** for the full step-by-step build.

---

## Experiments for students

1. Change the **Bounciness** of `Bouncy` while the game is running. Compare 0, 0.5 and 1.
2. Change **Bounce Combine** between Average and Maximum. Why does the bounce change?
3. Fire the cannon at 15°, 30°, 45°, 60° and 75° with the same power. Which goes farthest?
4. Change the gravity in *Project Settings → Physics* to simulate the Moon (−1.62).
5. Change the splash's burst **Count** and **velocity** to make a bigger or smaller splash.
6. Change the water colors and ripple settings to make a swamp, a pool or lava.
