using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using SubjugatorSim;
using SubjugatorSim.Entities;
using SubjugatorSim.src;

namespace SubjugatorSimTests
{
    [TestClass]
    public class PropertyTreeGridTest
    {
        public PropertyTreeGridTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var grid = new PropertyTreeGrid();
            grid.Bind(new State() {SceneWorld = new World()});
            
            var nodes = grid.TreeView.Nodes;

            Assert.AreEqual(1, nodes.Count);
            Assert.AreEqual(0, nodes[0].Nodes.Count);
            Assert.AreEqual("World", nodes[0].Text);
        }
    }
}
