using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader
{
    // Start is called before the first frame update
    public static List<string[]> LoadScenario(TextAsset csvFile)
    {
        List<string[]> csvDatas = new List<string[]>();
        StringReader reader = new StringReader(csvFile.text);

        while(reader.Peek() != -1)
        {
            string line = reader.ReadLine();
            csvDatas.Add(line.Split(','));
        }

        return csvDatas;
    }


}
