using System;
using System.Collections.Generic;
using System.Windows;

namespace Talamus_ContentManager.Models
{
    [Serializable]
    public class BookSave
    {
        public BookSave()
        {

        }
        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }
        public List<PartSave> Parts { get; set; }
        public List<ConnectionSave> Connections { get; set; }
    }

    [Serializable]
    public class PartSave
    {
        public PartSave()
        {

        }
        public Guid Guid { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Point Position { get; set; }
        public bool FirstPage { get; set; }
        public bool Demo { get; set; }
    }

    [Serializable]
    public class ConnectionSave
    {
        public ConnectionSave()
        {

        }
        public Point Start { get; set; }
        public Point End { get; set; }
        public Guid StartGuid { get; set; }
        public Guid EndGuid { get; set; }
    }

}
