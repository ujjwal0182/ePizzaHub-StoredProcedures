using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Core.Settings
{
    public class AssistantSettings
    {
        public string OllamaUrl { get; set; } = "";
        public string Model { get; set; } = "";
        public string McpUrl { get; set; } = "";
        public int MaxIterations { get; set; } = 5;
    }
}
