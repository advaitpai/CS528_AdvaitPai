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
}
