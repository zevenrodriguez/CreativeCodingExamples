# Building Interactive Environment

Unity project (Input System + TextMeshPro) with two small interactive scenes: a top-down collection game and a third-person trigger demo.

## Project Structure

```
Assets/
├── CMThirdPerson/
│   ├── CMThirdPerson.unity        # Third-person scene
│   └── script/CMThirdPersonMove.cs
├── CMTop/
│   ├── CMTop.unity                # Top-down scene
│   └── scripts/CMTopMove.cs
├── prefabs/
│   ├── cube.prefab
│   ├── Cylinder.prefab
│   └── Sphere.prefab
├── Scenes/
│   └── SampleScene.unity          # Default sample scene
├── scripts/
│   └── PlaceObjects.cs            # Random object spawner
└── InputSystem_Actions.inputactions
```

## Requirements

- Unity project using the **Input System** package (`UnityEngine.InputSystem`) with an action named `Move` (`Vector2`) in `InputSystem_Actions.inputactions`.
- **TextMeshPro** and **UI** (`UnityEngine.UI`).
- Player objects need a `CharacterController` and a trigger-capable collider setup.

## Scripts

### `PlaceObjects` — [Assets/scripts/PlaceObjects.cs](Assets/scripts/PlaceObjects.cs)

Spawns collectible prefabs at random positions when the scene starts.

| Member | Type | Description |
|---|---|---|
| `prefabs` | `GameObject[]` (serialized) | Prefabs to choose from (defaults to 3 slots: cube, sphere, cylinder). |
| `numberOfObjects` | `int` (public) | How many objects to spawn. Default `5`. |

Behavior: in `Start()`, spawns `numberOfObjects` instances at `x, z` in `[-10, 10]`, `y = 1`, choosing a random prefab each time.

### `CMTopMove` — [Assets/CMTop/scripts/CMTopMove.cs](Assets/CMTop/scripts/CMTopMove.cs)

Top-down player controller and collection logic.

| Member | Type | Description |
|---|---|---|
| `speed` | `float` | Movement speed. Default `5`. |
| `canvas` | `Canvas` | Message canvas, hidden at start. |
| `outputText` | `TMP_Text` | Text shown when the goal is reached. |
| `amount` | `PlaceObjects` | Reference used to size the found-items list. |

Behavior:
- **Movement:** `Move` action maps X to world X and Y to world Z; applied with `CharacterController.Move`.
- **Collecting:** on `OnTriggerEnter`, objects tagged `cube`, `sphere`, or `cylinder` are recorded, destroyed, and counted.
- **Win state:** when the count reaches `amount.numberOfObjects`, the canvas is enabled and shows "All objects found!".

Setup: tag the three prefabs `cube`, `sphere`, `cylinder` (see [Creating Tags](#creating-tags)) and enable **Is Trigger** on each prefab's collider.

### `CMThirdPersonMove` — [Assets/CMThirdPerson/script/CMThirdPersonMove.cs](Assets/CMThirdPerson/script/CMThirdPersonMove.cs)

Third-person, tank-style player controller with a proximity message.

| Member | Type | Description |
|---|---|---|
| `speed` | `float` | Movement and rotation speed. Default `5`. |
| `canvas` | `Canvas` | Message canvas, hidden at start. |
| `outputText` | `TMP_Text` | Text shown near the interactable. |

Behavior:
- **Movement:** `Move.y` moves along `transform.forward`; `Move.x` rotates around the Y axis.
- **Interaction:** entering the trigger of an object named `Interactable0` shows the canvas with "Collided with Interactable0"; leaving hides it and clears the text.

## Prefabs

[Assets/prefabs](Assets/prefabs) contains three 3D object prefabs, used as the collectibles spawned by `PlaceObjects` and collected by `CMTopMove`:

| Prefab | Shape | Expected tag |
|---|---|---|
| `cube.prefab` | Cube | `cube` |
| `Cylinder.prefab` | Cylinder | `cylinder` |
| `Sphere.prefab` | Sphere | `sphere` |

Each prefab has **Is Trigger** enabled on its collider and is tagged with the tag shown above, so the player passes through it and `OnTriggerEnter` fires.

### Creating Tags

Tags must exist before they can be assigned. Tag names are case-sensitive and must match the strings in `CMTopMove` exactly.

1. Open the prefab (double-click it in `Assets/prefabs`) or select the object in the Hierarchy.
2. In the Inspector, open the **Tag** dropdown at the top and choose **Add Tag...** (this opens **Project Settings > Tags and Layers**).
3. Under **Tags**, click **+**, enter the name (`cube`, `sphere` or `cylinder`) and click **Save**.
4. Select the prefab again, open the **Tag** dropdown and pick the new tag.
5. Repeat for each prefab.

Also confirm that the prefab's collider has **Is Trigger** checked.
## Scenes

| Scene | Purpose |
|---|---|
| `Assets/CMTop/CMTop.unity` | Top-down collection game (`CMTopMove` + `PlaceObjects`). |
| `Assets/CMThirdPerson/CMThirdPerson.unity` | Third-person movement and trigger demo (`CMThirdPersonMove`). |
| `Assets/Scenes/SampleScene.unity` | Default Unity sample scene. |

## Known Limitations

- `CMTopMove` counts every collected object as one item, so it depends on `PlaceObjects.numberOfObjects` matching the number of objects actually in the scene.
- `CMTopMove.Start()` and `CMThirdPersonMove.Start()` throw if `canvas` is unassigned; `CMTopMove` also requires `amount`.
- `CMThirdPersonMove` logs `rotateYAxis` every frame in `Update()`.
- `Move` is looked up by name via `InputSystem.actions.FindAction("Move")`; renaming the action breaks both controllers.
