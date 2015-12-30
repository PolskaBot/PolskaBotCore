using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flash.External;

namespace PolskaBot.Fade
{
    class FadeProxy
    {
        private ExternalInterfaceProxy _proxy;

        public event EventHandler<EventArgs> Ready;

        public FadeProxy(AxShockwaveFlashObjects.AxShockwaveFlash flashControl)
        {
            _proxy = new ExternalInterfaceProxy(flashControl);

            _proxy.ExternalInterfaceCall += (s, e) =>
            {
                switch(e.FunctionCall.FunctionName)
                {
                    case "checkStatus":
                        return true;
                    case "callbacksReady":
                        Ready?.Invoke(this, EventArgs.Empty);
                        return null;
                    default:
                        Console.WriteLine(e.FunctionCall.FunctionName);
                        return null;
                }
            };
        }
    }
}
