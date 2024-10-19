using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FeedDeepTweaks
{
    internal class Util
    {
        public static void Message(string message)
        {
            UIMessageManager.instance.DisplayMessage(message, Color.blue, null, null);
        }
    }
}
