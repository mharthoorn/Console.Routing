using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleRouting
{
    public class Node
    {
        public string[] Names;

        public Node(IEnumerable<string> names)
        {
            this.Names = names.ToArray();
        }

        public Node(string name)
        {
            this.Names = new string[] { name };
        }

        public bool Matches(string name)
        {
            for (int i = 0; i < Names.Length; i++)
            {
                if (string.Compare(Names[i], name, ignoreCase: true) == 0) return true; 
            }
            return false;
        }

        public override string ToString()
        {
            return string.Join(" | ", Names);
        }
    }

    public static class NodeExtensions
    {
        // 'Retailing' is cloning a node list and adding a tail segment to it.

        //public static List<Node> CloneAndAppend(this List<Node> nodes, MethodInfo method)
        //{
        //    var tail = CreateNode(method);
        //    return nodes.CloneAndAppend(tail);
        //}

        //public static List<Node> CloneAndAppend(this List<Node> nodes, Type type)
        //{
        //    var tail = CreateNode(type);
        //    return nodes.CloneAndAppend(tail);
        //}

        //public static List<Node> Retail(this List<Node> nodes, Command command)
        //{
        //    if (command is null || command.IsGeneric)
        //    {
        //        return nodes.Clone();
        //    }
        //    else
        //    {
        //        var node = new Node(command.Names);
        //        return nodes.Retail(node);
        //    }

        //}

        public static List<Node> Clone(this List<Node> nodes)
        {
            var result = new List<Node>();
            result.AddRange(nodes);
            return result;
        }

        public static List<Node> CloneAndAppend(this List<Node> nodes, Node tail)
        {
            if (tail is null) return nodes;
           
            var clone = nodes.Clone();
            clone.Add(tail);
            return clone;
        }

        public static Node TryCreateRoutingNode(this MethodInfo method)
        {
            var command = method.GetCustomAttribute<Command>();
            if (command is null) return null;

            var names = command.IsGeneric ? new string[] { method.Name } : command.Names;
            var node = new Node(names);
            return node;
        }

        public static Node TryCreateRoutingNode(this Type type)
        {
            var command = type.GetCustomAttribute<Command>();
            if (command is null) return null;

            var names = command.IsGeneric ? new string[] { type.Name } : command.Names;
            var node = new Node(names);
            return node;
        }

        //public static void CreateNode(this Node parent, Command command, Help help)
        //{
        //    if (!command.IsGeneric)
        //    {
        //        var child = new Node(command.Names);
        //        parent.Add(child);
        //    }
        //}

        //public static void CreateNode(this Node parent, Command command, MethodInfo method, Help help)
        //{
        //    var node = command.IsGeneric ? new Node(method.Name) : new Node(command.Names);

        //    node.Endpoint = new Endpoint
        //    {
        //        Method = method,
        //        Commands = Commands(parent, node).ToArray(),
        //        Description = method.GetCustomAttribute<Help>()?.Description
        //    };

        //    parent.Add(node);
        //}

    }
}