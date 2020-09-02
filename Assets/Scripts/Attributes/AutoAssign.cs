using System;
using System.Reflection;
using UnityEngine;

namespace DefaultNamespace.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class AutoAssign : Attribute
    {
        public AutoAssignType AutoAssignType;

        public AutoAssign(AutoAssignType type = AutoAssignType.OnObject)
        {
            AutoAssignType = type;
        }
    }

    public enum AutoAssignType
    {
        OnObject,
        OnChild,
        OnParent,
        Global
    }
}