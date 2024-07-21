namespace ConcurrentFileReading.Interfaces
{
    internal interface IDiskProcessor
    {
        int GetFileCharCount(string fileName, char chr);
        FileInfo[] GetDirecotryFiles(string dirPath);
    }
}
