using ConsoleRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRouting.Tests;

[Module]
public class FallbackTestModule
{
    [Command]
    public void CommandOne(string a)
    {

    }

    [Command]
    public void CommandTwo(string a, string b)
    {

    }

    [Command, Fallback]
    public void CommandThree(string[] args)
    {

    }
}


[Module]
public class ConsumingTestModule
{
    [Command]
    public void RemainingArray(string a, string b, string[] rest)
    {

    }

    [Command]
    public void AllArray(string a, string b, [All] string[] rest)
    {

    }

    [Command]
    public void RemainingArguments(string a, string b, Arguments rest)
    {

    }
     
    [Command]
    public void AllArguments(string a, string b, [All] Arguments rest)
    {

    }
}