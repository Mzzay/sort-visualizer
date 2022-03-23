using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace sort_visualizer.Utils;

public class IFile : MainWindow
{
    public string fileName;
    public string fileContent;
    public int[] arrayValue { get; set; }
    public char separator;

    public IFile(string path, char separator)
    {
        StreamReader str = new StreamReader(path);
        this.fileName = Path.GetFileName(path);
        this.fileContent = str.ReadToEnd();
        this.separator = separator;
    }
    
    // parse content in array
    public void ParseContent()
    {
        string[] content = this.fileContent.Split(this.separator);
        int[] contentValue = new int[content.Length];
        for (int i = 0; i < content.Length; i++)
            contentValue[i] = Convert.ToInt32(content[i]);

        this.arrayValue = contentValue;
        // PrintArray();
    }
    
    // print array
    public void PrintArray()
    {
        string res = "";
        for (int i = 0; i < this.arrayValue.Length; i++)
            res += this.arrayValue[i].ToString();
        
        Console.WriteLine(res);
    }
}
