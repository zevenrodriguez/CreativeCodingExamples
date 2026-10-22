# Sample Scripts

Two small Unity C# scripts that show common building blocks for interactive projects: playing sound effects and clicking on objects in a 3D scene.

| Script | What it does |
| --- | --- |
| [AudioManager.cs](AudioManager.cs) | Plays a "good" or "bad" sound clip when asked to by name |
| [RayCasting.cs](RayCasting.cs) | Detects which 3D object is under the mouse and logs its name when you click |

---

## AudioManager.cs

A simple sound player. It holds two audio clips and plays one of them when another script (or a UI button) calls `playClip`.

### How it works

- **Fields**
  - `audioSource`: the `AudioSource` component that actually plays the sound. It's found automatically in `Start()` with `GetComponent<AudioSource>()`, so the AudioSource **must be on the same GameObject** as this script.
  - `good` and `bad`: two `AudioClip`s. `[SerializeField]` keeps them private but makes them show up in the Inspector so you can drag sound files onto them.
- **`playClip(string clip)`**: a public method that takes the name of the sound you want:
  - `"good"` swaps the AudioSource's clip to `good` and plays it.
  - `"bad"` swaps the clip to `bad` and plays it.
  - Any other string does nothing.
- `Update()` is empty and can be deleted. It's left over from Unity's default script template.

### Setup

1. Create an empty GameObject (e.g. `AudioManager`).
2. Add an **Audio Source** component and the **AudioManager** script to it.
3. Drag your sound files into the **Good** and **Bad** slots in the Inspector.
4. Call it from somewhere:
   - **From a UI Button:** in the Button's **On Click ()** list, click **+**, drag in the AudioManager object, choose `AudioManager > playClip (string)`, and type `good` or `bad` in the text box.
   - **From another script:**
     ```csharp
     [SerializeField] AudioManager audioManager;

     void OnCorrectAnswer()
     {
         audioManager.playClip("good");
     }
     ```

### Things to try

- Add more clips (e.g. `win`, `lose`, `click`) and more `else if` branches.
- Use `audioSource.PlayOneShot(clip)` instead of setting `.clip` and calling `Play()`. That way sounds can overlap instead of cutting each other off.
- Add `[RequireComponent(typeof(AudioSource))]` above the class so Unity adds the AudioSource for you automatically.

---

## RayCasting.cs

Shows how to "click on" objects in a 3D scene. Every frame it shoots an invisible line (a **ray**) from the camera through the mouse position. If the ray hits something and the left mouse button was just pressed, it prints that object's name to the Console.

### How it works

1. **Read the mouse position.** `Mouse.current.position` gives the cursor's position in *screen pixels* (from the bottom-left corner of the Game view).
2. **Turn it into a ray.** `cam.ScreenPointToRay(mousePosition)` creates a `Ray` that starts at the camera and points into the scene through that pixel.
3. **Visualize it.** `Debug.DrawRay(...)` draws a yellow line 10 units long so you can see the ray in the **Scene** view while the game is running. It's for debugging only and doesn't appear in a build.
4. **Check for a hit.** `Physics.Raycast(camMouseRay, out RaycastHit hit)` returns `true` if the ray hits a collider. Information about what it hit is stored in `hit`.
5. **React to a click.** If the left mouse button was pressed *this frame* (`wasPressedThisFrame`), it logs `hit.collider.gameObject.name`.

### Setup

1. This script uses the **new Input System** (`using UnityEngine.InputSystem;`). Make sure the **Input System** package is installed (Window > Package Manager) and that **Project Settings > Player > Active Input Handling** is set to *Input System Package* or *Both*.
2. Add the script to any GameObject in the scene (the camera works fine).
3. Drag your **Main Camera** into the **Cam** slot in the Inspector.
4. Make sure the objects you want to click have a **3D Collider** (Box Collider, Mesh Collider, etc.). Objects without a collider, or with only 2D colliders, won't be detected.
5. Press Play, click on objects, and watch the **Console** for their names.

### Things to try

- Replace the `Debug.Log` with something visible: change the object's color, destroy it, or call the AudioManager's `playClip("good")`.
- Check what you clicked with `hit.collider.CompareTag("Target")` so only certain objects respond.
- Use `hit.point` to get the exact spot that was clicked, e.g. to spawn an object there.
- Pass a `LayerMask` to `Physics.Raycast` so the ray ignores certain layers.

---

## Using them together

These two scripts combine easily. For example, give `RayCasting` a reference to the `AudioManager` and play a sound when the player clicks the right object:

```csharp
[SerializeField] AudioManager audioManager;

// inside the click check in Update()
if (hit.collider.CompareTag("Target"))
    audioManager.playClip("good");
else
    audioManager.playClip("bad");
```
