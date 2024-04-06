using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationDrawer : StarDataLoader
{
    
    public static TextAsset constellation_datafile;
    public static TextAsset indian_constellation_datafile;
    public static TextAsset romanian_constellation_datafile;
    public static TextAsset norse_constellation_datafile;
    public static TextAsset egyptian_constellation_datafile;
    public static TextAsset boorong_constellation_datafile;
    public static GameObject constellation_parent;
    public static GameObject indian_constellation;
    public static GameObject romanian_constellation;
    public static GameObject norse_constellation;
    public static GameObject egyptian_constellation;
    public static GameObject boorong_constellation;
    public static string current_constellation;
    public static Dictionary<string, Vector3> constellations_star_positions;
    
    public static bool additional_previous;

    // Start is called before the first frame update
    void Start()
    {
        constellation_parent = new GameObject("Constellations");
        indian_constellation = new GameObject("Indian Constellations");
        romanian_constellation = new GameObject("Romanian Constellations");
        norse_constellation = new GameObject("Norse Constellations");
        egyptian_constellation = new GameObject("Egyptian Constellations");
        boorong_constellation = new GameObject("Boorong Constellations");
        constellation_datafile = Resources.Load<TextAsset>("constellation_coord");
        indian_constellation_datafile = Resources.Load<TextAsset>("indian-constellationship");
        romanian_constellation_datafile = Resources.Load<TextAsset>("romanian-constellationship");
        norse_constellation_datafile = Resources.Load<TextAsset>("norse-constellationship");
        egyptian_constellation_datafile = Resources.Load<TextAsset>("egyptian-constellationship");
        boorong_constellation_datafile = Resources.Load<TextAsset>("boorong-constellationship");
        constellations_star_positions = new Dictionary<string, Vector3>();
        additional_previous = false;
        StarDataLoader.scale = 1f;
        StarDataLoader.speed = 1f;
        StarDataLoader.constellation_type = "modern";
        current_constellation = StarDataLoader.constellation_type;
        readConstellation(ref constellation_datafile,ref constellation_parent);
        readConstellation(ref indian_constellation_datafile,ref indian_constellation);
        readConstellation(ref romanian_constellation_datafile,ref romanian_constellation);
        readConstellation(ref norse_constellation_datafile,ref norse_constellation);
        readConstellation(ref egyptian_constellation_datafile,ref egyptian_constellation);
        readConstellation(ref boorong_constellation_datafile,ref boorong_constellation);
        indian_constellation.SetActive(false);
        romanian_constellation.SetActive(false);
        norse_constellation.SetActive(false);
        egyptian_constellation.SetActive(false);
        boorong_constellation.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(StarDataLoader.stars_motion)
        {
            moveStar();
            if(StarDataLoader.constellation_type == "modern")
            {
                updateConstellations(ref constellation_parent);
            }
            else if(StarDataLoader.constellation_type == "indian")
            {
                updateConstellations(ref indian_constellation);
            }
            else if(StarDataLoader.constellation_type == "boorong")
            {
                updateConstellations(ref boorong_constellation);
            }
            else if(StarDataLoader.constellation_type == "egyptian")
            {
                updateConstellations(ref egyptian_constellation);
            }
            else if(StarDataLoader.constellation_type == "romanian")
            {
                updateConstellations(ref romanian_constellation);
            }
            else if(StarDataLoader.constellation_type == "norse")
            {
                updateConstellations(ref norse_constellation);
            }
            StarDataLoader.years += 75000000*Time.deltaTime*StarDataLoader.speed;
        }
        else if (!StarDataLoader.stars_motion && StarDataLoader.scale_changed)
        {
            updateConstellationStarPosition();
            if(StarDataLoader.constellation_type == "modern")
            {
                updateConstellations(ref constellation_parent);
            }
            else if(StarDataLoader.constellation_type == "indian")
            {
                updateConstellations(ref indian_constellation);
            }
            else if(StarDataLoader.constellation_type == "boorong")
            {
                updateConstellations(ref boorong_constellation);
            }
            else if(StarDataLoader.constellation_type == "egyptian")
            {
                updateConstellations(ref egyptian_constellation);
            }
            else if(StarDataLoader.constellation_type == "romanian")
            {
                updateConstellations(ref romanian_constellation);
            }
            else if(StarDataLoader.constellation_type == "norse")
            {
                updateConstellations(ref norse_constellation);
            }
            StarDataLoader.scale_changed = false;
        }
        if(StarDataLoader.constellation_type != current_constellation)
        {
            
            current_constellation = StarDataLoader.constellation_type;
            if(current_constellation=="modern")
            {
                constellation_parent.SetActive(true);
                indian_constellation.SetActive(false);
                boorong_constellation.SetActive(false);
                egyptian_constellation.SetActive(false);
                romanian_constellation.SetActive(false);
                norse_constellation.SetActive(false);
            }
            else if(current_constellation=="indian")
            {
                indian_constellation.SetActive(true);
                constellation_parent.SetActive(false);
                boorong_constellation.SetActive(false);
                egyptian_constellation.SetActive(false);
                romanian_constellation.SetActive(false);
                norse_constellation.SetActive(false);
            }
            else if (current_constellation == "boorong")
            {
                boorong_constellation.SetActive(true);
                indian_constellation.SetActive(false);
                constellation_parent.SetActive(false);
                egyptian_constellation.SetActive(false);
                romanian_constellation.SetActive(false);
                norse_constellation.SetActive(false);
            }
            else if (current_constellation == "egyptian")
            {
                egyptian_constellation.SetActive(true);
                indian_constellation.SetActive(false);
                boorong_constellation.SetActive(false);
                constellation_parent.SetActive(false);
                romanian_constellation.SetActive(false);
                norse_constellation.SetActive(false);
            }
            else if (current_constellation == "romanian")
            {
                romanian_constellation.SetActive(true);
                indian_constellation.SetActive(false);
                boorong_constellation.SetActive(false);
                egyptian_constellation.SetActive(false);
                constellation_parent.SetActive(false);
                norse_constellation.SetActive(false);
            }
            else if (current_constellation == "norse")
            {
                norse_constellation.SetActive(true);
                indian_constellation.SetActive(false);
                boorong_constellation.SetActive(false);
                egyptian_constellation.SetActive(false);
                romanian_constellation.SetActive(false);
                constellation_parent.SetActive(false);
            }
            else if (current_constellation == "none")
            {
                indian_constellation.SetActive(false);
                boorong_constellation.SetActive(false);
                egyptian_constellation.SetActive(false);
                romanian_constellation.SetActive(false);
                constellation_parent.SetActive(false);
                norse_constellation.SetActive(false);
            }
        }
        if(StarDataLoader.reset_world)
        {
            StarDataLoader.star_data = StarDataLoader.init_star_data;
            drawStars();
            updateConstellationStarPosition();
            if(StarDataLoader.constellation_type == "modern")
            {
                updateConstellations(ref constellation_parent);
            }
            else if(StarDataLoader.constellation_type == "indian")
            {
                updateConstellations(ref indian_constellation);
            }
            else if(StarDataLoader.constellation_type == "boorong")
            {
                updateConstellations(ref boorong_constellation);
            }
            else if(StarDataLoader.constellation_type == "egyptian")
            {
                updateConstellations(ref egyptian_constellation);
            }
            else if(StarDataLoader.constellation_type == "romanian")
            {
                updateConstellations(ref romanian_constellation);
            }
            else if(StarDataLoader.constellation_type == "norse")
            {
                updateConstellations(ref norse_constellation);
            }
            StarDataLoader.years += 0;
            StarDataLoader.reset_world = false;
        }
        if(additional_previous!=StarDataLoader.additional_info)
        {
            if(StarDataLoader.additional_info)
            {
                showAdditionalInfo();
            }
            else
            {
                hideAdditionalInfo();
            }
            additional_previous = StarDataLoader.additional_info;
        }
    }
    void readConstellation(ref TextAsset datafile, ref GameObject constellationParent)
    {
        var lines = datafile.text.Split('\n');
        List<string> constellation_lines = new List<string>();
        for (var i = 0; i < lines.Length; i++)
        {
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
            constellation.transform.parent = constellationParent.transform;
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
                    if (star_val.hip == hip_id[i])
                    {
                        point1 = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                        continue;
                    
                    }
                    if (star_val.hip == hip_id[i+1])
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
                    if (hip_id[0] == "Ori")
                    {
                        
                    }
                    string lname = hip_id[i] +"-"+ hip_id[i+1];
                    GameObject line = new GameObject(lname);
                    line.transform.parent = constellation.transform;
                    LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
                    lineRenderer.material.color = Color.white;
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.025f;
                    lineRenderer.endWidth = 0.025f;
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
                // StarDataLoader.stars_objects[i].transform.localScale = new Vector3(0.1f*StarDataLoader.scale,0.1f*StarDataLoader.scale,0.1f*StarDataLoader.scale);
                StarDataLoader.stars_objects[i].transform.position = new Vector3(star_val.x0*StarDataLoader.scale, star_val.y0*StarDataLoader.scale, star_val.z0*StarDataLoader.scale);
            }
        }

    }
    void updateConstellations(ref GameObject constellation_current_parent)
    {
        
        for (var i = 0; i < constellation_current_parent.transform.childCount; i++)
        {
            GameObject constellation = constellation_current_parent.transform.GetChild(i).gameObject;
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
    void updateConstellationStarPosition()
    {
        for (var i = 0; i < StarDataLoader.star_data.Count; i++)
        {
            StarData star_val = StarDataLoader.star_data[i];
            if(constellations_star_positions.ContainsKey(star_val.hip))
            {
                constellations_star_positions[star_val.hip] = new Vector3(star_val.x0*StarDataLoader.scale, star_val.y0*StarDataLoader.scale, star_val.z0*StarDataLoader.scale);
            }
        }
    }
    void showAdditionalInfo()
    {
        indian_constellation.SetActive(false);
        romanian_constellation.SetActive(false);
        norse_constellation.SetActive(false);
        egyptian_constellation.SetActive(false);
        boorong_constellation.SetActive(false);
        constellation_parent.SetActive(true);
        for (var i = 0; i < constellation_parent.transform.childCount; i++)
        {
            if(constellation_parent.transform.GetChild(i).gameObject.name == "Mon")
            {
                constellation_parent.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                constellation_parent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    void hideAdditionalInfo()
    {
        for (var i = 0; i < constellation_parent.transform.childCount; i++)
        {
            constellation_parent.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    
}

