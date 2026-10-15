# Building Interactive Environments

A small Unity project with two playable scenes that show the building blocks of an interactive 3D world:

- **CMTop**: a top-down collection game. Objects spawn at random spots and you walk into them to collect them. When you've found them all, a message appears.
- **CMThirdPerson**: a third-person movement demo. Walk up to an object and a message appears; walk away and it disappears.

Read this alongside the scripts. Each section explains *what* the code does and *why* it's written that way.

## What You'll Learn

- Reading keyboard/gamepad input with the **Input System**
- Moving a player with a **CharacterController**
- Making movement frame-rate independent with **`Time.deltaTime`**
- Detecting when objects touch using **triggers** (`OnTriggerEnter` / `OnTriggerExit`)
- Telling objects apart with **tags** and **names**
- Spawning objects at runtime with **prefabs** and **`Instantiate`**
- Showing on-screen text with a **Canvas** and **TextMeshPro**
- Connecting scripts and objects in the Inspector with **`[SerializeField]`**

---

## Project Structure

```
Assets/
├── CMTop/
│   ├── CMTop.unity                  # Top-down collection scene
│   └── scripts/CMTopMove.cs         # Player movement + collecting
├── CMThirdPerson/
│   ├── CMThirdPerson.unity          # Third-person trigger scene
│   └── script/CMThirdPersonMove.cs  # Player movement + proximity message
├── scripts/
│   └── PlaceObjects.cs              # Spawns random collectibles
├── prefabs/
│   ├── cube.prefab                  # Collectibles used by PlaceObjects
│   ├── Cylinder.prefab
│   └── Sphere.prefab
├── Scenes/SampleScene.unity         # Unity's default sample scene
├── InputSystem_Actions.inputactions # Input mappings (contains "Move")
├── Settings/                        # URP render settings (don't need to touch)
├── TextMesh Pro/                    # TMP fonts and shaders (don't need to touch)
└── TutorialInfo/                    # Unity template readme (can ignore)
```

### What's in each scene

| GameObject | CMTop | CMThirdPerson | Purpose |
|---|:-:|:-:|---|
| **Character** | ✓ | ✓ | The player. Has a `CharacterController` and the movement script. |
| **Floor** | ✓ | ✓ | Ground to walk on. |
| **Canvas → Panel → output** | ✓ | ✓ | UI message box. `output` is the TextMeshPro text. |
| **script** | ✓ | | Empty object that holds `PlaceObjects`. |
| **Interactable0** | | ✓ | The object that triggers the message. |
| **Main Camera**, **Directional Light**, **Global Volume**, **EventSystem** | ✓ | ✓ | Standard scene setup. |

---

## Key Concepts

### `Start()` and `Update()`

Every script here inherits from `MonoBehaviour`, which lets Unity call special methods for you:

- **`Start()`** runs **once**, just before the first frame. Use it for setup: finding components, hiding UI, creating arrays.
- **`Update()`** runs **every frame** (often 60+ times per second). Use it for things that change continuously, like movement.

### `[SerializeField]` vs `public`

```csharp
[SerializeField] float speed = 5.0f;   // private, but editable in the Inspector
public int numberOfObjects = 5;        // editable in the Inspector AND by other scripts
```

Both make a field appear in the **Inspector** so you can change values or drag in references without editing code. Use `[SerializeField]` by default. Use `public` only when another script needs to read the value. `CMTopMove` reads `PlaceObjects.numberOfObjects`, so that one is `public`.

> The number in the code (`= 5.0f`) is only the *default*. Once the script is on an object, **the Inspector value wins**. If changing the code doesn't seem to do anything, check the Inspector.

### The Input System

```csharp
moveAction = InputSystem.actions.FindAction("Move");      // in Start()
Vector2 moveValue = moveAction.ReadValue<Vector2>();       // in Update()
```

Instead of checking specific keys, we ask for the **`Move` action**, defined in `InputSystem_Actions.inputactions`. It gives back a `Vector2`:

- `moveValue.x` is left/right: −1 (A) to +1 (D)
- `moveValue.y` is down/up: −1 (S) to +1 (W)

Double-click `InputSystem_Actions.inputactions` to see which keys and sticks are bound to `Move`. Because the action is looked up **by name**, renaming it breaks both movement scripts.

### CharacterController and `Time.deltaTime`

```csharp
ourCharacterController.Move(movement * speed * Time.deltaTime);
```

- A **`CharacterController`** is a component for moving a player that bumps into walls instead of passing through them, without full physics like gravity or bouncing.
- **`Time.deltaTime`** is the number of seconds since the last frame. Multiplying by it turns "units per frame" into "units per **second**", so the player moves at the same speed on a fast or slow computer.

### Triggers

A **trigger** is a collider with **Is Trigger** checked. Objects pass *through* it, and Unity calls these methods on scripts attached to the objects involved:

```csharp
void OnTriggerEnter(Collider col)  // the moment something enters
void OnTriggerExit(Collider col)   // the moment it leaves
```

`col` is the *other* object's collider, so `col.gameObject` is the thing you touched.

For triggers to fire:
1. At least one of the two objects has a collider with **Is Trigger** checked.
2. One of them has a **Rigidbody** or a **CharacterController**. Our player has a CharacterController, so that's covered.

### Tags and names

Scripts need to know *what* they touched:

```csharp
if (col.gameObject.tag == "cube")              // CMTopMove: check by tag
if (col.gameObject.name == "Interactable0")    // CMThirdPersonMove: check by name
```

- **Names** are unique-ish labels for one object. Fine for a single special object.
- **Tags** are categories shared by many objects. Better for "any collectible".

Both are **case-sensitive**: `"Cube"` is not `"cube"`.

### Prefabs and `Instantiate`

A **prefab** is a saved, reusable GameObject (with its components and settings) stored in the Project. `Instantiate(prefab, position, rotation)` makes a new copy of it in the scene while the game is running.

---

## Script Walkthroughs

### `PlaceObjects`: [Assets/scripts/PlaceObjects.cs](Assets/scripts/PlaceObjects.cs)

Spawns random collectibles when the scene starts. In **CMTop** it's attached to the `script` GameObject.

```csharp
[SerializeField] GameObject[] prefabs = new GameObject[3];  // drag cube/sphere/cylinder in here
public int numberOfObjects = 5;

void Start()
{
    for (int i = 0; i < numberOfObjects; i++)
    {
        Vector3 position = new Vector3(Random.Range(-10f, 10f), 1.0f, Random.Range(-10f, 10f));
        int randomIndex = Random.Range(0, prefabs.Length);
        Instantiate(prefabs[randomIndex], position, Quaternion.identity);
    }
}
```

Step by step:
1. Loop `numberOfObjects` times.
2. Pick a random position: X and Z between −10 and 10, Y fixed at 1 (just above the floor).
3. Pick a random prefab from the array. With ints, `Random.Range(0, 3)` returns 0, 1 or 2 (the max is **excluded**).
4. `Instantiate` creates it. `Quaternion.identity` means "no rotation".

| Field | Type | Description |
|---|---|---|
| `prefabs` | `GameObject[]` | Prefabs to choose from. Every slot must be filled. |
| `numberOfObjects` | `int` | How many to spawn. Default `5`. |

### `CMTopMove`: [Assets/CMTop/scripts/CMTopMove.cs](Assets/CMTop/scripts/CMTopMove.cs)

Top-down movement plus the collecting game. Attached to **Character** in CMTop.

**Setup (`Start`)**

```csharp
moveAction = InputSystem.actions.FindAction("Move");
canvas.enabled = false;                                   // hide the message at first
ourCharacterController = GetComponent<CharacterController>();
ObjectsFound = new string[amount.numberOfObjects];        // one slot per object to find
```

`GetComponent<CharacterController>()` finds the CharacterController on the **same** GameObject as this script.

**Movement (`Update`)**

```csharp
Vector3 movement = new Vector3(moveValue.x, 0, moveValue.y);
ourCharacterController.Move(movement * speed * Time.deltaTime);
```

The input is 2D but the world is 3D. Input **X** maps to world **X** (left/right) and input **Y** maps to world **Z** (forward/back). Y stays 0 so you don't fly.

**Collecting (`OnTriggerEnter`)**

```csharp
if (col.gameObject.tag == "cube" || col.gameObject.tag == "sphere" || col.gameObject.tag == "cylinder")
{
    AddItem(col.gameObject.tag);          // remember what we found
    Destroy(col.gameObject);              // remove it from the scene
    bool allFound = CheckIfAllObjectsFound();
    if (allFound == true)
    {
        canvas.enabled = true;
        outputText.text = "All objects found!";
    }
}
```

- `||` means "or". Any of the three tags counts.
- `AddItem` stores the tag in the `ObjectsFound` array at position `counter`.
- `CheckIfAllObjectsFound` adds 1 to `counter`, then returns `true` once `counter` reaches the array's length (= `numberOfObjects`).

| Field | Type | Description |
|---|---|---|
| `speed` | `float` | Movement speed in units/second. Default `5`. |
| `canvas` | `Canvas` | The message canvas. Hidden at start. |
| `outputText` | `TMP_Text` | The `output` text object. |
| `amount` | `PlaceObjects` | Drag the `script` object here so we know how many objects exist. |

### `CMThirdPersonMove`: [Assets/CMThirdPerson/script/CMThirdPersonMove.cs](Assets/CMThirdPerson/script/CMThirdPersonMove.cs)

"Tank-style" controls: W/S moves in the direction you're facing and A/D turns. Attached to **Character** in CMThirdPerson.

**Movement (`Update`)**

```csharp
float moveDistance = moveValue.y * Time.deltaTime * speed;
float rotateYAxis  = moveValue.x * Time.deltaTime * speed;
transform.Rotate(0f, rotateYAxis, 0f);                         // turn around the vertical axis
ourCharacterController.Move(transform.forward * moveDistance); // move where we're facing
```

- `transform.Rotate(0, angle, 0)` spins the player around the **Y (up)** axis by `angle` **degrees**.
- `transform.forward` is the direction the player's front is pointing, so "forward" changes as you turn.
- The same `speed` is used for walking (units/sec) and turning (degrees/sec). At `5`, turning is quite slow. See the challenges below.

**Proximity message (`OnTriggerEnter` / `OnTriggerExit`)**

```csharp
void OnTriggerEnter(Collider col)
{
    if (col.gameObject.name == "Interactable0")
    {
        canvas.enabled = true;
        outputText.text = "Collided with Interactable0";
    }
}

void OnTriggerExit(Collider col)
{
    if (col.gameObject.name == "Interactable0")
    {
        canvas.enabled = false;
        outputText.text = "";
    }
}
```

Enter the trigger and the message shows. Leave and it hides. Make `Interactable0`'s trigger collider bigger than the object itself to create a "zone" around it.

| Field | Type | Description |
|---|---|---|
| `speed` | `float` | Walk speed (units/sec) and turn speed (degrees/sec). Default `5`. |
| `canvas` | `Canvas` | The message canvas. Hidden at start. |
| `outputText` | `TMP_Text` | The `output` text object. |

---

## Prefabs and Tags

[Assets/prefabs](Assets/prefabs) holds the three collectibles:

| Prefab | Tag (must match exactly) | Collider |
|---|---|---|
| `cube.prefab` | `cube` | Is Trigger ✓ |
| `Sphere.prefab` | `sphere` | Is Trigger ✓ |
| `Cylinder.prefab` | `cylinder` | Is Trigger ✓ |

The tags already exist in this project. If you make your own collectibles, here's how to add a tag:

1. Select the prefab (double-click it in `Assets/prefabs`) or an object in the Hierarchy.
2. At the top of the **Inspector**, open the **Tag** dropdown and choose **Add Tag...**
3. Under **Tags**, click **+**, type the name, and click **Save**.
4. Select your object again, open the **Tag** dropdown and pick the new tag.
5. Check that its collider has **Is Trigger** enabled.

If you add a new tag, also add it to the `if` in `CMTopMove.OnTriggerEnter`, or it won't be collected.

---

## Build It Yourself: Recreating a Scene

Try rebuilding **CMTop** in a new scene to make sure you understand every piece.

1. **File > New Scene**, then save it in your own folder.
2. **Floor:** *GameObject > 3D Object > Plane*. Scale it to `(2, 1, 2)` so it covers −10 to 10.
3. **Player:** *GameObject > 3D Object > Capsule*, renamed to `Character`. Set Y position to `1`.
   - **Add Component > Character Controller**
   - **Add Component > CMTopMove**
   - The capsule's own Capsule Collider can be removed. The CharacterController acts as its collider.
4. **UI:** *GameObject > UI > Panel*. This creates a Canvas and EventSystem too. Then right-click the Panel and choose *UI > Text - TextMeshPro*. Import TMP Essentials if asked.
5. **Spawner:** *GameObject > Create Empty*, renamed to `script`, then **Add Component > PlaceObjects**. Set the `prefabs` size to 3 and drag in the cube, sphere and cylinder prefabs.
6. **Wire up `CMTopMove`** on Character:
   - `Canvas` ← drag the **Canvas**
   - `Output Text` ← drag the **TextMeshPro text**
   - `Amount` ← drag the **script** object
7. Position the **Main Camera** above the floor looking down (e.g. position `(0, 20, 0)`, rotation `(90, 0, 0)`).
8. Press **Play**.

---

## Troubleshooting

| Problem | Likely cause / fix |
|---|---|
| **`NullReferenceException` in `Start()`** | A field in the Inspector is empty. Check `Canvas`, `Output Text` and (for CMTop) `Amount`. |
| **Player doesn't move** | Click inside the Game view first. Make sure the object has a **CharacterController**. Check `Speed` isn't `0` in the Inspector. |
| **`NullReferenceException` in `Update()`** | `FindAction("Move")` found nothing. The action was renamed, or the project's input actions asset isn't set (Edit > Project Settings > Input System Package). |
| **Walking into objects does nothing** | The object's collider needs **Is Trigger** checked, and the tag/name must match the code exactly (case-sensitive). |
| **"All objects found!" never shows** | `numberOfObjects` must match how many collectibles are really in the scene. Extra objects placed by hand aren't counted. |
| **`ArgumentException` / missing object when spawning** | A slot in the `prefabs` array is empty. |
| **Console flooded with numbers** | `CMThirdPersonMove` logs `rotateYAxis` every frame. Comment out that `Debug.Log` line. |
| **Text is pink or missing** | Import **TMP Essential Resources** (Window > TextMeshPro > Import TMP Essential Resources). |

---

## Challenges

Start small and test after each change.

1. **Tune it:** Change `speed` in the Inspector while the game is playing. (Changes made during Play mode reset when you stop.)
2. **Faster turning:** Add a separate `[SerializeField] float turnSpeed = 120f;` to `CMThirdPersonMove` and use it for `rotateYAxis`.
3. **Live score:** In `CMTopMove`, update `outputText` after every pickup, e.g. `"Found 2 / 5"`. You'll need to show the canvas earlier.
4. **Count by type:** Use the `ObjectsFound` array to report how many cubes, spheres and cylinders you collected.
5. **New collectible:** Make a new prefab with its own tag and add it to the spawner and the `if` statement.
6. **Several interactables:** Give each interactable a tag like `"interactable"` and show its **name** in the message instead of a hard-coded string.
7. **Sound:** Add an `AudioSource` and play a clip in `OnTriggerEnter`.
8. **Combine the scenes:** Use third-person controls in the collection game.
