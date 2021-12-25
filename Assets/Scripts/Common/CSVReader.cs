using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVReader
{
    // CSVファイルからデータを読み取る
    public static List<string[]> LoadScenario(TextAsset csvFile)
    {
        List<string[]> _csvDatas = new List<string[]>();
        StringReader _reader = new StringReader(csvFile.text);

        while(_reader.Peek() != -1)
        {
            string line = _reader.ReadLine();
            _csvDatas.Add(line.Split(','));
        }

        return _csvDatas;
    }
}
