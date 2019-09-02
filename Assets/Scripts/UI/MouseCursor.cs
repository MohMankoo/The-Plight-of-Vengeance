using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour {

    [Header("Game Cursors")]
    public TextAsset zykaCursor;
    public TextAsset XGunCursor;
    public TextAsset VGunCursor;
    private Texture2D gameCursorTexture;

    // Setup gameCursorTexture and default it to the cursor for the Zyka gun
    private void Start() {
        // Note: size of texture does not matter, as the png TextAsset image will replace it
        gameCursorTexture = new Texture2D(2, 2);
        gameCursorTexture.LoadImage(zykaCursor.bytes);
    }

    public void EnableGameCursor() {
        Cursor.SetCursor(gameCursorTexture, Vector2.zero, CursorMode.Auto);
    }

    // Note: menu cursor is just the default cursor
    public void EnableMenuCursor() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void ChangeGameCursorType(string gunType) {
        switch (gunType) {
            case "X-Gun":
                gameCursorTexture = new Texture2D(2, 2);
                gameCursorTexture.LoadImage(XGunCursor.bytes);
                break;
            case "V-Gun":
                gameCursorTexture = new Texture2D(2, 2);
                gameCursorTexture.LoadImage(VGunCursor.bytes);
                break;
            default:
                gameCursorTexture = new Texture2D(2, 2);
                gameCursorTexture.LoadImage(zykaCursor.bytes);
                break;
        }
    }

}
