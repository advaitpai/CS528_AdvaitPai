using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExoplanetChange : StarDataLoader
{
    public TextAsset exoplanet_datafile;
    public bool exoplanet_toggle;

    public static List<ExoplanetData> exoplanet_data;

    // Start is called before the first frame update
    void Start()
    {
        exoplanet_toggle = false;
        exoplanet_data = new List<ExoplanetData>();
        var lines = exoplanet_datafile.text.Split('\n');
        for (var i =1; i<lines.Length;i++)
        {
            var values = lines[i].Split(',');
            if (values.Length == 2)
            {
                ExoplanetData exoplanet_val = new ExoplanetData();
                exoplanet_val.hip = values[0];
                exoplanet_val.pl_pnum = int.Parse(values[1]);
                exoplanet_data.Add(exoplanet_val);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void toggle_pressed()
    {
        exoplanet_toggle = !exoplanet_toggle;
        if (exoplanet_toggle)
        {
            change_colour_scheme();
        }
    }
    void change_colour_scheme()
    {
        
    }
    Color exoColour(int val)
    {
        if (val == 1)
        {
            return new Color(255f/255f,127f/255f,127f/255f); // Light Red 
        }
        else if (val == 2)
        {
            return new Color(173f/255f,216f/255f,230f/255f); // Light Blue 
        }
        else if (val == 3)
        {
            return new Color(144f/255f,238f/255f,144f/255f); // Light Green
        }
        else if (val == 4)
        {
            return new Color(177f/255f,156f/255f,217f/255f); // Light Purple
        }
        else if (val == 5)
        {
            return new Color(255f/255f,255f/255f,191f/255f); // Light Yellow 
        }

        else if (val == 6)
        {
            return new Color(155f/255f,103f/255f,60f/255f); // Brown 
        }
        else
        {
            return Color.grey;
        }

    }
}
