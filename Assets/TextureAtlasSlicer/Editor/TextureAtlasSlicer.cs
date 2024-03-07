using System;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SimpleSpriteAnimator;
using UnityEditor.PackageManager.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System.Threading.Tasks;

public class TextureAtlasSlicer : EditorWindow
{
    public Texture logo;

    [MenuItem("CONTEXT/TextureImporter/Slice Sprite Using XML")]
    public static void SliceUsingXML(MenuCommand command)
    {
        var textureImporter = command.context as TextureImporter;

        var window = CreateInstance<TextureAtlasSlicer>();

        window.importer = textureImporter;

        window.ShowUtility();
    }

    [MenuItem("UNITY PARTY ENGINE/Slice Sprite Using XML")]
    public static void TextureAtlasSlicerWindow()
    {
        var window = CreateInstance<TextureAtlasSlicer>();

        window.Show();
    }

    [MenuItem("CONTEXT/TextureImporter/Slice Sprite Using XML", true)]
    public static bool ValidateSliceUsingXML(MenuCommand command)
    {
        var textureImporter = command.context as TextureImporter;

        //valid only if the texture type is 'sprite' or 'default'.
        return textureImporter && textureImporter.textureType == TextureImporterType.Sprite ||
               textureImporter.textureType == TextureImporterType.Default;
    }

    public TextureImporter importer;

    public TextureAtlasSlicer()
    {
        titleContent = new GUIContent("XML Slicer");
    }


    [SerializeField]
    private TextAsset xmlAsset;

    public SpriteAlignment spriteAlignment = SpriteAlignment.Center;

    public Vector2 customOffset = new Vector2(0.5f, 0.5f);

    public void OnSelectionChange()
    {
        UseSelectedTexture();
    }

    private Texture2D selectedTexture;

    private void UseSelectedTexture()
    {
        if (Selection.objects.Length > 1)
        {
            selectedTexture = null;
        }
        else
        {
            selectedTexture = Selection.activeObject as Texture2D;
        }

        if (selectedTexture != null)
        {
            var assetPath = AssetDatabase.GetAssetPath(selectedTexture);

            importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer)
            {
                xmlAsset = null;
                subTextures = null;

                if (!TryGetXML(assetPath))
                {
                    // Try to get text formatted sprite layout
                    if (!TryGetTXT(assetPath))
                    {
                        xmlAsset = null;
                        subTextures = null;
                    }
                }
            }
            else
            {
                xmlAsset = null;
                subTextures = null;
            }
        }
        else
        {
            importer = null;
            xmlAsset = null;
            subTextures = null;
        }

        Repaint();
    }

    private bool TryGetXML(string assetPath)
    {
        string extension = Path.GetExtension(assetPath);
        string pathWithoutExtension = assetPath.Remove(assetPath.Length - extension.Length, extension.Length);
        string xmlPath = pathWithoutExtension + ".xml";

        var temp = AssetDatabase.LoadAssetAtPath(xmlPath, typeof(TextAsset));

        if (temp != null)
        {
            xmlAsset = temp as TextAsset;
            ParseXML();
            return true;
        }
        else
            return false;
    }

    #region TXT
    private bool TryGetTXT(string assetPath)
    {
        string extension = Path.GetExtension(assetPath);
        string pathWithoutExtension = assetPath.Remove(assetPath.Length - extension.Length, extension.Length);
        string xmlPath = pathWithoutExtension + ".txt";

        var temp = AssetDatabase.LoadAssetAtPath(xmlPath, typeof(TextAsset));

        if (temp != null)
        {
            xmlAsset = temp as TextAsset;
            ParseTxt();
            return true;
        }
        else
            return false;
    }

    private void ParseTxt()
    {
        try
        {
            string[] lines = Regex.Split(xmlAsset.text, "\r\n|\r|\n");
            if (lines == null || lines.Length <= 0)
            {
                subTextures = null;
                return;
            }

            List<SubTexture> parsedSubTextures = new List<SubTexture>();
            foreach (string item in lines)
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                string[] line = item.Split(' ');
                if (line != null || line.Length == 6)
                {
                    bool pass = true;
                    foreach (string data in line)
                    {
                        if (string.IsNullOrEmpty(data))
                        {
                            pass = false;
                            break;
                        }
                    }

                    if (!pass)
                        continue;

                    SubTexture subtexture = new SubTexture
                    {
                        name = line[0],
                        x = Convert.ToInt32(line[2]),
                        y = Convert.ToInt32(line[3]),
                        width = Convert.ToInt32(line[4]),
                        height = Convert.ToInt32(line[5])
                    };

                    parsedSubTextures.Add(subtexture);
                }
            }

            if (parsedSubTextures.Count > 0)
            {
                subTextures = parsedSubTextures.ToArray();
                SetWantedDimenstions();
            }
        }
        catch (Exception)
        {
            subTextures = null;
        }

    }
    #endregion

    private SubTexture[] subTextures;
    private int wantedWidth, wantedHeight;


    private bool needToCharacter = true;
    private bool isBF = false;
    private bool leftToRight = false;
    private bool createMiss = false;
    private string pathToCreateSpriteAnim;
    private string pathToCreateChar;

    private void ParseXML()
    {
        try
        {
            var document = new XmlDocument();
            document.LoadXml(xmlAsset.text);

            var root = document.DocumentElement;
            if (root == null || root.Name != "TextureAtlas")
            {
                return;
            }

            subTextures = root.ChildNodes
                              .Cast<XmlNode>()
                              .Where(childNode => childNode.Name == "SubTexture")
                              .Select(childNode => GetSubtexture(childNode))
                              .ToArray();

            SetWantedDimenstions();

        }
        catch (Exception /*e*/)
        {
            //Debug.LogException(e);
            subTextures = null;
        }
    }

    private void SetWantedDimenstions()
    {
        wantedWidth = 0;
        wantedHeight = 0;

        foreach (var subTexture in subTextures)
        {
            var right = subTexture.x + subTexture.width;
            var bottom = subTexture.y + subTexture.height;

            wantedWidth = Mathf.Max(wantedWidth, right);
            wantedHeight = Mathf.Max(wantedHeight, bottom);
        }
    }

    private SubTexture GetSubtexture(XmlNode childNode)
    {
        if (childNode == null)
        {
            Debug.LogError("Childnode is null");
            return new SubTexture();
        }

        int w = 0;
        int h = 0;
        int _x = 0;
        int _y = 0;
        int _fx = int.MaxValue;
        int _fy = int.MaxValue;
        int _fw = int.MaxValue;
        int _fh = int.MaxValue;

        string _name = "ERROR";
        if (childNode.Attributes["name"] != null)
            _name = childNode.Attributes["name"].Value;
        else
            Debug.LogError("'name' attribute not found on childNode");

        if (childNode.Attributes["width"] != null)
            w = Convert.ToInt32(childNode.Attributes["width"].Value);
        else
            Debug.LogError("'width' attribute not found on childNode: " + _name);

        if (childNode.Attributes["height"] != null)
            h = Convert.ToInt32(childNode.Attributes["height"].Value);
        else
            Debug.LogError("'height' attribute not found on childNode: " + _name);

        if (childNode.Attributes["x"] != null)
            _x = Convert.ToInt32(childNode.Attributes["x"].Value);
        else
            Debug.LogError("'x' attribute not found on childNode: " + _name);

        if (childNode.Attributes["y"] != null)
            _y = Convert.ToInt32(childNode.Attributes["y"].Value);
        else
            Debug.LogError("'y' attribute not found on childNode: " + _name);

        if (childNode.Attributes["frameX"] != null)
            _fx = Convert.ToInt32(childNode.Attributes["frameX"].Value);

        if (childNode.Attributes["frameY"] != null)
            _fy = Convert.ToInt32(childNode.Attributes["frameY"].Value);

        if (childNode.Attributes["frameWidth"] != null)
            _fw = Convert.ToInt32(childNode.Attributes["frameWidth"].Value);

        if (childNode.Attributes["frameHeight"] != null)
            _fh = Convert.ToInt32(childNode.Attributes["frameHeight"].Value);

        return new SubTexture
        {
            width = w,
            height = h,
            x = _x,
            y = _y,
            name = _name,
            offsetX = _fx,
            offsetY = _fy,
            frameWidth = _fw,
            frameHeight = _fh
        };
    }

    public void OnEnable()
    {
        UseSelectedTexture();
    }

    public void OnGUI()
    {
        if (!logo)
        {
            Debug.LogError("There are problem in logo!");
            return;
        }
        else
        {
            //GUI.DrawTexture(new Rect(45, -100, 400, 400), logo, ScaleMode.ScaleToFit, true);
            GUILayout.Box(logo, GUILayout.Width(400), GUILayout.Height(170));
        }
        if (importer == null)
        {
            EditorGUILayout.LabelField("Please select a texture to slice.");
            return;
        }
        EditorGUI.BeginDisabledGroup(focusedWindow != this);
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Texture", Selection.activeObject, typeof(Texture), false);

            string fullPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string characterFolder = Path.GetDirectoryName(fullPath);
            pathToCreateSpriteAnim = Path.GetDirectoryName(characterFolder) + "/Characters_Info";
            pathToCreateChar = pathToCreateSpriteAnim + "/_CharactersData";

            EditorGUI.EndDisabledGroup();

            if (importer.textureType != TextureImporterType.Sprite &&
                importer.textureType != TextureImporterType.Default)
            {
                EditorGUILayout.LabelField("The Texture Type needs to be Sprite or Default!");
            }

            EditorGUI.BeginDisabledGroup((importer.textureType != TextureImporterType.Sprite &&
                                          importer.textureType != TextureImporterType.Default));
            {
                EditorGUI.BeginChangeCheck();
                xmlAsset = EditorGUILayout.ObjectField("XML Source", xmlAsset, typeof(TextAsset), false) as TextAsset;
                if (EditorGUI.EndChangeCheck())
                {
                    ParseXML();
                }

                spriteAlignment = (SpriteAlignment)EditorGUILayout.EnumPopup("Pivot", spriteAlignment);

                EditorGUI.BeginDisabledGroup(spriteAlignment != SpriteAlignment.Custom);
                customOffset = EditorGUILayout.Vector2Field("Custom Offset", customOffset);
                EditorGUI.EndDisabledGroup();

                var needsToResizeTexture = wantedWidth > selectedTexture.width || wantedHeight > selectedTexture.height;

                if (xmlAsset != null && needsToResizeTexture)
                {
                    EditorGUILayout.LabelField(
                        string.Format("Texture size too small."
                                      + " It needs to be at least {0} by {1} pixels!",
                            wantedWidth,
                            wantedHeight));
                    EditorGUILayout.LabelField("Try changing the Max Size property in the importer.");
                }

                if (subTextures == null || subTextures.Length == 0)
                {
                    EditorGUILayout.LabelField("Could not find any SubTextures in XML.");
                }

                EditorGUI.BeginDisabledGroup(xmlAsset == null || needsToResizeTexture || subTextures == null ||
                                             subTextures.Length == 0);


                needToCharacter = GUILayout.Toggle(needToCharacter, "Character?");

                isBF = GUILayout.Toggle(isBF, "BF");
                EditorGUI.BeginDisabledGroup(!isBF);
                createMiss = GUILayout.Toggle(createMiss, "Create Miss Animations");
                leftToRight = GUILayout.Toggle(leftToRight, "Change Left animation to Right");
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Slice"))
                {
                    PerformSlice();
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUI.EndDisabledGroup();
        }
        EditorGUI.EndDisabledGroup();
    }

    private struct SubTexture
    {
        public int width;
        public int height;
        public int x;
        public int y;
        public int offsetX;
        public int offsetY;
        public int frameWidth;
        public int frameHeight;
        public string name;
    }
    private async void PerformSlice()
    {
        if (importer == null)
        {
            return;
        }

        var textureHeight = selectedTexture.height;

        var needsUpdate = false;

        if (importer.spriteImportMode != SpriteImportMode.Multiple)
        {
            needsUpdate = true;
            importer.spriteImportMode = SpriteImportMode.Multiple;
        }

        var wantedSpriteSheet = (from subTexture in subTextures
                                 let actualY = textureHeight - (subTexture.y + subTexture.height)
                                 select new SpriteMetaData
                                 {
                                     alignment = (subTexture.offsetX == int.MaxValue || subTexture.offsetY == int.MaxValue) ?
                                        (int)spriteAlignment : (int)SpriteAlignment.Custom,
                                     border = new Vector4(),
                                     name = subTexture.name,
                                     pivot = GetPivotValue(spriteAlignment, customOffset, subTexture),
                                     rect = new Rect(subTexture.x, actualY, subTexture.width, subTexture.height)
                                 }).ToArray();
        if (!needsUpdate && !importer.spritesheet.SequenceEqual(wantedSpriteSheet))
        {
            needsUpdate = true;
            importer.spritesheet = wantedSpriteSheet;
        }

        if (needsUpdate)
        {
            EditorUtility.SetDirty(importer);

            try
            {
                AssetDatabase.StartAssetEditing();
                string assetPath = importer.assetPath;
                AssetDatabase.ImportAsset(importer.assetPath);

                EditorUtility.DisplayDialog("Success!", "The sprite was sliced successfully.", "OK");

                // load all of the sprites
                
                var objectsInPath = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(selectedTexture));
                List<Sprite> spriteSheetSprites = new List<Sprite>();
                for (int i = 0; i < objectsInPath.Length; i++)
                {
                    if (objectsInPath[i] is Sprite spr)
                    {
                        spriteSheetSprites.Add(spr);
                        spriteSheetSprites.Add(spr);
                    }
                }
                //Create a character Animations

                // Finish creation
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorUtility.DisplayDialog("Error", "There was an exception while trying to reimport the image." +
                                                     "\nPlease check the console log for details.", "OK");
            }
            finally
            {
                AssetDatabase.StopAssetEditing();

                await Task.Delay(1500);
                
                if (needToCharacter)
                {
                    var objectsInPath = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(selectedTexture));
                    List<Sprite> spriteSheetSprites = new List<Sprite>();
                    for (int i = 0; i < objectsInPath.Length; i++)
                    {
                        if (objectsInPath[i] is Sprite spr)
                        {
                            spriteSheetSprites.Add(spr);
                        }
                    }
                    var desiredPathSplited = AssetDatabase.GetAssetPath(selectedTexture).Split('/');
                    var desiredPath = string.Join('/', desiredPathSplited.Take(desiredPathSplited.Length - 1));
                    // MyClass is inheritant from ScriptableObject base class
                    string[] namesDad = new string[] { "up", "down", "left", "right", "idle", "Dad Sing Note DOWN", "Dad Sing Note LEFT", "Dad Sing Note RIGHT", "Dad Sing Note UP", "Dad idle dance" };

                    string[] namesBf = new string[] { "up", "down", "left", "right", "idle", "missup", "missdown", "missleft", "missright",
                "BF NOTE DOWN", "BF NOTE DOWN ALT", "BF NOTE DOWN MISS", "BF NOTE LEFT", "BF NOTE LEFT ALT", "BF NOTE LEFT MISS",
                "BF NOTE RIGHT", "BF NOTE RIGHT ALT", "BF NOTE RIGHT MISS", "BF NOTE UP", "BF NOTE UP ALT", "BF NOTE UP MISS", "BF idle dance"};

                    string[] namessDad = new string[] { "Sing Up", "Sing Down", "Sing Left", "Sing Right", "Idle", "Sing Down", "Sing Left", "Sing Right", "Sing Up", "Idle" };

                    string[] namessBF = new string[] { "BF Sing Up", "BF Sing Down", "BF Sing Left", "BF Sing Right", "BF Idle", "BF Sing Up Miss", "BF Sing Down Miss", "BF Sing Left Miss", "BF Sing Right Miss",
                "BF Sing Down", "BF Sing Down Alt", "BF Sing Down Miss", "BF Sing Left", "BF Sing Left Alt", "BF Sing Left Miss",
                "BF Sing Right", "BF Sing Right Alt", "BF Sing Right Alt", "BF Sing Up", "BF Sing Up Alt", "BF Sing Up Miss", "BF Idle"};

                    AssetDatabase.CreateFolder(pathToCreateSpriteAnim, selectedTexture.name);
                    string pathToCreatePoses = pathToCreateSpriteAnim + "/" + selectedTexture.name;
                    if (!isBF)
                    {
                       
                        
                        for (int i = 0; i < namesDad.Length; i++)
                        {
                            List<SpriteAnimationFrame> frames = new List<SpriteAnimationFrame>();

                            for (int j = 0; j < spriteSheetSprites.Count; j++)
                            {
                                string spriteNameLower = spriteSheetSprites[j].name.ToLower();
                                string targetNameLower = namesDad[i].ToLower();

                                // Check if the spriteNameLower contains the targetNameLower followed by a non-digit character or the end of the string
                                if (spriteNameLower.Contains(targetNameLower) &&
                                    (spriteNameLower.Length == (targetNameLower.Length + 4)))
                                {
                                    SpriteAnimationFrame f = new SpriteAnimationFrame();
                                    f.Sprite = spriteSheetSprites[j];
                                    frames.Add(f);
                                }
                            }

                            SpriteAnimation spriteAnimation = ScriptableObject.CreateInstance<SpriteAnimation>();

                            spriteAnimation.Frames = frames;

                            spriteAnimation.SpriteAnimationType = SpriteAnimationType.PlayOnce;
                            // Path has to start at "Assets"
                            spriteAnimation.Name = namessDad[i];
                            
                            
                            AssetDatabase.CreateAsset(spriteAnimation, $"{pathToCreatePoses}\\{namessDad[i]}.asset");


                        }
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();

                        // Start making character:

                        Character character = ScriptableObject.CreateInstance<Character>();
                        character.characterName = selectedTexture.name;
                        string[] guids = AssetDatabase.FindAssets("t:SpriteAnimation", new string[] { pathToCreatePoses });
                        List<SpriteAnimation> anim = new List <SpriteAnimation>();

                        foreach(string guid in guids)
                        {
                            string path = AssetDatabase.GUIDToAssetPath(guid);
                            SpriteAnimation animation = AssetDatabase.LoadAssetAtPath<SpriteAnimation>(path);
                            character.animations.Add(animation);
                        }

                        /*string pathToIcon = "Assets/__MOD ASSETS/_FNAF3/Images/icons/icon-" + selectedTexture.name;
                        Texture2D texIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(pathToIcon);
                        if (!texIcon)
                        {
                            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(pathToIcon).OfType<Sprite>().ToArray();
                            foreach(Sprite sprite in sprites)
                            {
                                if (sprite.name == $"icon-{selectedTexture.name}_0")
                                {
                                    character.portraitWin = sprite;
                                    character.portrait = sprite;
                                }
                                else if (sprite.name == $"icon-{selectedTexture.name}_1")
                                    character.portraitDead = sprite;
                            }
                        }

                       /* character.healthColor = GetDominantColor(texIcon);
                        character.songDurationColor = GetDominantColor(texIcon);*/

                        AssetDatabase.CreateAsset(character, $"{pathToCreateChar}/{selectedTexture.name}.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }
                    else
                    {
                        if (createMiss)
                        {
                            for (int i = 0; i < namesBf.Length; i++)
                            {
                                List<SpriteAnimationFrame> frames = new List<SpriteAnimationFrame>();

                                for (int j = 0; j < spriteSheetSprites.Count; j++)
                                {
                                    string spriteNameLower = spriteSheetSprites[j].name.ToLower();
                                    string targetNameLower = namesBf[i].ToLower();

                                    if (spriteNameLower.Contains(targetNameLower) &&
                                        (spriteNameLower.Length == (targetNameLower.Length + 4)))
                                    {
                                        SpriteAnimationFrame f = new SpriteAnimationFrame();
                                        f.Sprite = spriteSheetSprites[j];
                                        frames.Add(f);
                                    }
                                }

                                SpriteAnimation spriteAnimation = ScriptableObject.CreateInstance<SpriteAnimation>();

                                spriteAnimation.Frames = frames;

                                spriteAnimation.SpriteAnimationType = SpriteAnimationType.PlayOnce;
                                // Path has to start at "Assets"
                                spriteAnimation.Name = namessBF[i];
                                AssetDatabase.CreateAsset(spriteAnimation, $"{pathToCreatePoses}\\{namessBF[i]}.asset");


                                Protagonist character = ScriptableObject.CreateInstance<Protagonist>();
                                character.characterName = selectedTexture.name;
                                string[] guids = AssetDatabase.FindAssets("t:SpriteAnimation", new string[] { pathToCreatePoses });
                                List<SpriteAnimation> anim = new List<SpriteAnimation>();

                                foreach (string guid in guids)
                                {
                                    string path = AssetDatabase.GUIDToAssetPath(guid);
                                    SpriteAnimation animation = AssetDatabase.LoadAssetAtPath<SpriteAnimation>(path);
                                    character.animations.Add(animation);
                                }


                                AssetDatabase.CreateAsset(character, $"{pathToCreateChar}/{selectedTexture.name}.asset");
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();

                            }
                        }
                        else
                        {
                            for (int i = 0; i < namesBf.Length; i++)
                            {
                                List<SpriteAnimationFrame> frames = new List<SpriteAnimationFrame>();

                                for (int j = 0; j < spriteSheetSprites.Count; j++)
                                {
                                    string spriteNameLower = spriteSheetSprites[j].name.ToLower();
                                    string targetNameLower = namesBf[i].ToLower();

                                    if (spriteNameLower.Contains(targetNameLower) &&
                                        (spriteNameLower.Length == (targetNameLower.Length + 4)))
                                    {
                                        SpriteAnimationFrame f = new SpriteAnimationFrame();
                                        f.Sprite = spriteSheetSprites[j];
                                        frames.Add(f);
                                    }
                                }

                                SpriteAnimation spriteAnimation = ScriptableObject.CreateInstance<SpriteAnimation>();

                                spriteAnimation.Frames = frames;

                                spriteAnimation.SpriteAnimationType = SpriteAnimationType.PlayOnce;
                                // Path has to start at "Assets"
                                spriteAnimation.Name = namessBF[i];
                                if (leftToRight)
                                {
                                    if (i == 2)
                                    {
                                        spriteAnimation.Name = namessBF[3];
                                        AssetDatabase.CreateAsset(spriteAnimation, $"{pathToCreatePoses}\\{namessBF[3]}.asset");
                                    }
                                    else if (i == 3)
                                    {
                                        spriteAnimation.Name = namessBF[2];
                                        AssetDatabase.CreateAsset(spriteAnimation, $"{pathToCreatePoses}\\{namessBF[2]}.asset");
                                    }
                                    else
                                    {
                                        spriteAnimation.Name = namessBF[i];
                                        AssetDatabase.CreateAsset(spriteAnimation, $"{pathToCreatePoses}\\{namessBF[i]}.asset");
                                    }
                                }
                                else
                                {
                                    spriteAnimation.Name = namessBF[i];
                                    AssetDatabase.CreateAsset(spriteAnimation, $"{pathToCreatePoses}\\{namessBF[i]}.asset");
                                }

                                Protagonist character = ScriptableObject.CreateInstance<Protagonist>();
                                character.characterName = selectedTexture.name;
                                string[] guids = AssetDatabase.FindAssets("t:SpriteAnimation", new string[] { pathToCreatePoses });
                                List<SpriteAnimation> anim = new List<SpriteAnimation>();

                                foreach (string guid in guids)
                                {
                                    string path = AssetDatabase.GUIDToAssetPath(guid);
                                    SpriteAnimation animation = AssetDatabase.LoadAssetAtPath<SpriteAnimation>(path);
                                    character.animations.Add(animation);
                                }


                                AssetDatabase.CreateAsset(character, $"{pathToCreateChar}/{selectedTexture.name}.asset");
                                AssetDatabase.SaveAssets();
                                AssetDatabase.Refresh();
                            }
                        }
                    }

                }



            }
        }
        else
        {
            EditorUtility.DisplayDialog("Nope!", "The sprite is already sliced according to this XML file.", "OK");
        }
    }
    /*public Color GetDominantColor(Texture2D texture)
    {
        Color[] texColors = texture.GetPixels();
        int total = texColors.Length;

        float r = 0;
        float g = 0;
        float b = 0;

        for (int i = 0; i < total; i++)
        {
            r += texColors[i].r;
            g += texColors[i].g;
            b += texColors[i].b;
        }

        return new Color(r / total, g / total, b / total, 100);
    }*/

    //SpriteEditorUtility
    private static Vector2 GetPivotValue(SpriteAlignment alignment, Vector2 customOffset, SubTexture subTexture)
    {
        if (subTexture.offsetX == int.MaxValue || subTexture.offsetY == int.MaxValue)
        {
            switch (alignment)
            {
                case SpriteAlignment.Center:
                    return new Vector2(0.5f, 0.5f);
                case SpriteAlignment.TopLeft:
                    return new Vector2(0.0f, 1f);
                case SpriteAlignment.TopCenter:
                    return new Vector2(0.5f, 1f);
                case SpriteAlignment.TopRight:
                    return new Vector2(1f, 1f);
                case SpriteAlignment.LeftCenter:
                    return new Vector2(0.0f, 0.5f);
                case SpriteAlignment.RightCenter:
                    return new Vector2(1f, 0.5f);
                case SpriteAlignment.BottomLeft:
                    return new Vector2(0.0f, 0.0f);
                case SpriteAlignment.BottomCenter:
                    return new Vector2(0.5f, 0.0f);
                case SpriteAlignment.BottomRight:
                    return new Vector2(1f, 0.0f);
                case SpriteAlignment.Custom:
                    return customOffset;
                default:
                    return Vector2.zero;
            }
        }
        else
        {
            return new Vector2(subTexture.offsetX / (float)(subTexture.width), 0 / (float)(subTexture.height));
        }
    }
}