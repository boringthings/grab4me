internal record FilePath(string Path)
{
    public static implicit operator string(FilePath filePath) => filePath.Path;

    public static implicit operator FilePath(string path) => new(path);

    public override string ToString() => Path;
}