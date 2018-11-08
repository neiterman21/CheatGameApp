using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using CheatGameModel;
using CheatGameApp;

namespace CheatGameApp.Video.Multithreading
{
    public sealed class SaveImageTask : ITask
    {
        public TaskManager TaskManager { get; private set; }
        public TimeSpan Time { get; private set; }
        public Image Image { get; private set; }
        public int ImageIndex { get; private set; }
        public TaskResults Results { get; private set; }
        public string Folder { get { return this.Results.Folder; } }

        public SaveImageTask(TaskManager taskManager, TimeSpan time, Image image, int imageIndex, TaskResults results)
        {
            this.TaskManager = taskManager;
            this.Time = time;
            this.Image = image;
            this.ImageIndex = imageIndex;
            this.Results = results;
        }

        public void Process()
        {
            //save image
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            string imagePath = string.Format(Folder + "\\img{0:00000}_{1:00}_{2:00}_{3:00}_{4:000}.jpg", this.ImageIndex, Time.Hours, Time.Minutes, Time.Seconds, Time.Milliseconds);
            lock (this.Image)
            {
                this.Image.Save(imagePath, JpegEncoding.Codec, JpegEncoding.EncoderParams100);
            }

            //if (MainWindow.IsServer)
            //{
            //    TaskManager.Enqueue(Time, imagePath, Results);
            //}
            this.Image.Dispose();
        }
    }
}
