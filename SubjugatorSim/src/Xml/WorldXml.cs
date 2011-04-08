using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.Entities;
using Math=Mogre.Math;

namespace SubjugatorSim.src.Xml
{
    [XmlRoot("World")]
    public class WorldXml
    {
        public WorldXml()
        {
        }

        public WorldXml(World world)
        {
            Water = new WaterXml(world.Water);
            Sub = new SubXml(world.Sub);
            WorldObjects = new WorldObjectsXml(world.WorldObjects);
        }

        public WorldObjectsXml WorldObjects { get; set; }

        public WaterXml Water { get; set; }
        public SubXml Sub { get; set; }

        public World ToEntity(State state)
        {
            var world = new World(state);
            world.CreateInstance();
            world.Water = Water.ToEntity(state);
            world.Sub = Sub.ToEntity(state);
            world.WorldObjects = WorldObjects.ToEntity(state);
            return world;
        }
    }

    public class WorldObjectsXml : List<WorldObjectXml>
    {
        public WorldObjectsXml()
        {
        }

        public WorldObjectsXml(WorldObjects worldObjects)
        {
            if(worldObjects != null)
            foreach (var worldObject in worldObjects)
            {
                Add(new WorldObjectXml(worldObject));
            }
        }

        public WorldObjects ToEntity(State state)
        {
            var worldObjects = new WorldObjects(state);

            foreach (var worldObjectsXml in this)
            {
                worldObjects.Add(worldObjectsXml.ToEntity(state));
            }

            return worldObjects;
        }
    }

    public class WorldObjectXml : BaseEntityXml
    {
        public string Name;
        public string Mesh;
        public float Scale;

        public WorldObjectXml()
        {
        }

        public WorldObjectXml(WorldObject worldObject) : base(worldObject)
        {
            Name = worldObject.Name;
            Mesh = worldObject.Mesh;
            Scale = worldObject.Scale;
        }

        public WorldObject ToEntity(State state)
        {
            WorldObject worldObject = new WorldObject(state, Name); //TODO: FIX name
            worldObject.Populate(this, state);
            worldObject.Scale = Scale;
            worldObject.Mesh = Mesh;
            return worldObject;
        }
    }

    public class BaseEntityXml
    {
        public BaseEntityXml()
        {
        }

        public BaseEntityXml(BaseEntity entity)
        {
            Position = new Vector3Xml(entity.Position);
            Orientation = new QuaternionXml(entity.Orientation);
        }

        public QuaternionXml Orientation { get; set; }
        public Vector3Xml Position { get; set; }
    }

    public class Vector3Xml
    {

        public Vector3Xml(Vector3 vector3)
        {
            this.X = vector3.x;
            this.Y = vector3.y;
            this.Z = vector3.z;
        }

        public Vector3Xml()
        {
        }

        [XmlAttribute]
        public float X;
        
        [XmlAttribute]
        public float Y;

        [XmlAttribute]
        public float Z;

        public Vector3 ToEnitiy()
        {
            return new Vector3(X, Y, Z);
        }
    }

    public class QuaternionXml
    {
        public QuaternionXml()
        {
        }

        public QuaternionXml(Quaternion quaternion)
        {
            this.W = quaternion.w;
            this.X = quaternion.x;
            this.Y = quaternion.y;
            this.Z = quaternion.z;
        }

        [XmlAttribute]
        public float W;

        [XmlAttribute]
        public float X;

        [XmlAttribute]
        public float Y;

        [XmlAttribute]
        public float Z;

        public Quaternion ToEnitiy()
        {
            return new Quaternion(W,X,Y,Z);
        }
    }

    public class SubXml : BaseEntityXml
    {
        public SubXml()
        {
        }

        public SubXml(Sub sub) : base(sub)
        {
            
        }

        public Sub ToEntity(State state)
        {
            var sub = new Sub(state);
            sub.Populate(this,state);
            return sub;
        }
    }

    public class WaterXml : BaseEntityXml
    {
        public WaterXml()
        {
        }

        public WaterXml(Water water) : base(water)
        {
        }

        public Water ToEntity(State state)
        {
            var water = new Water(state);
            water.Populate(this,state);
            return water;
        }
    }
}
