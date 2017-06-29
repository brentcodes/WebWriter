using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace WebWriter
{
    public class WebWriter : IWebWriter
    {
        protected bool _isStreamCreatedInternally = false;
        protected bool _isWriterCreatedInternally = false;
        protected TextWriter _writer;
        protected int _indent = 0;
        protected Stream _stream;

        public WebWriter(TextWriter writer)
        {
            _writer = writer;
            _isWriterCreatedInternally = false;
        }

        public WebWriter(Stream stream)
        {
            _isWriterCreatedInternally = true;
            _writer = new StreamWriter(stream);
        }

        public WebWriter(string filePath)
        {
            _stream = File.Create(filePath);
            _isWriterCreatedInternally = true;
            _writer = new StreamWriter(_stream);
        }

        string GetIndent()
        {
            string indent = "";
            for (int i = 0; i < _indent; i++)
            {
                indent += "    ";
            }
            return indent;
        }

        public Closer OpenTag(string tag)
        {
            return OpenTag(tag, null);
        }

        public Closer OpenTag(string tag, string cssClass)
        {
            object attributes = null;
            if (cssClass != null)
                attributes = new { @class = cssClass };
            return OpenTag(tag, attributes);
        }

        public Closer OpenTag(string tag, object attributes)
        {
            _writer.Write(GetIndent() + "<" + tag);
            if (attributes != null)
            {
                Type t = attributes.GetType();
                foreach (var item in t.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                {
                    string attrName = item.Name.Replace('_', '-').ToLower();
                    if (attrName == "cssclass")
                        attrName = "class";
                    string attributeValue = HttpUtility.HtmlAttributeEncode((item.GetValue(attributes) ?? "").ToString());
                    _writer.Write(" " + attrName + "=\"" + attributeValue + "\"");
                }
            }
            _writer.Write(">" + _writer.NewLine);
            ++_indent;
            return new Closer(() => CloseTag(tag));
        }

        public void BreakLine()
        {
            _writer.Write(GetIndent() + "<br/>" + _writer.NewLine);
        }

        public void Nbsp()
        {
            Nbsp(1);
        }

        public void Nbsp(int spaces)
        {
            while (spaces--> 0)
                _writer.Write("&nbsp;");
        }

        public void WriteText(string text)
        {
            _writer.Write(GetIndent() + HttpUtility.HtmlEncode(text) + _writer.NewLine);
        }

        public void UnencodedWrite(string text)
        {
            _writer.Write(GetIndent() + text + _writer.NewLine);
        }

        public void CloseTag(string name)
        {
            --_indent;
            _writer.WriteLine(GetIndent() + "</" + name + ">");            
        }

        public void WriteForEach<T>(string tag, IEnumerable<T> collection, Func<T, string> text)
        {
            WriteForEach<T>(tag, null, collection, text);
        }

        public void WriteForEach<T>(string tag, string cssClass, IEnumerable<T> collection, Func<T, string> text)
        {
            object attributes = null;
            if (cssClass != null)
                attributes = new { @class = cssClass };
            WriteForEach(tag, attributes, collection, text);
        }

        public void WriteForEach<T>(string tag, object attributes, IEnumerable<T> collection, Func<T, string> text)
        {
            foreach (T item in collection)
            {
                WriteTag(tag,text(item), attributes);
            }
        }

        public void ForEach<T>(IEnumerable<T> collection, Action<T> callBack)
        {
            ForEach<T>(null, collection, callBack);
        }
        public void ForEach<T>(string containerTag, IEnumerable<T> collection, Action<T> callBack)
        {
            ForEach<T>(containerTag, null, collection, callBack);
        }

        public void ForEach<T>(string containerTag, string containerClass, IEnumerable<T> collection, Action<T> callBack)
        {
            object attributes = null;
            if (containerClass != null)
                attributes = new { @class = containerClass };
            ForEach<T>(containerTag, attributes, collection, callBack);
        }

        public void ForEach<T>(string containerTag, object containerAttributes, IEnumerable<T> collection, Action<T> callBack)
        {
            Closer closer = null;
            if (containerTag != null)
                closer =OpenTag(containerTag, containerAttributes);
            foreach (T item in collection)
                callBack(item);
            if (closer != null)
                closer.CloseTag();
        }

        public void TableRow(params object[] data)
        {
            this.ForEach("tr", data, (x) =>
            {
                var td = OpenTag("td");
                if (x != null)
                    WriteText(x.ToString());
                td.CloseTag();
            });
        }

        public void WriteTag(string tag, string text)
        {
            WriteTag(tag, text, null);
        }

        public void WriteTag(string tag, string text, string cssClass)
        {
            object attributes = null;
            if (cssClass != null)
                attributes = new { @class = cssClass };
            WriteTag(tag, text, attributes);
        }

        public void WriteTag(string tag, string text, object attributes)
        {
            var closer = OpenTag(tag, attributes);
            if (text != null)
                WriteText(text);
            closer.CloseTag();
        }

        public void WriteFileContents(string path)
        {
            using (FileStream stream = File.OpenRead(path))
            {
                WriteFileContents(stream);
            }
        }

        public void WriteFileContents(FileStream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                WriteFileContents(reader);
            }
        }

        public void WriteFileContents(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                _writer.WriteLine(reader.ReadLine());
            }
        }

        public void Dependency(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return;
            }
            else if(url.TrimEnd().EndsWith("js" ))
            {
                WriteTag("script", null, new { src = url, @type = "text/javascript" });
            }
            else if (url.TrimEnd().EndsWith("ico"))
            {
                WriteTag("link", null, new { rel = "shortcut icon", @type = "image/x-icon", href = url });
            }
            else if (url.TrimEnd().EndsWith("css"))
            {
                WriteTag("link", null, new { rel = "stylesheet", @type = "text/css", href = url });
            }
            else
            {
                throw new NotImplementedException("File extension not supported, use 'WriteTag' instead");
            }
        }

        public void Flush()
        {
            _writer.Flush();
        }

        public void Dispose()
        {
            try
            {
                if (_writer != null && _isWriterCreatedInternally)
                {
                    _writer.Dispose();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (_stream != null)
                    _stream.Dispose();
            }
        }
    }

}
