namespace Animaonline.Events
{
    public abstract class SmartEventArgs { }

    public class SmartEventArgs<T> : SmartEventArgs
    {
        #region Public Properties

        public object Sender { get; set; }
        public T Value { get; set; }

        #endregion

        #region Public Static Methods

        public static SmartEventArgs<T> Create<T>(T value, object sender = null)
        {
            var returnValue = new SmartEventArgs<T>
            {
                Sender = sender,
                Value = value
            };

            return returnValue;
        }

        #endregion

        #region Child Classes

        public abstract class Empty : SmartEventArgs<T> { }

        #endregion
    }
}