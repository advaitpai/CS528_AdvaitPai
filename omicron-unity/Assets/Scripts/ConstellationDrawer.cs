using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationDrawer : StarDataLoader
{
    
    public TextAsset constellation_datafile;
    public TextAsset indian_constellation_datafile;
    public TextAsset romanian_constellation_datafile;
    public TextAsset norse_constellation_datafile;
    public TextAsset egyptian_constellation_datafile;
    public TextAsset boorong_constellation_datafile;
    


    public static List<string> constellation_lines;

    public static GameObject constellation_parent;

    public static GameObject indian_constellation;
    public static GameObject romanian_constellation;
    public static GameObject norse_constellation;

    public static GameObject egyptian_constellation;
    public static GameObject boorong_constellation;

    public static Dictionary<string, Vector3> constellations_star_positions; 

    // Start is called before the first frame update
    void Start()
    {
        constellation_lines = new List<string>();
        constellation_parent = new GameObject("Constellations");
        indian_constellation = new GameObject("Indian Constellations");
        romanian_constellation = new GameObject("Romanian Constellations");
        norse_constellation = new GameObject("Norse Constellations");
        egyptian_constellation = new GameObject("Egyptian Constellations");
        boorong_constellation = new GameObject("Boorong Constellations");
        constellations_star_positions = new Dictionary<string, Vector3>();
        StarDataLoader.scale = 1f;
        StarDataLoader.speed = 1f;
        readConstellation();
        showNorseConstellations();
        showBoorongConstellations();
        //InvokeRepeating("updateConstellations", 15.0f, 10.0f);
        showIndianConstellations();
        showRomanianConstellations();
        showEgyptianConstellations();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(StarDataLoader.stars_motion)
        {
            moveStar();
            updateConstellations();
            StarDataLoader.years += 0.3048f*75000000*Time.deltaTime*StarDataLoader.speed;
        }
        
    }
    void readConstellation()
    {
        var lines = constellation_datafile.text.Split('\n');
        for (var i = 0; i < lines.Length; i++)
        {
            if (i > 88)
            {
                break;
            }
            if (lines[i] != "")
            {
                constellation_lines.Add(lines[i]);
            }
        }
        
        int line_count = 0;
        for (var x = 0; x < constellation_lines.Count; x++)
        {
            var hip_id = constellation_lines[x].Split(' ');
            GameObject constellation = new GameObject(hip_id[0]);
            constellation.transform.parent = constellation_parent.transform;
            for (var i = 3; i < hip_id.Length-1; i+=2)
            {
                Vector3 point1 = new Vector3();
                Vector3 point2 = new Vector3();
                for (var j = 0; j < StarDataLoader.star_data.Count; j++)
                {
                    StarData star_val = StarDataLoader.star_data[j];
                    if(star_val.hip == "")
                    {
                        continue;
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i]))
                    {
                        point1 = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                        continue;
                    
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i+1]))
                    {
                        point2 = new Vector3(star_val.x0, star_val.y0, star_val.z0);  
                        continue;
                    }
                    if(point1 != new Vector3() && point2 != new Vector3())
                    {
                        break;
                    }
                
                } 
                if (point1 != new Vector3() && point2 != new Vector3())
                {
                    string lname = hip_id[i] +"-"+ hip_id[i+1];
                    GameObject line = new GameObject(lname);
                    line.transform.parent = constellation.transform;
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    lineRenderer.receiveShadows = false;
                    lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.03f;
                    lineRenderer.endWidth = 0.03f;
                    lineRenderer.SetPosition(0, point1);
                    lineRenderer.SetPosition(1, point2);
                    constellations_star_positions[hip_id[i]] = point1;
                    constellations_star_positions[hip_id[i+1]] = point2;
                    line_count++;
                }  
            } 
            
        }
    } 
    void moveStar()
    {
        
        for (var i = 0; i < StarDataLoader.stars_objects.Count; i++)
        {

            StarData star_val = StarDataLoader.star_data[i];
            star_val.x0 = star_val.x0 + (star_val.vx*Time.deltaTime*StarDataLoader.speed);
            star_val.y0 = star_val.y0 + (star_val.vy*Time.deltaTime*StarDataLoader.speed);
            star_val.z0 = star_val.z0 + (star_val.vz*Time.deltaTime*StarDataLoader.speed);
            star_data[i] = star_val;
            if (constellations_star_positions.ContainsKey(star_val.hip) && star_val.hip != "")
            {
                constellations_star_positions[star_val.hip] = new Vector3(star_val.x0*StarDataLoader.scale, star_val.y0*StarDataLoader.scale, star_val.z0*StarDataLoader.scale);
            }
            if(star_val.visible)
            {    
                StarDataLoader.stars_objects[i].transform.localScale = new Vector3(0.1f*StarDataLoader.scale,0.1f*StarDataLoader.scale,0.1f*StarDataLoader.scale);
                StarDataLoader.stars_objects[i].transform.position = new Vector3(star_val.x0*StarDataLoader.scale, star_val.y0*StarDataLoader.scale, star_val.z0*StarDataLoader.scale);
            }
        }

    }
    void updateConstellations()
    {
        
        for (var i = 0; i < constellation_parent.transform.childCount; i++)
        {
            GameObject constellation = constellation_parent.transform.GetChild(i).gameObject;
            for (var j = 0; j < constellation.transform.childCount; j++)
            {
                GameObject line = constellation.transform.GetChild(j).gameObject;
                LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                var hip_id = line.name.Split('-');
                lineRenderer.SetPosition(0,constellations_star_positions[hip_id[0]]);
                lineRenderer.SetPosition(1,constellations_star_positions[hip_id[1]]);
            }
        }
    }
    void showIndianConstellations()
    {
        var lines = indian_constellation_datafile.text.Split('\n');
        List<string> temp_constellation_lines = new List<string>();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i] != "")
            {
                temp_constellation_lines.Add(lines[i]);
            }
        }
        
        int line_count = 0;
        for (var x = 0; x < temp_constellation_lines.Count; x++)
        {
            var hip_id = temp_constellation_lines[x].Split(' ');
            GameObject constellation = new GameObject(hip_id[0]);
            constellation.transform.parent = indian_constellation.transform;
            for (var i = 2; i < hip_id.Length-1; i+=2)
            {
                Vector3 point1 = new Vector3();
                Vector3 point2 = new Vector3();
                for (var j = 0; j < StarDataLoader.star_data.Count; j++)
                {
                    // Debug.Log("Checking for star_data "+ star_data[j]["hip"]);
                    StarData star_val = StarDataLoader.star_data[j];
                    if(star_val.hip == "")
                    {
                        continue;
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i]))
                    {
                        //point1 = new Vector3(point1_x*StarDataLoader.scale, point1_y*StarDataLoader.scale, point1_z*StarDataLoader.scale);
                        point1 = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                        continue;
                    
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i+1]))
                    {
                        point2 = new Vector3(star_val.x0, star_val.y0, star_val.z0);    
                        continue;
                    }
                    if(point1 != new Vector3() && point2 != new Vector3())
                    {
                        break;
                    }
                
                } 
                if (point1 != new Vector3() && point2 != new Vector3())
                {
                    string lname = hip_id[i] +"-"+ hip_id[i+1];
                    GameObject line = new GameObject(lname);
                    line.transform.parent = constellation.transform;
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    lineRenderer.receiveShadows = false;
                    lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.03f;
                    lineRenderer.endWidth = 0.03f;
                    lineRenderer.SetPosition(0, point1);
                    lineRenderer.SetPosition(1, point2);
                    line_count++;
                }  
            } 
            
        }
        indian_constellation.SetActive(false);
    }
    void showBoorongConstellations()
    {
        var lines = boorong_constellation_datafile.text.Split('\n');
        List<string> temp_constellation_lines = new List<string>();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i] != "")
            {
                temp_constellation_lines.Add(lines[i]);
            }
        }
        
        int line_count = 0;
        for (var x = 0; x < temp_constellation_lines.Count; x++)
        {
            var hip_id = temp_constellation_lines[x].Split(' ');
            GameObject constellation = new GameObject(hip_id[0]);
            constellation.transform.parent = boorong_constellation.transform;
            for (var i = 2; i < hip_id.Length-1; i+=2)
            {
                Vector3 point1 = new Vector3();
                Vector3 point2 = new Vector3();
                for (var j = 0; j < StarDataLoader.star_data.Count; j++)
                {
                    StarData star_val = StarDataLoader.star_data[j];
                    if(star_val.hip == "")
                    {
                        continue;
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i]))
                    {
                        point1 = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                        continue;
                    
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i+1]))
                    {
                        point2 = new Vector3(star_val.x0, star_val.y0, star_val.z0);   
                        continue;
                    }
                    if(point1 != new Vector3() && point2 != new Vector3())
                    {
                        break;
                    }
                
                } 
                if (point1 != new Vector3() && point2 != new Vector3())
                {
                    //Debug.Log("Attempting to draw line between " + point1 + " and " + point2);
                    string lname = hip_id[i] +"-"+ hip_id[i+1];
                    GameObject line = new GameObject(lname);
                    line.transform.parent = constellation.transform;
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    lineRenderer.receiveShadows = false;
                    lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.material.color = Color.white;
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.02f;
                    lineRenderer.endWidth = 0.02f;
                    lineRenderer.SetPosition(0, point1);
                    lineRenderer.SetPosition(1, point2);
                    line_count++;
                }  
            } 
            
        }
        boorong_constellation.SetActive(false);
    }
    void showRomanianConstellations()
    {
        var lines = romanian_constellation_datafile.text.Split('\n');
        List<string> temp_constellation_lines = new List<string>();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i] != "")
            {
                temp_constellation_lines.Add(lines[i]);
            }
        }
        
        int line_count = 0;
        for (var x = 0; x < temp_constellation_lines.Count; x++)
        {
            var hip_id = temp_constellation_lines[x].Split(' ');
            GameObject constellation = new GameObject(hip_id[0]);
            constellation.transform.parent = romanian_constellation.transform;
            for (var i = 2; i < hip_id.Length-1; i+=2)
            {
                Vector3 point1 = new Vector3();
                Vector3 point2 = new Vector3();
                for (var j = 0; j < StarDataLoader.star_data.Count; j++)
                {
            
                    StarData star_val = StarDataLoader.star_data[j];
                    if(star_val.hip == "")
                    {
                        continue;
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i]))
                    {
                        //point1 = new Vector3(point1_x*StarDataLoader.scale, point1_y*StarDataLoader.scale, point1_z*StarDataLoader.scale);
                        point1 = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                        continue;
                    
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i+1]))
                    {
                        point2 = new Vector3(star_val.x0, star_val.y0, star_val.z0);    
                        continue;
                    }
                    if(point1 != new Vector3() && point2 != new Vector3())
                    {
                        break;
                    }
                
                } 
                if (point1 != new Vector3() && point2 != new Vector3())
                {
                    //Debug.Log("Attempting to draw line between " + point1 + " and " + point2);
                    string lname = hip_id[i] +"-"+ hip_id[i+1];
                    GameObject line = new GameObject(lname);
                    line.transform.parent = constellation.transform;
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    lineRenderer.receiveShadows = false;
                    lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.03f;
                    lineRenderer.endWidth = 0.03f;
                    lineRenderer.SetPosition(0, point1);
                    lineRenderer.SetPosition(1, point2);
                    line_count++;
                }  
            } 
            
        }
        romanian_constellation.SetActive(false);
    }

    void showEgyptianConstellations()
    {
        var lines = egyptian_constellation_datafile.text.Split('\n');
        List<string> temp_constellation_lines = new List<string>();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i] != "")
            {
                temp_constellation_lines.Add(lines[i]);
            }
        }
        
        int line_count = 0;
        for (var x = 0; x < temp_constellation_lines.Count; x++)
        {
            var hip_id = temp_constellation_lines[x].Split(' ');
            GameObject constellation = new GameObject(hip_id[0]);
            constellation.transform.parent = egyptian_constellation.transform;
            for (var i = 2; i < hip_id.Length-1; i+=2)
            {
                Vector3 point1 = new Vector3();
                Vector3 point2 = new Vector3();
                for (var j = 0; j < StarDataLoader.star_data.Count; j++)
                {
                    // Debug.Log("Checking for star_data "+ star_data[j]["hip"]);
                    StarData star_val = StarDataLoader.star_data[j];
                    if(star_val.hip == "")
                    {
                        continue;
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i]))
                    {
                        //point1 = new Vector3(point1_x*StarDataLoader.scale, point1_y*StarDataLoader.scale, point1_z*StarDataLoader.scale);
                        point1 = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                        continue;
                    
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i+1]))
                    {
                        point2 = new Vector3(star_val.x0, star_val.y0, star_val.z0);    
                        continue;
                    }
                    if(point1 != new Vector3() && point2 != new Vector3())
                    {
                        break;
                    }
                
                } 
                if (point1 != new Vector3() && point2 != new Vector3())
                {
                    //Debug.Log("Attempting to draw line between " + point1 + " and " + point2);
                    string lname = hip_id[i] +"-"+ hip_id[i+1];
                    GameObject line = new GameObject(lname);
                    line.transform.parent = constellation.transform;
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    lineRenderer.receiveShadows = false;
                    lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.03f;
                    lineRenderer.endWidth = 0.03f;
                    lineRenderer.SetPosition(0, point1);
                    lineRenderer.SetPosition(1, point2);
                    line_count++;
                }  
            } 
            
        }
        egyptian_constellation.SetActive(false);
    }
    void showNorseConstellations()
    {
        var lines = norse_constellation_datafile.text.Split('\n');
        List<string> temp_constellation_lines = new List<string>();
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i] != "")
            {
                temp_constellation_lines.Add(lines[i]);
            }
        }
        
        int line_count = 0;
        for (var x = 0; x < temp_constellation_lines.Count; x++)
        {
            var hip_id = temp_constellation_lines[x].Split(' ');
            GameObject constellation = new GameObject(hip_id[0]);
            constellation.transform.parent = norse_constellation.transform;
            for (var i = 3; i < hip_id.Length-1; i+=2)
            {
                Vector3 point1 = new Vector3();
                Vector3 point2 = new Vector3();
                for (var j = 0; j < StarDataLoader.star_data.Count; j++)
                {
                    // Debug.Log("Checking for star_data "+ star_data[j]["hip"]);
                    StarData star_val = StarDataLoader.star_data[j];
                    if(star_val.hip == "")
                    {
                        continue;
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i]))
                    {
                        //point1 = new Vector3(point1_x*StarDataLoader.scale, point1_y*StarDataLoader.scale, point1_z*StarDataLoader.scale);
                        point1 = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                        continue;
                    
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i+1]))
                    {
                        point2 = new Vector3(star_val.x0, star_val.y0, star_val.z0);    
                        continue;
                    }
                    if(point1 != new Vector3() && point2 != new Vector3())
                    {
                        break;
                    }
                
                } 
                if (point1 != new Vector3() && point2 != new Vector3())
                {
                    //Debug.Log("Attempting to draw line between " + point1 + " and " + point2);
                    string lname = hip_id[i] +"-"+ hip_id[i+1];
                    GameObject line = new GameObject(lname);
                    line.transform.parent = constellation.transform;
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    lineRenderer.receiveShadows = false;
                    lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.03f;
                    lineRenderer.endWidth = 0.03f;
                    lineRenderer.SetPosition(0, point1);
                    lineRenderer.SetPosition(1, point2);
                    line_count++;
                }  
            } 
            
        }
        // Debug.Log("Total Lines for Norse:"+line_count);
        norse_constellation.SetActive(false);
    }
    
}

