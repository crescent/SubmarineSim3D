using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Mogre;
using NUnit.Framework;
using SubjugatorSim;
using SubjugatorSim.Entities;
using SubjugatorSim.src.Xml;

namespace SubjugatorSimTests.lib
{
    [TestFixture]
    public class WorldTest
    {
        [Test]
        public void SerlializationTest()
        {
            var xmlSerializer = new XmlSerializer(typeof(WorldXml));
            var builder = new StringBuilder();
            var writer = new StringWriter(builder);
            var state = new State();
            
            var world = new World(state);
            world.Water = new Water(state);
            world.Water.Position = new Vector3(0,1,2);
            world.Water.Orientation = new Quaternion(1, 2, 3, 4);

            world.Sub = new Sub(state);

            world.WorldObjects = new WorldObjects(state);
            world.WorldObjects.Add(new WorldObject(state, "worldObject1"));
            world.WorldObjects.Add(new WorldObject(state, "worldObject2"));

            xmlSerializer.Serialize(writer,new WorldXml(world));
            Console.WriteLine(builder.ToString());
        }
    }
}