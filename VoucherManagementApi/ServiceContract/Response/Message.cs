namespace VoucherManagementApi.ServiceContract.Response
{
    public class Message
    {
        #region Properties

        public CoreEnum.MessageType Type { get; set; }

        public string MessageText { get; set; }

        #endregion
    }

    public class CoreEnum
    {
        #region MessageType enum

        public enum MessageType
        {
            Error,
            Info,
            Warning
        }

        #endregion
    }
}
