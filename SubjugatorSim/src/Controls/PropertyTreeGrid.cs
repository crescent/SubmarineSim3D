using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Mogre;
using SubjugatorSim.Code;
using SubjugatorSim.Entities;

namespace SubjugatorSim.src
{
    public partial class PropertyTreeGrid : UserControl
    {
        public PropertyTreeGrid()
        {
            InitializeComponent();
        }

        private void PropertyTreeGrid_Load(object sender, EventArgs e)
        {
            TreeView.AfterSelect += TreeView_AfterSelect;
            TreeView.NodeMouseDoubleClick += TreeView_NodeMouseDoubleClick;

        }

        void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var propertyType = ((IPropertyGridEntity) e.Node.Tag);
            var selectObject = propertyType.CreateInstance();
            Bind(state);
            SelectEntityInTreeview(selectObject);
        }

        private List<TreeNode> TreeViewChildren()
        {
            var children = new List<TreeNode>();

            foreach(TreeNode node in TreeView.Nodes)
                AddChildrenTo(children, node);

            return children;
        }

        private void AddChildrenTo(List<TreeNode> nodes, TreeNode node)
        {
            foreach (TreeNode n in node.Nodes)
            {
                AddChildrenTo(nodes, n);
                nodes.Add(n);
            }
        }

        private void SelectEntityInTreeview(IPropertyGridEntity selectObject)
        {
            foreach (TreeNode node in TreeViewChildren())
                if(node.Tag == selectObject) TreeView.SelectedNode = node;
        }

        void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ShowSelectedNodeInPropertyGrid();
        }

        private State state;

        public void Bind(State state)
        {
            this.state = state;
            var world = state.SceneWorld;

            TreeView.Nodes.Clear();
            AddNode(null,0, new TreeNode(world.Name) { Name=world.Name,  Tag = world });

            var lastNode = TreeView.Nodes[0];

            AddChildren(lastNode, world);

            lastNode.Expand();

            ShowSelectedNodeInPropertyGrid();

            TreeView.NodeMouseClick += (sender, args) => NodeMouseClick(args, state);
            PropertyGrid.PropertyValueChanged += RefreshTreeViewNames;
        }

        protected void RefreshTreeViewNames(object s, PropertyValueChangedEventArgs e)
        {
            foreach (TreeNode node in TreeViewChildren())
                node.Text = (node.Tag as IPropertyGridEntity).Name;
        }

        private void NodeMouseClick(TreeNodeMouseClickEventArgs args, State state)
        {
            var entity = args.Node.Tag as IPropertyGridEntity;
            if(entity.Enabled) state.CameraManager.CameraNode.LookAt(entity.SimNode.Position);
        }

        public IPropertyGridEntity SelectedObject
        {
            get
            {
                return PropertyGrid.SelectedObject as IPropertyGridEntity ?? new NullEntity();
            }
            set
            {
                var previous = SelectedObject;
                var current = value ?? new NullEntity();
                
                previous.SimNode.ShowBoundingBox = false;
                current.SimNode.ShowBoundingBox = true;
                (state ?? new State()).SelectedNode = current.SimNode;

                PropertyGrid.SelectedObject = value;
            }
        }

        private void ShowSelectedNodeInPropertyGrid()
        {
            SelectedObject = TreeView.SelectedNode != null ? (TreeView.SelectedNode.Tag as IPropertyGridEntity) : null;
        }

        void AddNode(TreeNode parent, int index, TreeNode node)
        {
            var nodes = parent==null ? TreeView.Nodes : parent.Nodes;

            var treeNodes = nodes.Find(node.Name, true);
            if (treeNodes.Length == 0) nodes.Insert(index,node);
            else treeNodes[0].ForeColor = node.ForeColor;
        }

        private void AddChildren(TreeNode parent, IPropertyGridEntity o)
        {
            if(! (o is IPropertyGridEntityParent)) return;

            var children = (o as IPropertyGridEntityParent).Children;
            for (int i = 0; i < children.Count; i++)
            {
                var child = children[i];

                if (child == null) continue;

                AddNode(parent, i, new TreeNode(child.Name) { Name = child.Name, Tag = child, ForeColor = child.Enabled ? Color.Black : Color.Gray });

                AddChildren(parent.LastNode, child);
            }
        }

        public TreeView TreeView
        {
            get { return treeView;  }
        }

        public PropertyGrid PropertyGrid
        {
            get { return propertyGrid;  }
        }
    }

    public interface IPropertyGridEntity
    {
        string Name { get; }
        IPropertyGridEntity CreateInstance();
        bool Enabled { get; }
        SimNode SimNode { get; }
    }

    internal interface IPropertyGridEntityParent : IPropertyGridEntity
    {
            List<IPropertyGridEntity> Children { get; }
    }
}
