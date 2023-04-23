using System;

namespace ConsoleRouting.Tests
{
    public enum Component
    {
        Major,
        Minor,
        Patch,
        Pre
    }

    [Module("Tool")]
    public class ToolModule
    {
        [Command, Default]
        public void Info([Alt("?")] Flag help)
        {
            Console.WriteLine("Info");
        }

        [Command]
        public void Tool()
        {

        }

        [Command]
        public void Action(string name)
        { 
        }

        [Command] 
        public void Action(string name, string alias, Flag foo, Flag<string> bar)
        {
            
        }

        [Command]
        public void Save([Optional]string filename, Flag all, Flag json, Flag xml, Flag<string> pattern)
        {
            Console.WriteLine("Saving");
        }

        [Command]
        public void CmdWithDefault(bool apply = true)
        {

        }

        [Command]
        public void ShortFlagBug(Flag values, Flag actions, Flag variables)
        {
            //(p, l, s, v, f, m, a, v) (note the twice: v)
            //Won't bind on -v
            //Won't bind on -va
            //Won't bind on -r
            //Won't bind on -aq

            //Will bind on -var
            //Will bind on -vr
            //Will bind on -vq
            //Will bind on -aqv (does actions, variables, source)

        }

        [Command]
        public void ParamCaseSensitive(Flag values, Flag multiline, Flag Actions, Flag Variables)
        {
            //(p, l, s, v, f, m, a, v) (note the twice: v)
            //Won't bind on -v
            //Won't bind on -va
            //Won't bind on -r
            //Won't bind on -aq

            //Will bind on -var
            //Will bind on -vr
            //Will bind on -vq
            //Will bind on -aqv (does actions, variables, source)

        }
    }
    
}
