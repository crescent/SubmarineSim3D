using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.src;
using SubjugatorSim.src.Xml;

namespace SubjugatorSim.Entities
{
    public class World : IPropertyGridEntityParent
    {
        private readonly State state;
        private bool enabled = false;
        public bool Physics { get; set; }

        public Water Water { get; set; }

        public Sub Sub { get; set; }

        public WorldObjects WorldObjects { get; set; }

        #region IPropertyGridEntityParent Members

        public string Name
        {
            get { return "World"; }
        }

        public World(State state)
        {
            this.state = state;
        }

        public IPropertyGridEntity CreateInstance()
        {
            enabled = true;
            Water = new Water(state);
            Sub = new Sub(state);
            WorldObjects = new WorldObjects(state);
            new SceneCreator().Init(state);
            return this;
        }

        public bool Enabled
        {
            get { return enabled; }
        }

        public SimNode SimNode
        {
            get { return new NullSimNode(); }  //state.SceneManager.RootSceneNode;  }
        }

        public List<IPropertyGridEntity> Children
        {
            get
            {
                var list = new List<IPropertyGridEntity>();

                list.Add(Water);
                list.Add(Sub);

                if(WorldObjects != null) 
                    foreach (WorldObject worldObject in WorldObjects)
                        list.Add(worldObject);

                list.Add(WorldObjects);

                return list;
            }
        }

        public void Save(string filename)
        {
            var writer = new StreamWriter(filename);
            var worldXml = new WorldXml(state.SceneWorld);
            new XmlSerializer(typeof(WorldXml)).Serialize(writer, worldXml);
            writer.Close();
        }
        #endregion

        public void Load(string fileName)
        {
            var xmlSerializer = new XmlSerializer(typeof(WorldXml));
            var streamReader = new StreamReader(fileName);
            var worldXml = xmlSerializer.Deserialize(streamReader) as WorldXml;
            streamReader.Close();
            state.SceneWorld = worldXml.ToEntity(state);
        }
    }

    public class WorldObjects : List<WorldObject>, IPropertyGridEntity
    {
        private readonly State state;
        private static int count = 0;

        #region IPropertyGridEntity Members

        public WorldObjects(State state)
        {
            this.state = state;
        }

        public string Name
        {
            get { return "WorldObject"; }
        }

        public IPropertyGridEntity CreateInstance()
        {
            while (Find(o => o.Name == "WorldObject" + count) != null) count++;
            var worldObject = new WorldObject(state, "WorldObject" + count);
            worldObject.CreateInstance();
            Add(worldObject);
            return worldObject;
        }

        public bool Enabled
        {
            get { return false; }
        }

        public SimNode SimNode
        {
            get { return new NullSimNode();  }
        }

        #endregion
    }
}