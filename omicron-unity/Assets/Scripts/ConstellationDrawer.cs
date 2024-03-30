using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstellationDrawer : StarDataLoader
{
    public static bool constellations_loaded;
    public TextAsset constellation_datafile;
    // Start is called before the first frame update
    void Start()
    {
        constellations_loaded = false;
        //InvokeRepeating("renderStars", 3.0f, 1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (star_loaded && !constellations_loaded)
        {
            readConstellation();
            constellations_loaded = true;
        }
        if (star_loaded && constellations_loaded)
        {
            moveStar();
        }
    }
    void renderStars()
    {
            moveStar();
            //readConstellation();
    }
    void drawConstellation(string constellation)
    {
        // Split the constellation string
        //Debug.Log(constellation);
        var hip_id = constellation.Split(' ');
        for (var i = 3; i < hip_id.Length-1; i+=2)
        {
            Vector3 point1 = new Vector3();
            Vector3 point2 = new Vector3();
            // Debug.Log("Checking for hip1 "+ float.Parse(hip_id[i]));
            // Debug.Log("Checking for hip2 "+ float.Parse(hip_id[i+1]));
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
                
                }
                if (float.Parse(star_val.hip) == float.Parse(hip_id[i+1]))
                {
                    float point2_x = star_val.x0;
                    float point2_y = star_val.y0;
                    float point2_z = star_val.z0;
                    point2 = new Vector3(point2_x, point2_y, point2_z);    
                }
                if(point1 != new Vector3() && point2 != new Vector3())
                {
                    break;
                }

                
            } 
            if (point1 != new Vector3() && point2 != new Vector3())
            {
                //Debug.Log("Attempting to draw line between " + point1 + " and " + point2);
                GameObject line = new GameObject();
                line.transform.parent = transform;
                LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.positionCount = 2;
                lineRenderer.startWidth = 0.01f;
                lineRenderer.endWidth = 0.01f;
                lineRenderer.SetPosition(0, point1);
                lineRenderer.SetPosition(1, point2);
            }  
            
        }
    }
    void readConstellation()
    {
        //TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Constellations/constellation_coord.txt") as TextAsset;
        var lines = constellation_datafile.text.Split('\n');
        //Debug.Log("Reading constellation data"+ lines.Length);
        int count = 0;
        for (var i = 0; i < lines.Length; i++)
        {
            if (i > 88)
            {
                //Debug.Log("Constellations drawn: "+count);
                break;
            }
            if (lines[i] != "")
            {
                //Debug.Log("Drawing constellation"+(i+1));
                count += 1;
                drawConstellation(lines[i]);
            }
        }
    } 
    void moveStar()
    {
        //Debug.Log("Moving stars"+Time.deltaTime);
        //Debug.Log(star_data.Count);
        // for (var i = 0; i<StarDataLoader.star_data.Count;i++)
        // {
        //     GameObject star = transform.GetChild(i).gameObject;
        //     StarData star_val = StarDataLoader.star_data[i];
        //     star_val.x0 = star_val.x0 + star_val.vx;
        //     star_val.y0 = star_val.y0 + star_val.vy;
        //     star_val.z0 = star_val.z0 + star_val.vz;
        //     StarDataLoader.star_data[i] = star_val;
        //     star.transform.position = new Vector3(star_val.x0, star_val.y0, star_val.z0);
        // }
        Debug.Log("Moving stars");
        for (var i = 0; i < StarDataLoader.stars_objects.Count; i++)
        {

            Renderer renderer = StarDataLoader.stars_objects[i].GetComponent<Renderer>();
            StarData star_val = StarDataLoader.star_data[i];
            star_val.x0 = star_val.x0 + (star_val.vx*Time.deltaTime);
            star_val.y0 = star_val.y0 + (star_val.vy*Time.deltaTime);
            star_val.z0 = star_val.z0 + (star_val.vz*Time.deltaTime);
            star_data[i] = star_val;
            if(renderer.isVisible)
            {    
                StarDataLoader.stars_objects[i].transform.position = new Vector3(star_val.x0, star_val.y0, star_val.z0);
            }
        }
    }
}
