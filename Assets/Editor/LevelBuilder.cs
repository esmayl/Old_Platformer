using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;


public class LevelBuilder : EditorWindow  {

    public static GameObject[] decorations;
    public static GameObject[] platforms;
    public static GameObject[] hazards;
    public static GameObject[] doors;
    public static GameObject[] enemies;
    public static GameObject[] bosses;
    public static GameObject[] items;
    public static Material[] materials;
    public static Camera sceneCam;

    bool item = false;
    bool preview = false;
    GameObject previewObject;
    GameObject objToPlace;
    Material selectedMaterial = new Material(Shader.Find("Standard"));

    Vector3 spawnPos;
    float depth = -3f;
    Vector3 mousePos;
    Vector3 tempScale = Vector3.one;

    int buttonWidth =110;
    string groupName ="";
    bool canGroup = false;
    bool hazard = false;
    bool snap = false;
    static LevelBuilder window;
    List<GameObject> instanceList = new List<GameObject>();
    int mask;



    Vector2 decorationsScroll;
    Vector2 platformScroll;
    Vector2 hazardScroll;
    Vector2 doorScroll;
    Vector2 enemyScroll;
    Vector2 bossScroll;
    Vector2 itemScroll;
    Vector2 textureScroll;
    Vector2 oldDelta;

    Vector3 mouseStart = new Vector3();
    float tempMouseHeight = 0;


    void OnEnable() 
    { 
        SceneView.onSceneGUIDelegate += OnSceneGUI;        
        SceneView.lastActiveSceneView.orthographic = true;
        SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));
    }

    void OnDisable() { SceneView.onSceneGUIDelegate -= OnSceneGUI; SceneView.lastActiveSceneView.orthographic = false; DragAndDrop.PrepareStartDrag(); }

    [MenuItem("File/LevelBuilder")]
	static void Init () 
    {
        UnityEngine.Object[] tempObjArray=Resources.LoadAll("Hazards");
        hazards = new GameObject[tempObjArray.Length];
        for(int i = 0;i<tempObjArray.Length;i++){
            hazards[i] = (GameObject)tempObjArray[i];
        }

        tempObjArray = Resources.LoadAll("Doors");
        doors = new GameObject[tempObjArray.Length];
        for(int i = 0;i<tempObjArray.Length;i++){
            doors[i] = (GameObject)tempObjArray[i];
        }

        tempObjArray = Resources.LoadAll("Platforms");
        platforms = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            platforms[i] = (GameObject)tempObjArray[i];
        }
        tempObjArray = Resources.LoadAll("Enemies");
        enemies = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            enemies[i] = (GameObject)tempObjArray[i];
        }
        tempObjArray = Resources.LoadAll("Bosses");
        bosses = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            bosses[i] = (GameObject)tempObjArray[i];
        }
        tempObjArray = Resources.LoadAll("Materials");
        materials = new Material[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            materials[i] = tempObjArray[i] as Material;
        }

        tempObjArray = Resources.LoadAll("Decoration");
        decorations = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            decorations[i] = (GameObject)tempObjArray[i];
        }

        tempObjArray = Resources.LoadAll("Items");
        items = new GameObject[tempObjArray.Length];
        for (int i = 0; i < tempObjArray.Length; i++)
        {
            items[i] = (GameObject)tempObjArray[i];
        }

        window = (LevelBuilder)EditorWindow.GetWindow(typeof(LevelBuilder));
        window.minSize = new Vector2(410, 350);
        window.title = "LevelBuilder";

        sceneCam = GameObject.Find("SceneCamera").GetComponent<Camera>();

	}


    static bool IsMouseOver() 
    {
        return Event.current.type == EventType.Repaint && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition);
    }

    void OnGUI()
    {
        Event e = Event.current;

        if (preview)
        {
            GUILayout.Label("Press Left-Mouse to place object, press Escape to leave\n To group select multiple objects and select this window", GUILayout.Width(400));
            snap = GUILayout.Toggle(snap, "Snapping");
        }



        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Height(220));
                
                        EditorGUILayout.Space();
                        GUILayout.Label("Platforms");
                        platformScroll = EditorGUILayout.BeginScrollView(platformScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < platforms.Length;i++ )
                        {
                            if (GUILayout.Button(platforms[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                if(previewObject != null){DestroyImmediate(previewObject);}
                                if (hazard) { hazard = false; }

                                SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));

                                objToPlace = platforms[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.GetComponent<Renderer>().material = selectedMaterial;
                                previewObject.layer = LayerMask.NameToLayer("Preview");

                                Selection.activeGameObject = previewObject;                
                                preview = true;
                            }
                        }
                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.Space();
                        GUILayout.Label("Hazards");
                        hazardScroll = EditorGUILayout.BeginScrollView(hazardScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < hazards.Length; i++)
                        {

                            if (GUILayout.Button(hazards[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                if (previewObject != null) { DestroyImmediate(previewObject); }

                                hazard = true;
                                SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));

                                objToPlace = hazards[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.layer = LayerMask.NameToLayer("Preview");

                                tempScale = previewObject.transform.localScale;

                                Selection.activeGameObject = previewObject;
                                preview = true;
                            }
                        }
                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.Space();
                        GUILayout.Label("Decorations");
                        decorationsScroll = EditorGUILayout.BeginScrollView(decorationsScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < decorations.Length; i++)
                        {
                            if (GUILayout.Button(decorations[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                if (hazard) { hazard = false; }

                                SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));

                                objToPlace = decorations[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.GetComponent<Renderer>().material = selectedMaterial;
                                previewObject.layer = LayerMask.NameToLayer("Preview");

                                Selection.activeGameObject = previewObject;
                                preview = true;
                            }
                        }
                        EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
                        EditorGUILayout.Space();
                        GUILayout.Label("Enemies");
                        enemyScroll = EditorGUILayout.BeginScrollView(enemyScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < enemies.Length; i++)
                        {

                            if (GUILayout.Button(enemies[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                if (hazard) { hazard = false; }

                                SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));

                                objToPlace = enemies[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.layer = LayerMask.NameToLayer("Preview");


                                Selection.activeGameObject = previewObject;
                                preview = true;
                            }
                        }
                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.Space();
                        GUILayout.Label("Bosses");
                        bossScroll = EditorGUILayout.BeginScrollView(bossScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < bosses.Length; i++)
                        {

                            if (GUILayout.Button(bosses[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                if (hazard) { hazard = false; }
                                
                                SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));

                                objToPlace = bosses[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.GetComponent<Renderer>().material = selectedMaterial;
                                previewObject.layer = LayerMask.NameToLayer("Preview");


                                Selection.activeGameObject = previewObject;
                                preview = true;
                            }
                        }
                        EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                        EditorGUILayout.Space();
                        GUILayout.Label("Doors");
                        doorScroll = EditorGUILayout.BeginScrollView(doorScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < doors.Length; i++)
                        {
                            
                            if (GUILayout.Button(doors[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                if (hazard) { hazard = false; }

                                SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));

                                objToPlace = doors[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.layer = LayerMask.NameToLayer("Preview");

                                Selection.activeGameObject = previewObject;
                                preview = true;
                            }
                        }
                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.Space();
                        GUILayout.Label("Items");
                        itemScroll = EditorGUILayout.BeginScrollView(itemScroll, GUILayout.Width(130), GUILayout.Height(100));
                        for (int i = 0; i < items.Length; i++)
                        {
                            if (GUILayout.Button(items[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                            {
                                if (previewObject != null) { DestroyImmediate(previewObject); }
                                if (hazard) { hazard = false; }
                                item = true;
                                SceneView.lastActiveSceneView.LookAt(SceneView.lastActiveSceneView.pivot, Quaternion.LookRotation(-Vector3.right));

                                objToPlace = items[i];
                                Selection.activeObject = SceneView.currentDrawingSceneView;
                                spawnPos = sceneCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
                                spawnPos.x = depth;
                                spawnPos.y = Mathf.Round(spawnPos.y);
                                spawnPos.z = Mathf.Round(spawnPos.z);
                                previewObject = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                                previewObject.transform.position = spawnPos;
                                previewObject.layer = LayerMask.NameToLayer("Preview");

                                Selection.activeGameObject = previewObject;
                                preview = true;
                            }
                        }
                        EditorGUILayout.EndScrollView();

                        EditorGUILayout.Space();                        
                        GUILayout.Label("Textures");
                            textureScroll = EditorGUILayout.BeginScrollView(textureScroll, GUILayout.Width(130), GUILayout.Height(100));
                            for (int i = 0; i < materials.Length; i++)
                            {
                                if (GUILayout.Button(materials[i].name, GUILayout.Width(buttonWidth), GUILayout.Height(20)))
                                {
                                    selectedMaterial= materials[i];
                                    if (previewObject != null)
                                    {
                                        if (previewObject.GetComponent<MeshFilter>())
                                        {
                                            previewObject.GetComponent<Renderer>().material = selectedMaterial;
                                        }
                                        else
                                        {
                                            previewObject.transform.GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
                                        }
                                    }
                                    if (Selection.gameObjects.Length > 0)
                                    {
                                        GameObject[] gas = Selection.gameObjects;
                                        foreach (GameObject ga in gas)
                                        {
                                            if (ga.GetComponent<MeshFilter>())
                                            {
                                                ga.GetComponent<Renderer>().material = selectedMaterial;
                                            }
                                            if (ga.name.Contains("(Group)"))
                                            {
                                                foreach (Transform t in ga.transform)
                                                {
                                                    t.GetChild(0).GetComponent<Renderer>().material = selectedMaterial;
                                                }
                                            }
                                        }
                                    }
                                    
                                }
                            }
                            EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.Height(150));
        if (Selection.activeGameObject) 
        {

            GameObject rotateAble = Selection.activeGameObject;
            float tempFloat = EditorGUILayout.FloatField("Rotation", rotateAble.transform.rotation.x);
            float calcFloat = tempFloat - rotateAble.transform.rotation.x;
            window.Repaint();            

            rotateAble.transform.Rotate(rotateAble.transform.right, calcFloat);

    }
        if (hazard && previewObject)
        {
            tempScale = EditorGUILayout.Vector3Field("Scale", tempScale);
            tempScale.x = previewObject.transform.localScale.x;
            tempScale.y = (float)Math.Round(tempScale.y,1);
            tempScale.z = (float)Math.Round(tempScale.z,1);
            previewObject.transform.localScale = tempScale;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.Height(150));
        if (canGroup)
        {
            GUILayout.Label("Group name:");
            groupName = EditorGUILayout.TextField(groupName, GUILayout.Width(100));
            if (GUILayout.Button("Group"))
            {
                GameObject group = new GameObject();
                group.name = groupName+"(Group)";
                foreach (GameObject ga in Selection.gameObjects)
                {
                    ga.transform.parent = group.transform;
                    ga.isStatic = true;
                }
                Selection.activeGameObject = group;
                group = null;
            }
        }

        EditorGUILayout.EndVertical();
        if (e.keyCode == KeyCode.Escape && preview)
        {
            window.Repaint();
            DestroyImmediate(previewObject);
            preview = false;
            hazard = false;
            item = false;
        }
    }

    void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (canGroup)
        {
            window.Repaint();
        }
        if (e.keyCode == KeyCode.Escape && preview)
        {
            window.Repaint();
            DestroyImmediate(previewObject);
            preview = false;
            hazard = false;
            item = false;
        }
        
        if (previewObject != null)
        {
            previewObject.layer = LayerMask.NameToLayer("Preview");
            Selection.activeGameObject = previewObject;
            mousePos = sceneCam.ScreenToWorldPoint(new Vector2(sceneCam.pixelRect.width -(sceneCam.pixelRect.width-e.mousePosition.x), sceneCam.pixelRect.height - e.mousePosition.y));
            mousePos.x = depth;
            mousePos.y = (float)Math.Round(mousePos.y,1);
            mousePos.z = (float)Math.Round(mousePos.z,1);
            previewObject.transform.position = mousePos;
        }
        if (Selection.activeGameObject) { window.Repaint(); }
        if (Selection.gameObjects.Length > 1) {canGroup = true;}
        if (Selection.gameObjects.Length <= 1) { canGroup = false; }


        if (e.isMouse && previewObject != null)
        {
            if (e.type == EventType.mouseDown)
            {
                mouseStart = sceneCam.ScreenToWorldPoint(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), sceneCam.pixelRect.height - e.mousePosition.y));
                mouseStart.x = depth;
                if (snap) { mouseStart.y = Mathf.Round(mouseStart.y); mouseStart.z = Mathf.Round(mouseStart.z); }
                else { mouseStart.y = (float)Math.Round(mouseStart.y, 1); mouseStart.z = (float)Math.Round(mouseStart.z,1); }
                oldDelta = e.delta;
            }

            if (e.type == EventType.mouseUp && e.button ==0)
            {

                mousePos = sceneCam.ScreenToWorldPoint(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), sceneCam.pixelRect.height - e.mousePosition.y));
                mousePos.x = depth;
                mask = (1 << objToPlace.layer);

                if (snap) { mousePos.y = Mathf.Round(mousePos.y); mousePos.z = Mathf.Round(mousePos.z); }
                else { mousePos.y = (float)Math.Round(mousePos.y, 1); mousePos.z = (float)Math.Round(mousePos.z, 1); }

                if (objToPlace.layer != LayerMask.NameToLayer("Enemy"))
                {
                    if (!Physics.SphereCast(sceneCam.ScreenPointToRay(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), sceneCam.pixelRect.height - e.mousePosition.y)), 0.3f, 1000, mask))
                    {
                        GameObject tempObj = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                        tempObj.transform.position = mousePos;
                        if (!hazard || !item) { tempObj.GetComponent<Renderer>().material = selectedMaterial; }
                        tempObj.transform.localScale = previewObject.transform.localScale;
                        tempObj.isStatic = true;
                        tempObj = null;
                    }
                }
                else
                {
                    GameObject tempObj = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
                    tempObj.transform.position = mousePos;
                    tempObj.transform.localScale = previewObject.transform.localScale;
                    tempObj = null;
                }

            }
        }

        if (e.type == EventType.mouseDown)
        {
            mouseStart = sceneCam.ScreenToWorldPoint(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), sceneCam.pixelRect.height - e.mousePosition.y));
            mouseStart.x = depth;
            if (snap) { mouseStart.y = Mathf.Round(mouseStart.y); mouseStart.z = Mathf.Round(mouseStart.z); }
            else { mouseStart.y = (float)Math.Round(mouseStart.y, 1); mouseStart.z = (float)Math.Round(mouseStart.z, 1); }
            tempMouseHeight = sceneCam.pixelRect.height - e.mousePosition.y;
        }

        //find out why fast drag spreads more and slow drag puts nice together
        //if (e.type == EventType.MouseDrag && previewObject != null)
        //{
        //    DragAndDrop.PrepareStartDrag();
        //
        //    DragAndDrop.StartDrag("TestDrag");
        //    
        //    e.Use();
        //
        //    if (e.delta.x < -0.1f)
        //    {
        //        mouseStart = sceneCam.ScreenToWorldPoint(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), tempMouseHeight));
        //        mouseStart.x = depth;
        //        if (!Physics.SphereCast(sceneCam.ScreenPointToRay(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), tempMouseHeight)), 0.3f, 1000, mask))
        //        {
        //            GameObject tempObj = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
        //            Vector3 placePos = mouseStart + (tempObj.transform.forward * -0.1f);
        //            tempObj.transform.position = placePos;
        //        }
        //
        //        oldDelta = e.delta;
        //
        //    }
        //    if (e.delta.x > 0.1f)
        //    {
        //        mouseStart = sceneCam.ScreenToWorldPoint(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), tempMouseHeight));
        //        mouseStart.x = depth;
        //        if (!Physics.SphereCast(sceneCam.ScreenPointToRay(new Vector2(sceneCam.pixelRect.width - (sceneCam.pixelRect.width - e.mousePosition.x), tempMouseHeight)), 0.3f, 1000, mask))
        //        {
        //            GameObject tempObj = PrefabUtility.InstantiatePrefab(objToPlace) as GameObject;
        //            tempObj.transform.position = mouseStart + (tempObj.transform.forward * 0.1f);
        //        }
        //        oldDelta = e.delta;
        //    }
        //  
        //    if (e.type == EventType.mouseUp)
        //    {
        //        DragAndDrop.PrepareStartDrag();
        //    }
        //        
        //}

    }
}
