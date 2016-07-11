using System;
using System.Collections.Generic;
using uFrame.Editor.Graphs.Data;

namespace uFrame.Editor.Configurations
{
    public class ConfigurationProxyConfiguration : GraphItemConfiguration
    {
        public Func<GenericNode, IEnumerable<GraphItemConfiguration>> ConfigSelector { get; set; }
    }
}