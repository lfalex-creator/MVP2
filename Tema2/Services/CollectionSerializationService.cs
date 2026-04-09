using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Xml.Serialization;
using Tema2.Models;

namespace Tema2.Services
{
    internal class CollectionSerializationService<T>
    {
        private ObservableCollection<T> collection;
        public CollectionSerializationService(ObservableCollection<T> theCollection)
        {
            collection = theCollection;
        }
        public void Serialize()
        {
            FileStream fileStr = new FileStream($"{typeof(T).ToString().Split(".").Last()}s.xml", FileMode.Create);
            new XmlSerializer(typeof(ObservableCollection<T>)).Serialize(fileStr, collection);
            fileStr.Dispose();
        }
        public void Deserialize()
        {
            FileStream file = new FileStream($"{typeof(T).ToString().Split(".").Last()}s.xml", FileMode.Open);
            ObservableCollection<T> objs = new XmlSerializer(typeof(ObservableCollection<T>)).Deserialize(file) as ObservableCollection<T>;
            foreach(var obj in objs)
                collection.Add(obj);
            file.Dispose();
        }

    }
}
