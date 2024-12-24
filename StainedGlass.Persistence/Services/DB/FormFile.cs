using Microsoft.AspNetCore.Http;

namespace StainedGlass.Persistence.Services.DB;

public class FormFile : IFormFile
{
    private readonly byte[] _content;

    public FormFile(byte[] content, string fileName, string contentType)
    {
        _content = content; 
        FileName = fileName; 
        ContentType = contentType;
    } 
    public string ContentType { get; } 
    public string ContentDisposition => throw new System.NotImplementedException(); 
    public IHeaderDictionary Headers => throw new System.NotImplementedException(); 
    public long Length => _content.Length; 
    public string Name => throw new System.NotImplementedException(); 
    public string FileName { get; }

    public void CopyTo(Stream target)
    {
        using (var stream = new MemoryStream(_content))
        {
            stream.CopyTo(target);
        }
    }

    public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        using (var stream = new MemoryStream(_content))
        {
            return stream.CopyToAsync(target, cancellationToken);
        }
    }

    public Stream OpenReadStream()
    {
        return new MemoryStream(_content);
    }
}