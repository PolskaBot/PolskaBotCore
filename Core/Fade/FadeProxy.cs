using System;
using System.Collections.Generic;
using Flash.External;

namespace PolskaBot.Fade
{
    /// <summary>
    /// Creates proxy that handles Fade Protocol on top of AxShockwaveFlash control.
    /// </summary>
    public class FadeProxy
    {
        public ExternalInterfaceProxy proxy { get; private set; }

        #region Private fields

        private Dictionary<string, FadeProxyClient> _clients = new Dictionary<string, FadeProxyClient>();

        #endregion

        #region Events

        /// <summary>
        /// Reports that both parties reported to be ready for communication.
        /// </summary>
        public event EventHandler<EventArgs> Ready;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes FadeProxy on top of specified AxShockwaveFlash control.
        /// </summary>
        /// <param name="flashControl">AxShockwaveFlash control that contains Fade compatible SWF file.</param>
        public FadeProxy(AxShockwaveFlashObjects.AxShockwaveFlash flashControl)
        {
            proxy = new ExternalInterfaceProxy(flashControl);

            proxy.ExternalInterfaceCall += (s, e) =>
            {
                switch(e.FunctionCall.FunctionName)
                {
                    case "checkStatus":
                        return true;
                    case "callbacksReady":
                        Ready?.Invoke(this, EventArgs.Empty);
                        return null;
                    case "stageOneInitialized":
                        _clients[(string)e.FunctionCall.Arguments[0]]?.HandleCall(e);
                        return null;
                    default:
                        Console.WriteLine(e.FunctionCall.FunctionName);
                        return null;
                }
            };
        }

        #endregion

        #region Client helpers

        /// <summary>
        /// Creates new Client for specified Proxy and control.
        /// </summary>
        /// <returns>Ready to use FadeProxyClient.</returns>
        public FadeProxyClient CreateClient()
        {
            FadeProxyClient client = new FadeProxyClient(this);
            _clients.Add(client.ID, client);
            return client;
        }

        #endregion
    }
}
