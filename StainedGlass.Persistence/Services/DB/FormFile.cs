using Microsoft.AspNetCore.Http;

namespace StainedGlass.Persistence.Services.DB;

public class FormFile : IFormFile
{
    private readonly byte[] _content;

    public FormFile(byte[] content, string fileName, string contentType)
    {
        _content = content; 
        FileName = fileName; 
        Name = fileName; 
        ContentType = contentType;
        ContentDisposition = "inline; filename" + fileName;
        Headers = new CustomHeaderDictionary();
        Headers.Add("Content-Disposition", ContentDisposition);
    } 
    public string ContentType { get; } 
    public string ContentDisposition { get; } 
    public IHeaderDictionary Headers { get; } 
    public long Length => _content.Length; 
    public string Name { get; } 
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