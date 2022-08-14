using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talamus_ContentManager.Models
{
    [Serializable]
    public class Settings
    {
        public Settings()
        {

        }
        public string DBPath { get; set; }
        public string BotToken { get; set; }
    }
}
