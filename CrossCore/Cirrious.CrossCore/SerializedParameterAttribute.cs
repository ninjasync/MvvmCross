#if !DOT42
using System;

namespace Cirrious.CrossCore
{
    /// <summary>
    /// (This is a Dot42-specific attribute)
    /// <para/>
    /// Specifies this parameter is used in serialization. Types and objects passed
    /// as this parameter will have all their public fields and public and private 
    /// properties preserved and not pruned. 
    /// <para/>
    /// <para/>
    /// Can be used on normal and generic method parameters.
    /// <para>
    /// Propagates recursivly to the types of fields and properties of the passed type.
    /// </para>
    /// <para/>
    /// Note that this Attribute is not inherited when used on generic method parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.GenericParameter, Inherited = true, AllowMultiple = false)]
    public class SerializedParameterAttribute : Attribute
    {
    }
}
#endif
