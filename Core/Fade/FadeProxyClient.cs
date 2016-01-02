using System;
using Flash.External;

namespace PolskaBot.Fade
{
    /// <summary>
    /// FadeProxyClient represents single client that uses encryption.
    /// </summary>
    public class FadeProxyClient
    {
        #region Properties

        /// <summary>
        /// Unique identifier of client.
        /// </summary>
        public string ID { get; set; } = Guid.NewGuid().ToString();

        #endregion

        #region Private fields

        private FadeProxy _proxy;

        #endregion

        #region Events

        /// <summary>
        /// Reports that asynchronous stage one has been loaded.
        /// </summary>
        public event EventHandler<EventArgs> StageOneLoaded;

        /// <summary>
        /// Reports that asynchronous stage one has occurred error when loading.
        /// </summary>
        public event EventHandler<EventArgs> StageOneFailed;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates single client for spicified proxy.
        /// </summary>
        /// <param name="proxy">FadeProxy to which client belongs.</param>
        public FadeProxyClient(FadeProxy proxy)
        {
            _proxy = proxy;
        }

        #endregion

        #region Connection

        public void Disconnect()
        {
            _proxy.proxy.Call("disconnect", ID);
        }

        #endregion

        #region Handling calls

        /// <summary>
        /// Handles calls made from Fade targeted to this client.
        /// </summary>
        /// <param name="eventArgs">Event arguments that contain information about call.</param>
        /// <returns></returns>
        public object HandleCall(ExternalInterfaceCallEventArgs eventArgs)
        {
            if(eventArgs.FunctionCall.FunctionName.Equals("stageOneInitialized"))
            {
                if ((bool)eventArgs.FunctionCall.Arguments[1] == true)
                    StageOneLoaded?.Invoke(this, EventArgs.Empty);
                else
                    StageOneFailed?.Invoke(this, EventArgs.Empty);
            }
            return null;
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Resets both stage one and stage two.
        /// </summary>
        public void Reset()
        {
            _proxy.proxy.Call("reset", ID);
        }

        /// <summary>
        /// Initializes stage one with specified code.
        /// </summary>
        /// <param name="code">Code which will be used for initialization.</param>
        public void InitStageOne(byte[] code)
        {
            _proxy.proxy.Call("initStageOne", ID, Convert.ToBase64String(code));
        }

        /// <summary>
        /// Generates public key for encryption.
        /// </summary>
        /// <returns>Public key for encryption</returns>
        public byte[] GenerateKey()
        {
            return Convert.FromBase64String((string)_proxy.proxy.Call("generateKey", ID));
        }

        /// <summary>
        /// Initializes stage two with specified code.
        /// </summary>
        /// <param name="code">Code which will be used for initialization.</param>
        public void InitStageTwo(byte[] code)
        {
            _proxy.proxy.Call("initStageTwo", ID, Convert.ToBase64String(code));
        }

        #endregion

        #region Encryption

        /// <summary>
        /// Encrypts specified input
        /// </summary>
        /// <param name="input">Input to be encrypted</param>
        /// <returns>Encrypted input</returns>
        public byte[] Encrypt(byte[] input)
        {
            return Convert.FromBase64String((string)_proxy.proxy.Call("encode", ID, Convert.ToBase64String(input)));
        }

        /// <summary>
        /// Decrypts specified input
        /// </summary>
        /// <param name="input">Decrypted input</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] input)
        {
            return Convert.FromBase64String((string)_proxy.proxy.Call("decode", ID, Convert.ToBase64String(input)));
        }

        #endregion
    }
}
