namespace nJinja
{
    using System.IO;
    using Microsoft.Scripting;

    /// <summary>
    /// Implement the stream content provider so that I can pull and execute
    /// Python scripts that have been embedded in the assembly. 
    /// 
    /// I ripped this code off from:
    /// https://github.com/devhawk/pygments.wlwriter
    /// 
    /// It's simple but credit where it's due after all.
    /// </summary>
    public class BasicStreamContentProvider : StreamContentProvider
    {
        Stream _stream;

        public BasicStreamContentProvider(Stream stream)
        {
            _stream = stream;
        }

        public override Stream GetStream()
        {
            return _stream;
        }
    }
}
