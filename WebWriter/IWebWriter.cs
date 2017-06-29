using System;
using System.Collections.Generic;
using System.IO;

namespace WebWriter
{
    public interface IWebWriter : IDisposable
    {
        /// <summary>
        /// Write a br tag
        /// </summary>
        void BreakLine();

        /// <summary>
        /// Close a tag
        /// </summary>
        /// <param name="tag">Type of element to close</param>
        void CloseTag(string tag);

        /// <summary>
        /// Render a tag to reference a icon, css, or javascript file
        /// </summary>
        /// <param name="url"></param>
        void Dependency(string url);

        /// <summary>
        /// Flush the underlying writer
        /// </summary>
        void Flush();

        /// <summary>
        /// Loop over a collection
        /// </summary>
        /// <typeparam name="T">The element type of the collection</typeparam>
        /// <param name="collection">Collection to iterate over</param>
        /// <param name="callBack">The function to call for each element in the collection</param>
        void ForEach<T>(IEnumerable<T> collection, Action<T> callBack);

        /// <summary>
        /// Loop over a collection
        /// </summary>
        /// <typeparam name="T">The element type of the collection</typeparam>
        /// <param name="containerTag">The type of tag inside of which to render</param>
        /// <param name="collection">Collection to iterate over</param>
        /// <param name="callBack">The function to call for each element in the collection</param>
        void ForEach<T>(string containerTag, IEnumerable<T> collection, Action<T> callBack);

        /// <summary>
        /// Loop over a collection
        /// </summary>
        /// <typeparam name="T">The element type of the collection</typeparam>
        /// <param name="containerTag">The type of tag inside of which to render</param>
        /// <param name="containerClass">Css class(es) to apply to the container element</param>
        /// <param name="collection">Collection to iterate over</param>
        /// <param name="callBack">The function to call for each element in the collection</param>
        void ForEach<T>(string containerTag, string containerClass, IEnumerable<T> collection, Action<T> callBack);

        /// <summary>
        /// Loop over a collection
        /// </summary>
        /// <typeparam name="T">The element type of the collection</typeparam>
        /// <param name="containerTag">The type of tag inside of which to render</param>
        /// <param name="containerAttributes">Html attributes to decorate the container element with</param>
        /// <param name="collection">Collection to iterate over</param>
        /// <param name="callBack">The function to call for each element in the collection</param>
        void ForEach<T>(string containerTag, object containerAttributes, IEnumerable<T> collection, Action<T> callBack);

        /// <summary>
        /// Write one non-breaking space
        /// </summary>
        void Nbsp();

        /// <summary>
        /// Write non-breaking spaces
        /// </summary>
        /// <param name="spaces">The number of spaces to write</param>
        void Nbsp(int spaces);

        /// <summary>
        /// Open a tag
        /// </summary>
        /// <param name="tag">The type of element</param>
        /// <returns>An object with which to close the tag once contents are finished being written</returns>
        Closer OpenTag(string tag);

        /// <summary>
        /// Open a tag
        /// </summary>
        /// <param name="tag">The type of element</param>
        /// <param name="cssClass">The classes to apply to the </param>
        /// <returns>An object with which to close the tag once contents are finished being written</returns>
        Closer OpenTag(string tag, string cssClass);

        /// <summary>
        /// Open a tag
        /// </summary>
        /// <param name="tag">The type of element</param>
        /// <param name="cssClass">The classes to apply to the </param>
        /// <returns>An object with which to close the tag once contents are finished being written</returns>
        Closer OpenTag(string tag, object attributes);

        /// <summary>
        /// Render a row of columns
        /// </summary>
        /// <param name="data">The content of each column</param>
        void TableRow(params object[] data);

        /// <summary>
        /// Do an raw write.
        /// </summary>
        /// <param name="text">The content which will be written without encoding</param>
        void UnencodedWrite(string text);

        /// <summary>
        /// Do an unencoded write of the contents of a file
        /// </summary>
        /// <param name="path">The path at which to find the content to write</param>
        void WriteFileContents(string path);

        /// <summary>
        /// Do an unencoded write of the contents of a file
        /// </summary>
        /// <param name="path">A reader to get the content to write</param>
        void WriteFileContents(StreamReader reader);

        /// <summary>
        /// Do an unencoded write of the contents of a file
        /// </summary>
        /// <param name="path">The stream pointed at  the content to write</param>
        void WriteFileContents(FileStream stream);

        /// <summary>
        /// Write a tag for each item in a collection
        /// </summary>
        /// <typeparam name="T">The type of item in the collection</typeparam>
        /// <param name="tag">The type of element to create</param>
        /// <param name="collection">The collection to be iterated over</param>
        /// <param name="text">A function to get the content of the element</param>
        void WriteForEach<T>(string tag, IEnumerable<T> collection, Func<T, string> text);

        /// <summary>
        /// Write a tag for each item in a collection
        /// </summary>
        /// <typeparam name="T">The type of item in the collection</typeparam>
        /// <param name="tag">The type of element to create</param>
        /// <param name="collection">The collection to be iterated over</param>
        /// <param name="text">A function to get the content of each element</param>
        /// <param name="cssClass">The classes to be applied to each type of element</param>
        void WriteForEach<T>(string tag, string cssClass, IEnumerable<T> collection, Func<T, string> text);

        /// <summary>
        /// Write a tag for each item in a collection
        /// </summary>
        /// <typeparam name="T">The type of item in the collection</typeparam>
        /// <param name="tag">The type of element to create</param>
        /// <param name="collection">The collection to be iterated over</param>
        /// <param name="text">A function to get the content of the element</param>
        /// <param name="attributes">The html attributes to be applied to each element</param>
        void WriteForEach<T>(string tag, object attributes, IEnumerable<T> collection, Func<T, string> text);

        /// <summary>
        /// Open and close a tag
        /// </summary>
        /// <param name="tag">The type of element</param>
        /// <param name="text">The content of the element</param>
        void WriteTag(string tag, string text);

        /// <summary>
        /// Open and close a tag
        /// </summary>
        /// <param name="tag">The type of element</param>
        /// <param name="text">The content of the element</param>
        /// <param name="cssClass">The classes to be applied to the element</param>
        void WriteTag(string tag, string text, string cssClass);

        /// <summary>
        /// Open and close a tag
        /// </summary>
        /// <param name="tag">The type of element</param>
        /// <param name="text">The content of the element</param>
        /// <param name="attributes">The html attributes to be applied to the element</param>
        void WriteTag(string tag, string text, object attributes);

        /// <summary>
        /// Write html encoded text
        /// </summary>
        /// <param name="text">The text to be directly written</param>
        void WriteText(string text);
    }
}