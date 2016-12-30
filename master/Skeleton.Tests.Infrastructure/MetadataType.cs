namespace Skeleton.Tests.Infrastructure
{
    public class MetadataType
    {
        private int _field;
        public int Property
        {
            get; set;
        }

        public int Method()
        {
            return _field + Property;
        }

        public int Field;

        private int _writeOnlyProperty;
        public int WriteOnlyProperty
        {
            set
            {
                _writeOnlyProperty = value;
            }
        }

        private int _readOnlyProperty;
        public int ReadOnlyProperty
        {
            get
            {
                return _readOnlyProperty;
            }
        }
    }
}