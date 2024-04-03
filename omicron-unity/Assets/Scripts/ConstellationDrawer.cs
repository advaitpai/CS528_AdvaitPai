using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationDrawer : StarDataLoader
{
    //public static bool constellations_loaded;
    public TextAsset constellation_datafile;

    public static List<string> constellation_lines;

    public static GameObject constellation_parent;
    public static List<LineVector> hip_linesData;

    // Start is called before the first frame update
    void Start()
    {
        constellation_lines = new List<string>();
        constellation_parent = new GameObject("Constellations");
        readConstellation();
        //InvokeRepeating("updateConstellations", 20.0f, 20.0f);
        //drawConstellations();
        // Read CSV file and store it in a list
        
    }

    // Update is called once per frame
    void Update()
    {
        moveStar();
    }
    
    void moveConstellations()
    {
        
    }
    void readConstellation()
    {
        //TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Constellations/constellation_coord.txt") as TextAsset;
        var lines = constellation_datafile.text.Split('\n');
        //Debug.Log("Reading constellation data"+ lines.Length);
        //int count = 0;
        for (var i = 0; i < lines.Length-78; i++)
        {
            if (i > 88)
            {
                //Debug.Log("Constellations drawn: "+count);
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
                    // Debug.Log("Checking for star_data "+ star_data[j]["hip"]);
                    StarData star_val = StarDataLoader.star_data[j];
                    if(star_val.hip == "")
                    {
                        continue;
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i]))
                    {
                        float point1_x = star_val.x0;
                        float point1_y = star_val.y0;
                        float point1_z = star_val.z0;
                        point1 = new Vector3(point1_x, point1_y, point1_z);
                        continue;
                    
                    }
                    if (float.Parse(star_val.hip) == float.Parse(hip_id[i+1]))
                    {
                        float point2_x = star_val.x0;
                        float point2_y = star_val.y0;
                        float point2_z = star_val.z0;
                        point2 = new Vector3(point2_x, point2_y, point2_z);    
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
                    lineRenderer.startColor = Color.white;
                    lineRenderer.endColor = Color.white;
                    //lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.positionCount = 2;
                    lineRenderer.startWidth = 0.03f;
                    lineRenderer.endWidth = 0.03f;
                    lineRenderer.useWorldSpace = true;
                    lineRenderer.SetPosition(0, point1);
                    lineRenderer.SetPosition(1, point2);
                    line_count++;
                }  
            } 
            
        }
        Debug.Log("Total Lines:"+line_count);
    } 
    void moveStar()
    {
        Debug.Log("Moving stars and constellations");
        hip_linesData = new List<LineVector>();
        for (var i = 0; i < StarDataLoader.stars_objects.Count; i++)
        {

            LineVector lineData = new LineVector();
            StarData star_val = StarDataLoader.star_data[i];
            star_val.x0 = star_val.x0 + (star_val.vx*Time.deltaTime*StarDataLoader.speed);
            star_val.y0 = star_val.y0 + (star_val.vy*Time.deltaTime*StarDataLoader.speed);
            star_val.z0 = star_val.z0 + (star_val.vz*Time.deltaTime*StarDataLoader.speed);
            star_data[i] = star_val;
            lineData.line_hip = star_val.hip;
            lineData.point_pos = new Vector3(star_val.x0, star_val.y0, star_val.z0);
            hip_linesData.Add(lineData);
            if(star_val.visible)
            {    
                StarDataLoader.stars_objects[i].transform.position = new Vector3(star_val.x0, star_val.y0, star_val.z0);
            }
        }
        // for (var i = 0; i < constellation_parent.transform.childCount; i++)
        // {
        //     GameObject constellation = constellation_parent.transform.GetChild(i).gameObject;
        //     for (var j = 0; j < constellation.transform.childCount; j++)
        //     {
        //         GameObject line = constellation.transform.GetChild(j).gameObject;
        //         LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        //         var hip_id = line.name.Split('-');
        //         for (var k = 0; k < hip_linesData.Count; k++)
        //         {
        //             LineVector lineData = hip_linesData[k];
        //             if (lineData.line_hip == hip_id[0])
        //             {
        //                 lineRenderer.SetPosition(0, lineData.point_pos);
        //                 continue;
        //             }
        //             if (lineData.line_hip == hip_id[1])
        //             {
        //                 lineRenderer.SetPosition(1, lineData.point_pos);
        //                 continue;
        //             }
        //         }
        //     }
        // }
        // for (var i = 0; i < constellation_parent.transform.childCount; i++)
        // {
        //     GameObject constellation = constellation_parent.transform.GetChild(i).gameObject;
        //     for (var j = 0; j < constellation.transform.childCount; j++)
        //     {
        //         GameObject line = constellation.transform.GetChild(j).gameObject;
        //         LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        //         var hip_id = line.name.Split('-');
        //         lineRenderer.SetPosition(0,getStarLocation(hip_id[0]));
        //         lineRenderer.SetPosition(1,getStarLocation(hip_id[1]));
        //     }
        // }

    }
    Vector3 getStarLocation(string hip_id)
    {
        for (var i = 0; i < StarDataLoader.star_data.Count; i++)
        {
            StarData star_val = StarDataLoader.star_data[i];
            if (star_val.hip == hip_id)
            {
                return new Vector3(star_val.x0, star_val.y0, star_val.z0);
            }
        }
        return new Vector3(0,0,0);
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
                for (var k = 0; k < hip_linesData.Count; k++)
                {
                    LineVector lineData = hip_linesData[k];
                    if (lineData.line_hip == hip_id[0])
                    {
                        lineRenderer.SetPosition(0, lineData.point_pos);
                        continue;
                    }
                    if (lineData.line_hip == hip_id[1])
                    {
                        lineRenderer.SetPosition(1, lineData.point_pos);
                        continue;
                    }
                }
            }
        }
    }
}

