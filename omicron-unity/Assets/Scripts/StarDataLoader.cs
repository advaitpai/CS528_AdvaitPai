using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class StarDataLoader : MonoBehaviour
{
    // List to store the data
    //public List<Dictionary<string, string>> star_data;
    public static List<StarData> star_data;
    public static List<GameObject> stars_objects;

    public static TextAsset star_datafile;

    public static bool star_loaded;

    public GameObject person_camera;

    public GameObject person_orient;

    public static Vector3 last_render_pos;

    public static float threshold;

    public TMP_Text sol_dist;

    public static float scale;

    public static float speed;

    public static TextAsset exoplanet_datafile;

    public static bool stars_motion;

    public static List<StarData> init_star_data;
    public static float years;

    public static string constellation_type;

    public static Vector3 first_render_pos;
    public static Quaternion first_render_rot;

    public GameObject prefab;
    public static bool reset_world;

    // Start is called before the first frame update
    void Start()
    {
        // Read CSV file and store it in a list
        // Debug.Log("Loading Star Data");
        reset_world = false;
        star_datafile = Resources.Load<TextAsset>("athyg_31_reduced_m10_cleaned_subset");
        exoplanet_datafile = Resources.Load<TextAsset>("exoplanet_cleaned");
        var lines = star_datafile.text.Split('\n');
        star_data = new List<StarData>();
        stars_objects = new List<GameObject>();
        stars_motion = false;
        scale = 1f;
        threshold = 30f;
        speed = 1f;
        constellation_type = "modern";
        //last_render_pos = person_camera.transform.position;
        last_render_pos = new Vector3(0,0,0);
        first_render_pos = new Vector3(0,0,0);
        first_render_rot = new Quaternion(0,0,0,0);
        for (var i = 1; i < lines.Length-1; i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 11 && values[0]!="")
            {
                StarData star_val = new StarData();
                star_val.id = i;
                star_val.hip = values[0].Substring(0,values[0].Length-2);
                star_val.dist = values[1];
                star_val.x0 = float.Parse(values[2])*0.3048f;
                star_val.y0 = float.Parse(values[3])*0.3048f;
                star_val.z0 = float.Parse(values[4])*0.3048f;
                star_val.absmag = float.Parse(values[5]);
                star_val.mag = values[6];
                star_val.vx = float.Parse(values[7])*0.3048f*1.02269e-6f*750000000;
                star_val.vy = float.Parse(values[8])*0.3048f*1.02269e-6f*750000000;
                star_val.vz = float.Parse(values[9])*0.3048f*1.02269e-6f*750000000;
                star_val.spect = values[10];
                star_val.visible = false; 
                star_val.pl_pnum = getPlNum(star_val.hip);
                star_data.Add(star_val);
                
                // Create a quad for each star
                //GameObject star = GameObject.CreatePrimitive(PrimitiveType.Quad);
                GameObject star = Instantiate(prefab);
                star.transform.position = new Vector3(star_val.x0, star_val.y0, star_val.z0);
                star.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                star.GetComponent<Renderer>().material.color = getColour(star_val.spect);
                //star.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
                stars_objects.Add(star);
            }
    
        }
        init_star_data = new List<StarData>();
        init_star_data = star_data;
        drawStars();
    }

    // Update is called once per frame
    void Update()
    {
        if(first_render_pos == new Vector3(0,0,0))
        {
            first_render_pos = person_camera.transform.position;
            first_render_rot = person_orient.transform.rotation;

        }
        if (calculate_distance(person_camera.transform.position,last_render_pos) > 7.5f)
        {
            last_render_pos = person_camera.transform.position;
            drawStars();
        }
        sol_dist.text = "Distance to Sol: "+((calculate_distance(person_camera.transform.position,new Vector3(0,1,0))/0.3048).ToString("F2"));

    }
    public Color getColour(string spect) // http://www.vendian.org/mncharity/dir3/starcolor/ Using the rgb values from here
    {
        if (spect == "O")
        {
            return new Color(155f/255f,176f/255f,255f/255f);
        }
        else if (spect == "B")
        {
            return new Color(170f/255f,191f/255f,255f/255f);
        }
        else if (spect == "A")
        {
            return new Color(202f/255f,215f/255f,255f/255f);
        }
        else if (spect == "F")
        {
            return new Color(248f/255f,247f/255f,255f/255f);
        }
        else if (spect == "G")
        {
            return new Color(255f/255f,244f/255f,234f/255f);
        }
        else if (spect == "K")
        {
            return new Color(255f/255f,210f/255f,161f/255f);
        }
        else if (spect == "M")
        {
            return new Color(255f/255f,204f/255f,111f/255f);
        }
        else
        {
            return Color.grey;
        }
    }
    float calculate_distance(Vector3 pos1,Vector3 pos2)
    { 
        return Vector3.Distance(pos1,pos2);
    }
    public void drawStars()
    {
        for (var i = 0; i < star_data.Count; i++)
        {
            StarData star_val = star_data[i];
            Vector3 star_loc = new Vector3(star_val.x0*scale, star_val.y0*scale, star_val.z0*scale);
            if (calculate_distance(last_render_pos,star_loc) < threshold)
            {
                star_val.visible = true;
                stars_objects[i].transform.position = star_loc;
                stars_objects[i].transform.localScale = new Vector3(0.3f*scale, 0.3f*scale, 0.3f*scale);
            }
            else
            {
                star_val.visible = false;
            }
            
            stars_objects[i].SetActive(star_val.visible);
        }
        star_loaded = true;
    }
    int getPlNum(string star_hip)
    {
        var lines = exoplanet_datafile.text.Split('\n');
        for (var i =1; i<lines.Length;i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 2)
            {
                string temp_hip = values[0];
                if (temp_hip == star_hip)
                {
                    return int.Parse(values[1]);
                }
            }
        }
        return 0;
    }

    
}
