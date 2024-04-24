using System.Text;
using System.Text.Json;

namespace Dictionary;

public class FileManager
{
    private string _pathToFile;
    private FileStream _file;
    public FileManager(string path)
    {
        _pathToFile = path;
        _file = new FileStream(path, FileMode.Open);
    }

    public void OpenFile(string path)
    {
        
    }
    public Dictionary<string, string> ReadFile()
    {
        Dictionary<string, string>?list = JsonSerializer.Deserialize<Dictionary<string, string>>(_file);
        return list;
    }
    
    public Dictionary<string, List<string>> ReadFileWithDefinitions()
    {
        Dictionary<string, List<string>>?list = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(_file);
        return list;
    }

    public void CloseFile()
    {
        _file.Close();
    }
}