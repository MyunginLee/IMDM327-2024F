using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string pathtofile = "data.csv";
        //Stream Reader: https://learn.microsoft.com/en-us/dotnet/api/system.io.streamreader?view=net-8.0
        StreamReader strReader = new StreamReader(pathtofile);
        bool endOfFile = false; 
        //float [,] data;
        int i = 0;
        while (!endOfFile)
        {
            string data_String = strReader.ReadLine(); // read line by line until the end of file.
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(',');
            // Print values
            Debug.Log(data_values[0].ToString() + " " + data_values[1].ToString());
            i++;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

}

