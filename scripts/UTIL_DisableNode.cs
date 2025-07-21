using Godot;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class UTIL_DisableNode : Node
{
    private static Dictionary<string, List<Node>> _stateGroups = new();

    public static void CreateStateGroup(string groupName, params Node[] nodes)
    {
        if (_stateGroups.ContainsKey(groupName))
        {
            //GD.PushWarning($"State group '{groupName}' already exists. Overwriting.");
        }

        _stateGroups[groupName] = new List<Node>(nodes);

        foreach (var node in nodes)
        {
            DeepDisable(node);
        }
    }

    public static void SwitchState(string groupName, Node toEnable)
    {
        GD.Print("trying to switch");
        if (!_stateGroups.TryGetValue(groupName, out var group))
        {
            GD.PushError($"State group '{groupName}' not found!");
            return;
        }

        foreach (var node in group)
        {
            if (node == toEnable)
                DeepEnable(node);
            else
                DeepDisable(node);
        }
    }

    public static void DisableStateGroup(string groupName)
    {
        if (!_stateGroups.TryGetValue(groupName, out var groupNodes))
        {
            GD.PushWarning($"State group '{groupName}' does not exist.");
            return;
        }

        foreach (var node in groupNodes)
        {
            DeepDisable(node);
        }
    }

    public static void DeepDisable(Node node)
    {
        if (node == null)
        {
            throw new Exception("No node found to disable!");
        }

        if (node is CanvasItem canvasItem)
        {
            canvasItem.Visible = false;
        }

        if (node is CollisionShape3D colliisonShape)
        {
            colliisonShape.Disabled = true;
        }

        if (node is Area3D area)
        {
            area.Monitorable = false;
        }

        if (node is CanvasItem item)
        {
            item.Visible = false;
        }

        node.ProcessMode = Node.ProcessModeEnum.Disabled;

        foreach (Node child in node.GetChildren())
        {
            DeepDisable(child);
        }
    }
    public static void DeepEnable(Node node)
    {
        if (node == null) return;

        if (node is CanvasItem canvasItem)
        {
            canvasItem.Visible = true;
        }

        if (node is CollisionShape3D collisionShape)
        {
            collisionShape.Disabled = false;
        }

        if (node is Area3D area)
        {
            area.Monitorable = true;
        }

        node.ProcessMode = Node.ProcessModeEnum.Inherit;

        foreach(Node child in node.GetChildren())
        {
            DeepEnable(child);
        }
    }
}
