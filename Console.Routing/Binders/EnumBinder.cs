﻿using System;

namespace ConsoleRouting;


public class EnumBinder : IBinder
{
    public bool Optional => false;

    public bool Match(Type type) => type.IsEnum;

    public BindStatus TryUse(Arguments arguments, Parameter param, int index, out object result)
    {
        if (arguments.TryUseEnum(index, param, out var value))
        {
            result = value;
            return BindStatus.Success;
        }
        else
        {
            result = null;
            return BindStatus.Failed;
        }
    }
}

